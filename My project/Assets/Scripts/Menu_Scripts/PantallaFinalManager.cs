using UnityEngine;
using TMPro;
using System;

public class PantallaFinalManager : MonoBehaviour
{
    [Header("Referencias de Texto (Campaña Normal)")]
    public TextMeshProUGUI txtTiempoActual;
    public TextMeshProUGUI txtMejorTiempo;
    public TextMeshProUGUI txtPuntajeActual;
    public TextMeshProUGUI txtMejorPuntaje;

    [Header("Contenedor de Hitos/Logros")]
    public Transform contenedorHitos;
    public GameObject prefabTextoHito;

    void Start()
    {
        // Forzamos que el mouse sea visible para navegar los menús finales
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // --- PASO 1: PRIORIZAR LA LECTURA DEL ARCHIVO DE TEXTO (.TXT) ---
        var datos = ManejadorGuardadoTexto.CargarDatos();

        float tiempoFinal = datos.partidaTiempoActual;
        int puntajeCalculado = datos.partidaPuntajeActual;
        int poderesUsados = 0;

        // Red de seguridad: Si por algo el .txt marca cero pero el SkillManager sigue en RAM
        if (tiempoFinal == 0f && SkillManager.Instance != null)
        {
            tiempoFinal = SkillManager.Instance.tiempoNivel;
            puntajeCalculado = (int)SkillManager.Instance.skillActual;
        }

        if (SkillManager.Instance != null)
        {
            poderesUsados = SkillManager.Instance.poderesUtilizados;
        }

        // --- PASO 2: CÁLCULO INTERNO DE LOS BONOS ---
        bool metioBonoTiempo = tiempoFinal < 30f && tiempoFinal > 0f;
        bool metioBonoPoderes = poderesUsados <= 1;
        bool metioBonoCondicion = false;

        if (metioBonoTiempo) puntajeCalculado += 1;
        if (metioBonoPoderes) puntajeCalculado += 4;

        // --- PASO 3: GESTIÓN DE RÉCORDS HISTÓRICOS EN EL BLOC DE NOTAS ---
        float mejorTiempoHistorico = datos.recordMejorTiempoNormal;
        int mejorPuntajeHistorico = datos.recordMejorPuntajeNormal;

        bool nuevoRecordTiempo = false;
        bool nuevoRecordPuntaje = false;

        if (tiempoFinal > 0f && tiempoFinal < mejorTiempoHistorico)
        {
            mejorTiempoHistorico = tiempoFinal;
            datos.recordMejorTiempoNormal = tiempoFinal;
            nuevoRecordTiempo = true;
        }

        if (puntajeCalculado > mejorPuntajeHistorico)
        {
            mejorPuntajeHistorico = puntajeCalculado;
            datos.recordMejorPuntajeNormal = puntajeCalculado;
            nuevoRecordPuntaje = true;
        }

        // Guardamos los datos finales en el save_data.txt
        ManejadorGuardadoTexto.GuardarDatos(
            tiempoFinal,
            mejorTiempoHistorico,
            puntajeCalculado,
            mejorPuntajeHistorico
        );

        // --- PASO 4: MOSTRAR DATOS EN INTERFAZ ---
        txtTiempoActual.text = "Tiempo Total: " + FormatearTiempo(tiempoFinal) + (nuevoRecordTiempo ? " ¡NUEVO RECORD!" : "");
        txtMejorTiempo.text = "Mejor Tiempo: " + (mejorTiempoHistorico >= 999998f ? "--:--:--" : FormatearTiempo(mejorTiempoHistorico));

        txtPuntajeActual.text = "Puntaje Obtenido: " + puntajeCalculado + " pts" + (nuevoRecordPuntaje ? " ¡NUEVO RECORD!" : "");
        txtMejorPuntaje.text = "Mejor Puntaje: " + mejorPuntajeHistorico + " pts";

        DesplegarHitos(metioBonoTiempo, metioBonoPoderes, metioBonoCondicion);
    }

    private void DesplegarHitos(bool bonoTiempo, bool bonoPoderes, bool bonoCondicion)
    {
        foreach (Transform hijo in contenedorHitos) Destroy(hijo.gameObject);

        if (bonoTiempo)
            CrearTextoHito("Hito de Velocidad: ¡Pasaste el nivel en menos de 30 segundos! (+1 pt)");

        if (bonoPoderes)
            CrearTextoHito("Hito Minimalista: Completaste el nivel usando maximo 1 poder. (+4 pts)");

        if (bonoCondicion)
            CrearTextoHito("Reglas de la Casa: Superaste la adversidad con una Condicion activa. (+1 pt)");

        if (contenedorHitos.childCount == 0)
        {
            CrearTextoHito("Sin hitos adicionales en esta carrera. ¡Intenta ir mas rapido o usar menos poderes!");
        }
    }

    private void CrearTextoHito(string mensaje)
    {
        GameObject nuevoHito = Instantiate(prefabTextoHito, contenedorHitos);
        nuevoHito.GetComponent<TextMeshProUGUI>().text = mensaje;
    }

    private string FormatearTiempo(float tiempoEnSegundos)
    {
        if (tiempoEnSegundos <= 0) return "00:00:00";
        TimeSpan t = TimeSpan.FromSeconds(tiempoEnSegundos);
        return string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
    }


    public void VolverAlTutorialYReiniciar()
    {
        Time.timeScale = 1f;
        ControlarPausaMenu.CronometroActivo = false;

        // 1. Dejar limpio el archivo de texto para que la nueva partida empiece en ceros
        var datos = ManejadorGuardadoTexto.CargarDatos();
        ManejadorGuardadoTexto.GuardarDatos(
            0f,
            datos.recordMejorTiempoNormal,
            0,
            datos.recordMejorPuntajeNormal
        );

        Debug.Log("<color=cyan>[HARD RESET]</color> Destruyendo instancias persistentes en RAM...");

        // 2. DETONACIÓN DE MEMORIA: Buscamos y destruimos CUALQUIER clon persistente que cause conflicto
        // Buscamos tus componentes específicos por tipo y eliminamos su objeto entero de la RAM
        if (SkillManager.Instance != null) Destroy(SkillManager.Instance.gameObject);
        if (LevelLoader.Instance != null) Destroy(LevelLoader.Instance.gameObject);

        // Usamos FindObjectOfType para aquellos que puedan estar en escena o tengan Singletons diferentes
        var logicManager = FindObjectOfType<Logic_Manager>();
        if (logicManager != null) Destroy(logicManager.gameObject);

        var condicionesManager = FindObjectOfType<CondicionesManager>();
        if (condicionesManager != null) Destroy(condicionesManager.gameObject);

        var reRollManager = FindObjectOfType<ReRollManager>();
        if (reRollManager != null) Destroy(reRollManager.gameObject);

        // 3. Forzamos la carga directa de la escena usando el SceneManager nativo.
        // Al haber destruido los managers viejos, la nueva escena creará sus versiones limpias desde cero.
        Debug.Log("<color=yellow>[HARD RESET]</color> RAM Limpia. Cargando escena de Tutorial original...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lvl_Based_Tutorial");
    }
}
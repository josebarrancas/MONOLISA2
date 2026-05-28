using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerSelectorPrincipal : MonoBehaviour
{
    [Header("Base de Datos de Poderes")]
    public List<PoderData> todosLosPoderes = new List<PoderData>();

    // CAMBIO: Ahora es public para que el Manager filtre los duplicados y lea qué poderes tiene equipados
    public List<PoderData> poderesEnUso = new List<PoderData>();
    private int indiceActual;

    [Header("Referencias UI")]
    public RectTransform circuloAzul;
    public CanvasGroup grupoInformacion;
    public RectTransform iconoPoderActual;

    [Header("Textos del Poder")]
    public TextMeshProUGUI txtNombre;
    public TextMeshProUGUI txtEtiquetas;
    public TextMeshProUGUI txtExplicacion;
    public TextMeshProUGUI txtCargas;

    [Header("Configuración Círculo")]
    public Vector3 escalaExpandida = new Vector3();

    [Header("Configuración Icono")]
    public Vector3 escalaIconoExpandido = new Vector3();
    public float desplazamientoDerecha;

    [Header("Animación")]
    public float velocidadSuavizado = 10f;

    private bool expandido = false;
    private Vector3 posicionOriginalIcono;

    void Start()
    {
        if (iconoPoderActual != null)
            posicionOriginalIcono = iconoPoderActual.anchoredPosition;

        PrepararPartida();
    }

    public void PrepararPartida()
    {
        if (Logic_Manager.instance == null)
        {
            Debug.LogError("No se encontró el Logic_Manager en la escena.");
            return;
        }

        // CAMBIO CRÍTICO: Ahora le pedimos los 3 poderes finales, no los 9
        List<PoderData> poderesAsignados = Logic_Manager.instance.poderesSeleccionados;

        // RED DE SEGURIDAD (Para cuando haces pruebas directas en el nivel)
        if (poderesAsignados == null || poderesAsignados.Count == 0)
        {
            Debug.LogWarning("No se hicieron selecciones en ReRoll. Cargando poderes de respaldo para pruebas.");
            if (Logic_Manager.instance.mazoJugador != null && Logic_Manager.instance.mazoJugador.Count >= 3)
            {
                poderesAsignados = Logic_Manager.instance.mazoJugador.GetRange(0, 3);
            }
            else
            {
                return;
            }
        }

        poderesEnUso.Clear();

        // Clonamos los 3 poderes seleccionados
        foreach (PoderData poder in poderesAsignados)
        {
            poderesEnUso.Add(Instantiate(poder));
        }

        Debug.Log($"<color=cyan>HUD configurado con los {poderesEnUso.Count} poderes elegidos en el ReRoll.</color>");

        indiceActual = 0;
        ActualizarVisualPoder(poderesEnUso[indiceActual]);
    }

    void Update()
    {
        if (this == null || circuloAzul == null) return;

        // ==========================================================
        // CANDADO DE SEGURIDAD: MENÚ DE PAUSA (CAMPAÑA)
        // ==========================================================
        if (ControlarPausaMenu.IsPausado)
        {
            // Forzamos el cierre si el menú de pausa interrumpe la selección
            expandido = false;
        }
        else
        {
            // Si el juego NO está en pausa, procesamos la entrada normalmente
            expandido = Input.GetKey(KeyCode.C);

            if (expandido)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) CambiarPoder(-1);
                if (Input.GetKeyDown(KeyCode.RightArrow)) CambiarPoder(1);
            }
        }

        // Mantenemos las interpolaciones (Lerp) corriendo sin escala de tiempo 
        // para asegurar un cierre limpio y fluido durante la pausa
        float delta = Time.unscaledDeltaTime * velocidadSuavizado;

        Vector3 objetivoCirculo = expandido ? escalaExpandida : Vector3.one;
        circuloAzul.localScale = Vector3.Lerp(circuloAzul.localScale, objetivoCirculo, delta);

        Vector3 objetivoEscalaIcono = expandido ? escalaIconoExpandido : Vector3.one;
        iconoPoderActual.localScale = Vector3.Lerp(iconoPoderActual.localScale, objetivoEscalaIcono, delta);

        Vector3 objetivoPosicionIcono = expandido ?
            posicionOriginalIcono + new Vector3(desplazamientoDerecha, 0, 0) :
            posicionOriginalIcono;

        iconoPoderActual.anchoredPosition = Vector3.Lerp(iconoPoderActual.anchoredPosition, objetivoPosicionIcono, delta);

        grupoInformacion.alpha = Mathf.Lerp(grupoInformacion.alpha, expandido ? 1 : 0, delta);
    }

    void CambiarPoder(int direccion)
    {
        if (poderesEnUso == null || poderesEnUso.Count == 0)
        {
            Debug.LogWarning("Lista de poderes vacía en el HUD.");
            return;
        }

        // Esta fórmula matemática funciona perfecto para 3 poderes
        indiceActual = (indiceActual + direccion + poderesEnUso.Count) % poderesEnUso.Count;
        ActualizarVisualPoder(poderesEnUso[indiceActual]);
    }

    public void ActualizarVisualPoder(PoderData datos)
    {
        if (datos == null)
        {
            Debug.LogWarning("Intentando actualizar HUD sin datos de poder.");
            return;
        }

        txtNombre.text = datos.nombre;
        txtEtiquetas.text = string.Join(" / ", datos.etiquetas);
        txtExplicacion.text = datos.explicacion;

        if (datos.tipo == TipoUso.Varios)
            txtCargas.text = "Usos: " + datos.cantidadUsos;
        else
            txtCargas.text = "";

        if (datos.icono != null)
            iconoPoderActual.GetComponent<Image>().sprite = datos.icono;
    }

    public string ObtenerNombrePoderActual()
    {
        if (poderesEnUso == null || poderesEnUso.Count == 0)
        {
            return "";
        }

        return poderesEnUso[indiceActual].nombre;
    }

    public void RegistrarUsoDePoder()
    {
        PoderData actual = poderesEnUso[indiceActual];
        if (actual.tipo == TipoUso.Varios && actual.cantidadUsos > 0)
        {
            actual.cantidadUsos--;
            ActualizarVisualPoder(actual);
        }
    }

    public PoderData IntercambiarPoderActivo(PoderData nuevoPoder)
    {
        // 1. Guardamos una copia del poder que el jugador tiene actualmente en el slot
        PoderData poderViejo = poderesEnUso[indiceActual];

        // 2. Sobrescribimos el slot del jugador con el poder de la caja
        poderesEnUso[indiceActual] = nuevoPoder;

        // 3. Forzamos la actualización visual de la UI de este slot
        ActualizarVisualPoder(poderesEnUso[indiceActual]);

        // 4. Devolvemos el poder viejo para que la caja se lo quede
        return poderViejo;
    }
}
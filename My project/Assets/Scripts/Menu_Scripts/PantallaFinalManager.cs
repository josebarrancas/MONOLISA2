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
        // --- PASO 1: PRIORIZAR LA LECTURA DEL DISCO (PLAYERPREFS) ---
        // Jalamos el tiempo congelado en la meta para ganarle al reinicio de los scripts persistentes
        float tiempoFinal = PlayerPrefs.GetFloat("Partida_TiempoActual", 0f);
        int puntajeCalculado = PlayerPrefs.GetInt("Partida_PuntajeActual", 50);
        int poderesUsados = 0;

        // Red de seguridad: Si por algo el PlayerPrefs falló pero el SkillManager sigue vivo en RAM
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

        // --- PASO 3: GESTIÓN DE RÉCORDS HISTÓRICOS ---
        float mejorTiempoHistorico = PlayerPrefs.GetFloat("Record_MejorTiempo_Normal", float.MaxValue);
        int mejorPuntajeHistorico = PlayerPrefs.GetInt("Record_MejorPuntaje_Normal", 0);

        bool nuevoRecordTiempo = false;
        bool nuevoRecordPuntaje = false;

        if (tiempoFinal > 0f && tiempoFinal < mejorTiempoHistorico)
        {
            PlayerPrefs.SetFloat("Record_MejorTiempo_Normal", tiempoFinal);
            mejorTiempoHistorico = tiempoFinal;
            nuevoRecordTiempo = true;
        }

        if (puntajeCalculado > mejorPuntajeHistorico)
        {
            PlayerPrefs.SetInt("Record_MejorPuntaje_Normal", puntajeCalculado);
            mejorPuntajeHistorico = puntajeCalculado;
            nuevoRecordPuntaje = true;
        }

        PlayerPrefs.Save();

        // --- PASO 4: MOSTRAR DATOS EN INTERFAZ (TEXTO PLANO) ---
        txtTiempoActual.text = "Tiempo Total: " + FormatearTiempo(tiempoFinal) + (nuevoRecordTiempo ? " ¡NUEVO RECORD!" : "");
        txtMejorTiempo.text = "Mejor Tiempo: " + (mejorTiempoHistorico == float.MaxValue ? "--:--:--" : FormatearTiempo(mejorTiempoHistorico));

        txtPuntajeActual.text = "Puntaje Obtenido: " + puntajeCalculado + " pts" + (nuevoRecordPuntaje ? " ¡NUEVO RECORD!" : "");
        txtMejorPuntaje.text = "Mejor Puntaje: " + mejorPuntajeHistorico + " pts";

        // --- PASO 5: DESPLEGAR LOS HITOS ---
        DesplegarHitos(metioBonoTiempo, metioBonoPoderes, metioBonoCondicion);
    }

    private void DesplegarHitos(bool bonoTiempo, bool bonoPoderes, bool bonoCondicion)
    {
        // Limpieza de seguridad del contenedor vertical
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
}
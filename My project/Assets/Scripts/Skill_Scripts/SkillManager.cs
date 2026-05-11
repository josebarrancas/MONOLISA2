using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("Algoritmo Skill (Estado Actual)")]
    [Range(0, 100)] public float skillActual = 50f;
    public string estadoActual = "Based";

    [Header("Métricas del Nivel (Debug)")]
    public int intentosNivel = 1; // Movimiento.cs incrementa esto al morir
    public float tiempoInicio;
    public int poderesUtilizados;

    // Diccionario de preferencias (Sigue funcionando igual)
    public Dictionary<string, int> historialPoderes = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            ReiniciarMetricas();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReiniciarMetricas()
    {
        tiempoInicio = Time.time;
        poderesUtilizados = 0;
        // No reiniciamos intentosNivel aquí, eso se hace después de calcular resultados
    }

    public void RegistraUsoPoder(string nombrePoder)
    {
        poderesUtilizados++;

        if (historialPoderes.ContainsKey(nombrePoder))
            historialPoderes[nombrePoder]++;
        else
            historialPoderes.Add(nombrePoder, 1);

        Debug.Log($"<color=green>SkillManager:</color> Poder '{nombrePoder}' registrado ({historialPoderes[nombrePoder]} usos).");
    }

    public void CalcularResultados()
    {
        float tiempoFinal = Time.time - tiempoInicio;

        // 1. PUNTOS POR INTENTOS (Dificultad)
        int pIntentos = 0;
        if (intentosNivel == 1) pIntentos = 15; // Gran bonus por pasar a la primera
        else if (intentosNivel <= 3) pIntentos = 5;
        else if (intentosNivel <= 5) pIntentos = 0;
        else if (intentosNivel <= 8) pIntentos = -10;
        else pIntentos = -20;

        // 2. PUNTOS POR TIEMPO (Fluidez)
        int pTiempo = 0;
        if (tiempoFinal < 45f) pTiempo = 10;
        else if (tiempoFinal <= 90f) pTiempo = 5;
        else if (tiempoFinal <= 180f) pTiempo = 0;
        else pTiempo = -10;

        // 3. PUNTOS POR PODERES (Maestría)
        int pPoder = 0;
        if (poderesUtilizados == 0) pPoder = 15; // Jugador rústico/habilidoso
        else if (poderesUtilizados <= 3) pPoder = 5;
        else if (poderesUtilizados <= 6) pPoder = 0;
        else pPoder = -10;

        // 4. NUEVA FÓRMULA DE SKILL (Ajuste por Desviación)
        // La fórmula anterior (0.7 + 0.3) tendía a bajar el skill drásticamente.
        // Ahora usamos un factor de cambio basado en el desempeño:
        float puntuacionNivel = pIntentos + pTiempo + pPoder;

        // El skill sube o baja suavemente según la puntuación (máximo +/- 15 puntos por nivel)
        float factorCambio = puntuacionNivel * 0.4f;
        skillActual = Mathf.Clamp(skillActual + factorCambio, 0, 100);

        // 5. DETERMINAR EL ESTADO
        if (skillActual < 40) estadoActual = "Issue";      // El jugador está sufriendo (Ayudarle)
        else if (skillActual > 70) estadoActual = "Solution"; // El jugador es un pro (Castigarle)
        else estadoActual = "Based";                         // Flujo normal

        Debug.Log($"<color=cyan>--- SKILL REPORT ---</color>\n" +
                  $"Puntos: Intentos({pIntentos}) Tiempo({pTiempo}) Poderes({pPoder})\n" +
                  $"Variación: {factorCambio} | Nuevo Skill: {skillActual} | Estado: {estadoActual}");

        // Limpieza para el siguiente nivel
        intentosNivel = 1;
        ReiniciarMetricas();
    }
}
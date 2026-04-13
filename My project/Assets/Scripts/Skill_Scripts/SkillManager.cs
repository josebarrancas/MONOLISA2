using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("Algoritmo Skill (Estado Actual)")]
    public float skillActual = 50f; // Inicia en un valor neutro
    public string estadoActual = "Based";

    [Header("Métricas del Nivel (Debug)")]
    public int intentosNivel = 1;
    public float tiempoInicio;
    public int poderesUtilizados;

    // Diccionario para saber qué poderes le gustan más al jugador
    public Dictionary<string, int> historialPoderes = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se borra al cambiar de nivel
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
    }

    public void RegistraUsoPoder(string nombrePoder)
    {
        poderesUtilizados++;

        if (historialPoderes.ContainsKey(nombrePoder))
        {
            historialPoderes[nombrePoder]++;
        }
        else
        {
            historialPoderes.Add(nombrePoder, 1);
        }
        Debug.Log($"Poder registrado: {nombrePoder}. Usos totales: {historialPoderes[nombrePoder]}");
        Debug.Log($"Muertes acumuladas: {intentosNivel}. ");
    }

    public void CalcularResultados()
    {
        float tiempoFinal = Time.time - tiempoInicio;

        // 1. PUNTOS POR INTENTOS (Dificultad percibida)
        int pIntentos = 0;
        if (intentosNivel == 1) pIntentos = 10;
        else if (intentosNivel <= 3) pIntentos = 5;
        else if (intentosNivel <= 5) pIntentos = 0;
        else if (intentosNivel <= 8) pIntentos = -5;
        else pIntentos = -10;

        // 2. PUNTOS POR TIEMPO (Fluidez)
        int pTiempo = 0;
        if (tiempoFinal < 60f) pTiempo = 5;
        else if (tiempoFinal <= 120f) pTiempo = 0;
        else pTiempo = -5;

        // 3. PUNTOS POR PODERES (Dominio de mecánicas)
        // Nota: Menos poderes usados = Jugador más habilidoso (rústico)
        int pPoder = 0;
        if (poderesUtilizados == 0) pPoder = 10;
        else if (poderesUtilizados <= 2) pPoder = 0;
        else pPoder = -5;

        // 4. FÓRMULA DE PUNTUACIÓN Y SKILL
        // Calculamos cuánto sumó o restó este nivel específicamente
        float puntuacionNivel = pIntentos + pTiempo + pPoder;

        // Ponderación: 70% historia previa, 30% desempeño actual
        skillActual = (skillActual * 0.7f) + (puntuacionNivel * 0.3f);

        // 5. DETERMINAR EL ESTADO (Rangos corregidos)
        if (skillActual < 35) estadoActual = "Issue";
        else if (skillActual > 65) estadoActual = "Solution";
        else estadoActual = "Based";

        Debug.Log($"<color=cyan>--- RESULTADOS SKILL ---</color>");
        Debug.Log($"Puntos: Intentos({pIntentos}) Tiempo({pTiempo}) Poderes({pPoder})");
        Debug.Log($"Puntuacion final del nivel: {puntuacionNivel}");
        Debug.Log($"Numero de intentos: ({intentosNivel}) Tiempo({tiempoFinal}) Poderes({pPoder})");
        Debug.Log($"Skill Actualizada: {skillActual} | Siguiente Fase: {estadoActual}");

        // Limpiar datos para el siguiente nivel
        intentosNivel = 1;
        ReiniciarMetricas();
    }
}
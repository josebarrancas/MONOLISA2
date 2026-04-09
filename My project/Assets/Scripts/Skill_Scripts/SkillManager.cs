using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour

{
    public static SkillManager Instance;
    [Header("Algoritmo Skill (solo para debug)")]
    public float skillActual = 50f;
    public string estadoActual = "Based";

    [Header("Metricas del nivel actal (solo debug)")]
    public int intentosNivel = 1;
    public float tiempoInicio;
    public int poderesUtilizados;

    public Dictionary<string, int> historialPoderes = new Dictionary<string, int>();

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ReiniciarMetricas();
        }
        else { 
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
        else {
            historialPoderes.Add(nombrePoder,1);
        }
        Debug.Log($"Poder registrado: {nombrePoder}. Usos totales: {historialPoderes[nombrePoder]}");
    }





    public void CalcularResultados()
    { 
        float tiempoFinal = Time.time - tiempoInicio;

        // Puntos por intento
        int pIntentos = 0;
        if(intentosNivel == 1) pIntentos = 10;
        else if (intentosNivel <= 3) pIntentos = 5;
        else if (intentosNivel <= 5) pIntentos = 0;
        else if (intentosNivel <= 8) pIntentos = -5;
        else pIntentos = -10;

        // Puntos por intento
        int pTiempo = 0;
        if (tiempoFinal < 60f) pTiempo = 5;
        else if (tiempoFinal <= 120f) pTiempo = 0;
        else pTiempo = -5;

        // Puntos para poderes
        int pPoder = 0;
        if (poderesUtilizados == 0) pPoder = 10;
        else if (poderesUtilizados <= 2) pPoder = 0;
        else pPoder = -5;

        // Formula para calcular la puntuacion
        float puntuacionNivel = pIntentos + pTiempo + pPoder;
        skillActual = (skillActual * 0.7f) + (puntuacionNivel * 0.3f);

        if(skillActual < 35) estadoActual = "Issue";
        else if (skillActual > 65) estadoActual = "Solution";
        else estadoActual = "Based";

        Debug.Log($"<color=cyan>Skill Actualizada: {skillActual} | Estado: {estadoActual}</color>");

        // Preparar para el siguiente nivel
        intentosNivel = 1;
        ReiniciarMetricas();
    }


}

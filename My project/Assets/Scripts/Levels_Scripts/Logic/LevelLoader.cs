using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    public string proximaEscenaCargar;

    [Header("Progreso de la Partida")]
    public int nivelGlobal = 1; // Empezamos en 0(El Tutorial)

    [Header("Bolsas de Niveles (Fases)")]
    public List<LevelData> nivelesFase1;
    public List<LevelData> nivelesFase2;
    public List<LevelData> nivelesFase3;
    public List<LevelData> nivelesFase4;

    [Header("Memoria de la Partida Actual")]
    public List<LevelData> nivelesDeEstaPartida = new List<LevelData>();
    public int nivelActualIndice = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void CargarSiguienteNivel(string dificultad)
    {
        // Si no hay niveles o ya completamos la ronda de 3
        if (nivelesDeEstaPartida == null || nivelActualIndice >= nivelesDeEstaPartida.Count)
        {
            GenerarNuevaRonda();
        }

        if (nivelesDeEstaPartida.Count > 0)
        {
            string nombreBase = nivelesDeEstaPartida[nivelActualIndice].nombreEscena;
            string dificultadMin = dificultad.ToLower();

            string nombreLimpio = nombreBase;
            if (nombreLimpio.Contains("based")) nombreLimpio = nombreLimpio.Replace("based", dificultadMin);
            else if (nombreLimpio.Contains("Based")) nombreLimpio = nombreLimpio.Replace("Based", dificultadMin);

            proximaEscenaCargar = nombreLimpio;

            if (CondicionesManager.Instance != null && SkillManager.Instance != null)
            {
                CondicionesManager.Instance.EvaluarCondicionesParaNivel(proximaEscenaCargar, SkillManager.Instance.skillActual);
            }

            
            nivelActualIndice++;

            SceneManager.LoadScene("Pantalla_Seleccion");
        }
    }

    private void GenerarNuevaRonda()
    {
        List<LevelData> pool = new List<LevelData>();
        int cantidad = 3;

        if (nivelGlobal >= 8)
        {
            pool = nivelesFase4; cantidad = 1;
        }
        else if (nivelGlobal >= 6)
        {
            pool = nivelesFase3; cantidad = 2;
        }
        else if (nivelGlobal >= 3) // <--- Si el nivelGlobal es 3, ya es el cuarto nivel real
        {
            pool = nivelesFase2; cantidad = 3;
        }
        else
        {
            pool = nivelesFase1; cantidad = 3;
        }

        Debug.Log($"<color=yellow>GENERADOR:</color> Sorteando para Nivel {nivelGlobal}. Fase detectada por umbral.");

        List<string> tags = new List<string>() { "General" };
        if (Logic_Manager.instance != null) tags = Logic_Manager.instance.ObtenerMayoriaEtiquetas();

        nivelesDeEstaPartida = mejorRondaNiveles(tags, cantidad, pool);
        nivelActualIndice = 0;
    }

    private List<LevelData> mejorRondaNiveles(List<string> etiquetas, int cantidad, List<LevelData> pool)
    {
        int aTomar = Mathf.Min(cantidad, pool.Count);
        List<List<LevelData>> simulaciones = new List<List<LevelData>>();
        List<float> puntajes = new List<float>();

        for (int i = 0; i < 3; i++)
        {
            List<LevelData> ronda = pool.OrderBy(x => Random.value).Take(aTomar).ToList();
            simulaciones.Add(ronda);

            float score = 0;
            foreach (var nivel in ronda)
            {
                int match = 0;
                foreach (string tag in nivel.etiquetasNivel)
                {
                    if (LogicaPoderes.Relaciones.ContainsKey(tag))
                        if (etiquetas.Any(e => LogicaPoderes.Relaciones[tag].Contains(e))) match++;
                }
                score += (nivel.etiquetasNivel.Length > 0) ? (float)match / nivel.etiquetasNivel.Length : 0;
            }
            puntajes.Add(score / aTomar);
        }
        return simulaciones[puntajes.IndexOf(puntajes.Max())];
    }
}
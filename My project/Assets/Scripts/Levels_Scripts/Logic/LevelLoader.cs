using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    // Aquí guardaremos los LevelData que ganaron el sorteo
    private List<LevelData> nivelesDeEstaPartida;
    public int nivelActualIndice = 0;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    // Esta función la llamas DESPUÉS de que SorteoNiveles termine
    public void EstablecerRondaGanadora(List<LevelData> rondaElegida)
    {
        nivelesDeEstaPartida = rondaElegida;
        nivelActualIndice = 0;
    }

    public void CargarSiguienteNivel(string dificultad)
    {
        // Si acabamos de pasar el nivel X, sumamos para ir al nivel X + 1
        nivelActualIndice++;

        if (nivelActualIndice < nivelesDeEstaPartida.Count)
        {
            // Obtenemos el nombre base de la escena desde el ScriptableObject
            string nombreBaseEscena = nivelesDeEstaPartida[nivelActualIndice].nombreEscena;

            // Construimos el nombre inteligente: "NivelAgua_Issue"
            string escenaFinal = nombreBaseEscena + "_" + dificultad;

            Debug.Log($"Cargando Nivel {nivelActualIndice + 1}: {escenaFinal}");
            UnityEngine.SceneManagement.SceneManager.LoadScene(escenaFinal);
        }
        else
        {
            Debug.Log("¡Felicidades Jorge! Has completado la fase.");
            // Aquí podrías cargar una escena de "Resultados Finales"
        }
    }
}
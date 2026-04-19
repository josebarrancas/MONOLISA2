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
        if (nivelActualIndice < nivelesDeEstaPartida.Count)
        {
            // 1. Obtenemos el nombre completo que tiene el ScriptableObject
            // Ejemplo: "Lvl_1_Based"
            string nombreEscenaBased = nivelesDeEstaPartida[nivelActualIndice].nombreEscena;

            // 2. El TRUCO: Reemplazamos "Based" por "Issue" o "Solution"
            // Si nombreEscenaBased es "Lvl_1_Based", resultado será "Lvl_1_Issue"
            string escenaFinal = nombreEscenaBased.Replace("Based", dificultad);

            Debug.Log($"Nivel Base: {nombreEscenaBased} -> Cargando Variante: {escenaFinal}");

            // 3. Aumentamos el índice para la próxima y cargamos
            nivelActualIndice++;
            UnityEngine.SceneManagement.SceneManager.LoadScene(escenaFinal);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    // Inicializamos la lista de una vez para que nunca sea null, solo vacía
    private List<LevelData> nivelesDeEstaPartida = new List<LevelData>();
    public int nivelActualIndice = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("<color=green>LevelLoader:</color> Instancia principal creada y protegida.");
        }
        else
        {
            Debug.Log("<color=yellow>LevelLoader:</color> Se detectó un duplicado en la escena, destruyéndolo...");
            Destroy(gameObject);
        }
    }

    public void EstablecerRondaGanadora(List<LevelData> rondaElegida)
    {
        if (rondaElegida == null)
        {
            Debug.LogError("<color=red>LevelLoader:</color> ¡ERROR! Intentaron entregar una lista nula desde el Sorteo.");
            return;
        }

        nivelesDeEstaPartida = rondaElegida;
        nivelActualIndice = 0;
        Debug.Log($"<color=cyan>LevelLoader:</color> Datos recibidos con éxito. Total de niveles cargados: {nivelesDeEstaPartida.Count}");
    }

    public void CargarSiguienteNivel(string dificultad)
    {
        // Verificamos si la lista tiene algo
        if (nivelesDeEstaPartida.Count == 0)
        {
            Debug.LogError("<color=red>LevelLoader:</color> No se puede cargar nivel. La lista 'nivelesDeEstaPartida' está vacía. ¿Se llamó a EstablecerRondaGanadora desde el Menú?");
            return;
        }

        if (nivelActualIndice < nivelesDeEstaPartida.Count)
        {
            string nombreEscenaBased = nivelesDeEstaPartida[nivelActualIndice].nombreEscena;
            string escenaFinal = nombreEscenaBased.Replace("Based", dificultad);

            Debug.Log($"<color=white>Cargando:</color> {escenaFinal} (Índice: {nivelActualIndice})");

            nivelActualIndice++;
            UnityEngine.SceneManagement.SceneManager.LoadScene(escenaFinal);
        }
        else
        {
            Debug.Log("Partida finalizada. No hay más niveles en la lista.");
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    public string proximaEscenaCargar;
    [Header("Memoria de la Partida")]
    public List<PoderData> poderesDeLaPartida = new List<PoderData>();

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
        // 1. Blindaje contra listas vacías
        if (nivelesDeEstaPartida == null || nivelesDeEstaPartida.Count == 0)
        {
            Debug.LogError("Error Crítico: El LevelLoader no tiene niveles registrados.");
            return;
        }

        // 2. ¿Aún hay niveles en la lista por jugar?
        if (nivelActualIndice < nivelesDeEstaPartida.Count)
        {
            // Extraemos el nombre base del nivel
            string nombreBase = nivelesDeEstaPartida[nivelActualIndice].nombreEscena;

            // Guardamos el siguiente destino en la memoria
            proximaEscenaCargar = nombreBase.Replace("Based", dificultad);

            // Aumentamos el contador para la próxima puerta
            nivelActualIndice++;

            // Hacemos la parada técnica en la pantalla de Re-roll
            UnityEngine.SceneManagement.SceneManager.LoadScene("Pantalla_Seleccion");
        }
        else
        {
            // 3. ¡EL JUGADOR SOBREVIVIÓ A TODOS LOS NIVELES!
            Debug.Log("<color=yellow>¡PARTIDA COMPLETADA!</color> Regresando al Hub...");

            // Limpiamos la memoria para que la próxima partida empiece fresca
            nivelActualIndice = 0;
            nivelesDeEstaPartida.Clear();
            poderesDeLaPartida.Clear();

            // --- CAMBIE ESTE TEXTO POR EL NOMBRE EXACTO DE SU ESCENA DEL HUB ---
            UnityEngine.SceneManagement.SceneManager.LoadScene("Hub_Principal");
        }
    }
}
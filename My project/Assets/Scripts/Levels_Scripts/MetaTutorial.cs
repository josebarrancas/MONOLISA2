using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaTutorial : MonoBehaviour
{
    [Header("Referencias de Scripts")]
    public Logic_Manager logicManager;
    public SorteoNiveles sorteador;

    [Header("Configuración de Partida")]
    public int nivelesPorPartida = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos que sea el jugador
        if (other.CompareTag("Player"))
        {
            // --- NUEVA CERRADURA DE COLECCIONABLES ---
            // Le preguntamos al script de monedas si el contador ya llegó al máximo
            if (MonedaColeccionable.TodasLasMonedasRecogidas())
            {
                EjecutarConfiguracionDePartida();
            }
            else
            {
                Debug.LogWarning("<color=orange>BLOQUEO TUTORIAL:</color> Aún no has recogido el dato (moneda) de esta zona.");
            }
        }
    }

    void EjecutarConfiguracionDePartida()
    {
        List<string> top3 = logicManager.ObtenerMayoriaEtiquetas();

        if (top3 != null && top3.Count > 0)
        {
            List<LevelData> rondaGanadora = sorteador.mejorRondaNiveles(top3, nivelesPorPartida);

            if (rondaGanadora != null && rondaGanadora.Count > 0)
            {

                if (LevelLoader.Instance != null)
                {
                    LevelLoader.Instance.EstablecerRondaGanadora(rondaGanadora);



                    string escenaInicial = rondaGanadora[0].nombreEscena;

                    Debug.Log($"<color=cyan>MetaTutorial:</color> Enviando {rondaGanadora.Count} niveles al LevelLoader.");

                    // 1. Le "anotamos" al LevelLoader cuál es el nivel que sigue
                    LevelLoader.Instance.proximaEscenaCargar = escenaInicial;

                    // 2. Mandamos al jugador a la sala de espera (Pantalla de Selección)
                    SceneManager.LoadScene("Pantalla_Seleccion");
                }
                else
                {
                    Debug.LogError("Error: No se encontró el LevelLoader en la escena del menú.");
                }
            }
            else
            {
                Debug.LogError("El sorteador no devolvió niveles.");
            }
        }
        else
        {
            Debug.LogError("No se pudieron obtener etiquetas del mazo.");
        }
    }
}
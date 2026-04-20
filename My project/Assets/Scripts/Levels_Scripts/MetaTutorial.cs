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
        if (other.CompareTag("Player"))
        {
            EjecutarConfiguracionDePartida();
        }
    }

    void EjecutarConfiguracionDePartida()
    {
        // PASO 1: Obtener etiquetas
        List<string> top3 = logicManager.ObtenerMayoriaEtiquetas();

        if (top3 != null && top3.Count > 0)
        {
            // PASO 2: Sortear la mejor ronda
            List<LevelData> rondaGanadora = sorteador.mejorRondaNiveles(top3, nivelesPorPartida);

            if (rondaGanadora != null && rondaGanadora.Count > 0)
            {
                 
                // En lugar de GameData, se lo entregamos al LevelLoader que es el que manda
                if (LevelLoader.Instance != null)
                {
                    LevelLoader.Instance.EstablecerRondaGanadora(rondaGanadora);

                    // PASO 4: Cargar el primer nivel usando el nombre del ScriptableObject
                    // Usamos "Based" por defecto para el primer nivel
                    string escenaInicial = rondaGanadora[0].nombreEscena;

                    Debug.Log($"<color=cyan>MetaTutorial:</color> Enviando {rondaGanadora.Count} niveles al LevelLoader.");
                    SceneManager.LoadScene(escenaInicial);
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
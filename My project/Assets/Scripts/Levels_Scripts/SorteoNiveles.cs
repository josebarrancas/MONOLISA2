using UnityEngine;

/// <summary>
/// Script para las puertas del Hub. 
/// Solo le da el "empujón" inicial al LevelLoader para que arranque el flujo.
/// </summary>
public class SorteoNiveles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que sea el jugador quien toca la puerta
        if (collision.CompareTag("Player"))
        {
            if (LevelLoader.Instance != null)
            {
                Debug.Log("<color=cyan>SorteoNiveles:</color> Puerta detectada. Iniciando motor automático...");

                // No necesitamos sortear nada aquí. 
                // El LevelLoader detectará que su lista está vacía y ejecutará 
                // su propia lógica matemática de fases y etiquetas.
                LevelLoader.Instance.CargarSiguienteNivel("Based");
            }
            else
            {
                Debug.LogError("Error Crítico: No se encontró el LevelLoader en la escena.");
            }
        }
    }
}
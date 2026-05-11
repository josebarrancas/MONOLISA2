using UnityEngine;

/// <summary>
/// Encargado de detectar el fin del tutorial y disparar el inicio 
/// de la primera ronda real (Fase 1).
/// </summary>
public class MetaTutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Solo avanzamos si el jugador recogió la moneda/dato del tutorial
            if (MonedaColeccionable.TodasLasMonedasRecogidas())
            {
                EjecutarConfiguracionDePartida();
            }
            else
            {
                Debug.LogWarning("<color=orange>BLOQUEO TUTORIAL:</color> Aún no has recogido el dato de esta zona.");
            }
        }
    }

    void EjecutarConfiguracionDePartida()
    {
        if (LevelLoader.Instance != null)
        {
            Debug.Log("<color=cyan>MetaTutorial:</color> Tutorial completado.");

            LevelLoader.Instance.nivelGlobal = 0;
            LevelLoader.Instance.nivelActualIndice = 0;

            // Mandamos a cargar la Pantalla de Selección para el primer nivel real
            LevelLoader.Instance.CargarSiguienteNivel("Based");
        }
        else
        {
            Debug.LogError("Error: No se encontró el LevelLoader. ¿Empezó desde el Hub?");
        }
    }
}
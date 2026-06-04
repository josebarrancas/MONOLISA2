using UnityEngine;

/// <summary>
/// Encargado de detectar el fin del tutorial y disparar el inicio 
/// de la primera ronda real (Fase 1) integrando el sistema de guardado en TXT.
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

            // --- NUEVO SISTEMA TXT ---
            // 1. Cargamos los datos actuales para no borrar el récord histórico
            var datos = ManejadorGuardadoTexto.CargarDatos();

            // 2. Limpiamos el tiempo de la partida actual a cero para el speedrun limpio,
            // pero mantenemos sus mejores récords de tiempo y puntaje previos intactos.
            ManejadorGuardadoTexto.GuardarDatos(
                0f,
                datos.recordMejorTiempoNormal,
                0,
                datos.recordMejorPuntajeNormal
            );

            // 3. Encendemos el motor del reloj global en el script de pausa
            ControlarPausaMenu.CronometroActivo = true;

            Debug.Log("[QA TIEMPO] ¡Meta Cruzada! Tiempo reseteado a 0 en TXT y cronómetro encendido globalmente.");

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
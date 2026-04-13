using UnityEngine;

/// <summary>
/// Script encargado de detectar el fin del nivel y coordinar
/// la carga de la siguiente escena inteligente.
/// </summary>
public class MetaNivel : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Indica si este es el nivel 1, 2 o 3 dentro de la fase actual.")]
    public int numeroNivelActual;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que sea el jugador quien toca la meta
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡Meta alcanzada! Procesando transición inteligente...");

            // 1. Validamos que los Managers existan en la escena
            if (SkillManager.Instance != null && LevelLoader.Instance != null)
            {
                // 2. Ejecutamos el cálculo de la habilidad (Skill) basada en:
                // Intentos, Tiempo y Uso de Poderes.
                SkillManager.Instance.CalcularResultados();

                // 3. Obtenemos el estado resultante ("Issue", "Based" o "Solution")
                string dificultadDetectada = SkillManager.Instance.estadoActual;

                Debug.Log($"Dificultad calculada para el siguiente reto: {dificultadDetectada}");

                // 4. Le pedimos al LevelLoader que busque la siguiente escena 
                // del set sorteado y le aplique la dificultad correspondiente.
                LevelLoader.Instance.CargarSiguienteNivel(dificultadDetectada);
            }
            else
            {
                // Error de seguridad por si olvidaste poner los Managers en el menú de inicio
                if (SkillManager.Instance == null)
                    Debug.LogError("Falta el SkillManager en la escena.");

                if (LevelLoader.Instance == null)
                    Debug.LogError("Falta el LevelLoader en la escena.");
            }
        }
    }
}
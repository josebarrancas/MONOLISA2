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
        if (collision.CompareTag("Player"))
        {
            if (!MonedaColeccionable.TodasLasMonedasRecogidas())
            {
                Debug.LogWarning("BLOQUEADO: No has recolectado todos los archivos de datos (monedas).");
                return;
            }

            Debug.Log("¡Meta alcanzada y requisitos cumplidos!");

            if (SkillManager.Instance != null && LevelLoader.Instance != null)
            {
                SkillManager.Instance.CalcularResultados();
                string dificultadDetectada = SkillManager.Instance.estadoActual;
                LevelLoader.Instance.CargarSiguienteNivel(dificultadDetectada);
            }
        }
    }
}
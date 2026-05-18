using UnityEngine;

public class MetaNivel : MonoBehaviour
{
    private bool yaSeActivo = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaSeActivo)
        {
            if (!MonedaColeccionable.TodasLasMonedasRecogidas()) return;

            yaSeActivo = true;

            // --- PASO 1: AVISAR AL SKILL MANAGER ---
            // Esto es lo que le faltaba. Aquí calculamos los puntos antes de cambiar.
            if (SkillManager.Instance != null)
            {
                SkillManager.Instance.CalcularResultados();
            }
            else
            {
                Debug.LogWarning("MetaNivel: No se encontró el SkillManager para guardar resultados.");
            }

            // --- PASO 2: AVANZAR DE NIVEL ---
            if (LevelLoader.Instance != null)
            {
                LevelLoader.Instance.nivelGlobal++;

                // ELIMINADO: LevelLoader.Instance.nivelActualIndice++; 
                // (El LevelLoader ya hace esto internamente, no lo sume doble).

                // Leemos la dificultad que el SkillManager acaba de calcular en el Paso 1
                string dif = SkillManager.Instance != null ? SkillManager.Instance.estadoActual : "Based";

                LevelLoader.Instance.CargarSiguienteNivel(dif);
            }
        }
    }
}
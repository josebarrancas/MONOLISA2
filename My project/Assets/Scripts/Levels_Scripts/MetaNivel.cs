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
                // Avanzamos el contador primero para saber qué nivel se acaba de completar
                LevelLoader.Instance.nivelGlobal++;

                // CORTE ENGRANADO: Si el nivel global llegó a 9, significa que la campaña se acabó
                if (LevelLoader.Instance.nivelGlobal >= 9)
                {
                    Debug.Log("🏁 ¡Nivel 9 alcanzado y completado! Asegurando datos y cargando Pantalla Final...");

                    if (SkillManager.Instance != null)
                    {
                        // Forzamos el guardado en el disco local antes del cambio de escena
                        PlayerPrefs.SetFloat("Partida_TiempoActual", SkillManager.Instance.tiempoNivel);
                        PlayerPrefs.SetInt("Partida_PuntajeActual", (int)SkillManager.Instance.skillActual);
                        PlayerPrefs.Save();
                    }

                    UnityEngine.SceneManagement.SceneManager.LoadScene("Escena_PantallaFinal");
                }
                else
                {
                    // FLUJO ORDINARIO (Niveles 1 al 8)
                    string dif = SkillManager.Instance != null ? SkillManager.Instance.estadoActual : "Based";
                    LevelLoader.Instance.CargarSiguienteNivel(dif);
                }
            }
        }
    }
}
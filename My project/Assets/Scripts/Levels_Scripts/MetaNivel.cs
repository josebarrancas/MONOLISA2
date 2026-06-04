using UnityEngine;
using UnityEngine.SceneManagement;

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
                    Debug.Log("🏁 ¡Nivel 9 alcanzado y completado! Procesando récords en TXT y cargando Pantalla Final...");

                    // 1. Apagamos el motor del cronómetro de inmediato para congelar el tiempo final
                    ControlarPausaMenu.CronometroActivo = false;

                    // 2. Leemos los datos que se han estado acumulando en el archivo de texto
                    var datos = ManejadorGuardadoTexto.CargarDatos();

                    float tiempoFinalCampana = datos.partidaTiempoActual;
                    float mejorTiempoHistorico = datos.recordMejorTiempoNormal;

                    // Sacamos el puntaje del SkillManager si existe
                    int puntajeActual = 0;
                    if (SkillManager.Instance != null)
                    {
                        puntajeActual = (int)SkillManager.Instance.skillActual;
                    }

                    int mejorPuntajeHistorico = datos.recordMejorPuntajeNormal;

                    // 3. VALIDACIÓN DEL RÉCORD DE TIEMPO (Menor tiempo = Mejor Récord)
                    if (tiempoFinalCampana < mejorTiempoHistorico && tiempoFinalCampana > 0.1f)
                    {
                        Debug.Log("<color=green>¡NUEVO RÉCORD DE SPEEDRUN DETECTADO EN EL TXT!</color>");
                        datos.recordMejorTiempoNormal = tiempoFinalCampana;
                    }

                    // 4. VALIDACIÓN DEL RÉCORD DE PUNTAJE (Mayor puntaje = Mejor Récord)
                    if (puntajeActual > mejorPuntajeHistorico)
                    {
                        Debug.Log("<color=cyan>¡NUEVO RÉCORD DE PUNTAJE DETECTADO EN EL TXT!</color>");
                        datos.recordMejorPuntajeNormal = puntajeActual;
                    }

                    // 5. Forzamos el guardado definitivo en el save_data.txt
                    ManejadorGuardadoTexto.GuardarDatos(
                        tiempoFinalCampana,
                        datos.recordMejorTiempoNormal,
                        puntajeActual,
                        datos.recordMejorPuntajeNormal
                    );

                    // 6. Saltamos de escena seguros de que los datos ya están en el Bloc de Notas
                    SceneManager.LoadScene("Escena_PantallaFinal");
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
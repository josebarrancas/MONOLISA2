using UnityEngine;

public class BarraSeguimiento : MonoBehaviour
{
    public Animator animatorBarra;
    public GameObject contenidoVisual; // El objeto que tiene la imagen

    void Awake()
    {
        // Empezamos ocultos
        if (contenidoVisual != null) contenidoVisual.SetActive(false);
    }

    public void ActualizarEstado(float actual, float max, bool mostrar)
    {
        // MENSAJE DE CONTROL 1: ¿Al menos está entrando aquí?
        Debug.Log($"[BARRA] Recibido - Mostrar: {mostrar} | Actual: {actual} | Max: {max}");

        if (contenidoVisual == null)
        {
            Debug.LogError("[BARRA] ¡Falta arrastrar el 'Contenido Visual' en el Inspector!");
            return;
        }

        contenidoVisual.SetActive(mostrar);

        if (mostrar && max > 0)
        {
            float porcentaje = 1f - (actual / max);
            if (animatorBarra != null)
            {
                animatorBarra.SetFloat("Progreso", Mathf.Clamp01(porcentaje));
            }
            else
            {
                Debug.LogError("[BARRA] ¡No hay un Animator asignado en 'animatorBarra'!");
            }
        }
    }
}
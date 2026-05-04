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
        // Solo activamos si el poder lo requiere
        contenidoVisual.SetActive(mostrar);

        if (mostrar && max > 0)
        {
            // Calculamos el frame (0.0 lleno - 1.0 vacío)
            float porcentaje = 1f - (actual / max);
            animatorBarra.SetFloat("Progreso", Mathf.Clamp01(porcentaje));
            Debug.Log($"Energía: {actual}/{max} - Porcentaje enviado: {porcentaje}");
        }
    }
}
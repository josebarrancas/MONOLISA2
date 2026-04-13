using UnityEngine;

public class GelAdherentePower : MonoBehaviour
{
    [Header("Configuración de la Pistola")]
    public GameObject prefabGel;
    public int cargasRestantes = 4;

    [Tooltip("Qué tan lejos llega el disparo de pintura.")]
    public float distanciaAlcance = 6f;

    [Tooltip("Capa que el láser reconocerá como 'Pared' para poder pintarla.")]
    public LayerMask capaPared;

    public void EjecutarDisparo()
    {
        if (cargasRestantes > 0)
        {
            // Detectamos hacia dónde mira el jugador
            float direccionX = transform.localScale.x > 0 ? 1f : -1f;
            Vector2 direccionDisparo = new Vector2(direccionX, 0f);

            // Disparamos un rayo láser invisible para buscar la pared
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direccionDisparo, distanciaAlcance, capaPared);

            if (hit.collider != null)
            {
                // Si le dimos a una pared, instanciamos el Gel justo donde golpeó el láser
                GameObject gel = Instantiate(prefabGel, hit.point, Quaternion.identity);

                // Alineamos la pintura a la pared (opcional para que el sprite no se vea chueco)
                if (direccionX < 0) gel.transform.localScale = new Vector3(-1, 1, 1);

                cargasRestantes--;
                Debug.Log("Gel Adherente pegado a la pared. Cargas restantes: " + cargasRestantes);
            }
            else
            {
                Debug.Log("El disparo de pintura no alcanzó ninguna pared.");
            }
        }
        else
        {
            Debug.Log("¡No quedan cargas de Gel Adherente!");
        }
    }

    public void ResetearCargas()
    {
        cargasRestantes = 4;
    }
}
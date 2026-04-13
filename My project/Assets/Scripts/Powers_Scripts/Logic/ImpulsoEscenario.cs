using UnityEngine;

public class ImpulsoEscenario : MonoBehaviour
{
    [Header("Configuración de la Onda")]
    public float fuerzaEmpuje = 12f;
    public float distanciaAlcance = 3.5f; // Cuántas celdas llega la onda
    public float anchoOnda = 2.5f;      // Qué tan ancha es la "boca" del megáfono
    public LayerMask capasBloques;      // Selecciona la capa de tus bloques para no empujar el suelo

    public void EjecutarOnda()
    {
        // 1. Detectamos hacia dónde mira el jugador (Derecha 1, Izquierda -1)
        float mirarX = transform.localScale.x > 0 ? 1 : -1;
        Vector2 direccion = new Vector2(mirarX, 0);

        // 2. Calculamos el centro del área de impacto frente al jugador
        Vector2 puntoOrigen = (Vector2)transform.position + (direccion * (distanciaAlcance / 2));

        // 3. Buscamos todos los colliders en ese rectángulo invisible
        Collider2D[] objetosEnArea = Physics2D.OverlapBoxAll(puntoOrigen, new Vector2(distanciaAlcance, anchoOnda), 0, capasBloques);

        foreach (Collider2D col in objetosEnArea)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

            // 4. Si el objeto tiene físicas, le aplicamos el impulso
            if (rb != null)
            {
                rb.AddForce(direccion * fuerzaEmpuje, ForceMode2D.Impulse);
                Debug.Log("Impulsando objeto: " + col.name);
            }
        }
    }

    // Dibujamos un cuadro amarillo en la ventana Scene para ver el alcance
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float mirarX = transform.localScale.x > 0 ? 1 : -1;
        Vector2 centro = (Vector2)transform.position + (new Vector2(mirarX, 0) * (distanciaAlcance / 2));
        Gizmos.DrawWireCube(centro, new Vector2(distanciaAlcance, anchoOnda));
    }
}

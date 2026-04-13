using UnityEngine;

public class CuboRepulsorLogic : MonoBehaviour
{
    [Header("Configuración de Repulsión")]
    public float fuerzaRepulsion = 15f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Aseguramos que el cubo sea sólido y caiga por gravedad
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el jugador toca el cubo
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Calculamos la dirección desde el centro del cubo hacia el jugador
                Vector2 direccion = (collision.transform.position - transform.position).normalized;

                // Aplicamos la fuerza de repulsión al jugador
                playerRb.linearVelocity = Vector2.zero; // Frenamos velocidad previa para que el impulso sea limpio
                playerRb.AddForce(direccion * fuerzaRepulsion, ForceMode2D.Impulse);

                Debug.Log("¡Cubo Repulsor activado!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Regla del doc: Si sale del HUD (toca la zona muerta), se elimina
        if (collision.CompareTag("Dead"))
        {
            Destroy(gameObject);
            Debug.Log("Cubo Repulsor eliminado por caer al vacío.");
        }
    }
}
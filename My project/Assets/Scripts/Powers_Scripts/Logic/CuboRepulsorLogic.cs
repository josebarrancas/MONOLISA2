using UnityEngine;

public class CuboRepulsorLogic : MonoBehaviour
{
    [Header("Configuración de Repulsión")]
    public float fuerzaRepulsion = 25f;
    public float empujeExtraArriba = 1.0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el jugador toca el cubo
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Calculamos la dirección dinámica
                Vector2 direccion = (collision.transform.position - transform.position);

                // Le inyectamos altura artificial
                direccion.y += empujeExtraArriba;
                direccion = direccion.normalized;

                // Aplicamos la velocidad para el rebote perfecto
                playerRb.linearVelocity = direccion * fuerzaRepulsion;

                Debug.Log($"¡Cubo Repulsor activado! Dirección ajustada: {direccion}");
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


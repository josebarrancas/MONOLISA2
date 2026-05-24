using UnityEngine;

public class Bloque_Fragil_A_Flotante : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // <-- Variable para la imagen

    [Header("Configuracion de bloque flotante")]
    public float fuerzaImpacto = 1f;

    [Header("Visual Flotante")]
    [Tooltip("La imagen que tendrá el bloque cuando ya esté flotando fijo en el aire")]
    public Sprite imagenBloqueFlotante; // <-- Nueva ranura en el Inspector

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // <-- Asignamos el componente

        // ... Tu lógica de congelar el Rigidbody y damping que ya tenías ...
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;

        // CAMBIO AL ESTADO FLOTANTE: Restauramos la imagen a una textura firme
        if (imagenBloqueFlotante != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = imagenBloqueFlotante;
        }

        Debug.Log("-> El bloque ahora es flotante y cambió su aspecto visual.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this.enabled) return;

        Debug.Log("Hay colision con el bloque flotante");
        Vector2 dirrecionImpacto = (transform.position - collision.transform.position).normalized;
        rb.AddForce(dirrecionImpacto * fuerzaImpacto, ForceMode2D.Impulse);
    }
}
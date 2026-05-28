using UnityEngine;

public class Bloque_Fragil_A_Desenganche : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Configuración de Desenganche")]
    public float fuerzaImpacto = 1f;
    [Tooltip("Límite en el eje Y. Si el bloque cae por debajo de este valor, se destruirá.")]
    public float limiteCaidaY = -20f;

    [Header("Visual Desenganche")]
    [Tooltip("La imagen que tendrá el bloque al desprenderse")]
    public Sprite imagenBloqueDesenganche;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Etiqueta actualizada
        gameObject.tag = "desenganche";

        // RQF26: El sistema aplica gravedad a las plataformas
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f; // Gravedad restaurada para que caiga
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Conservamos la velocidad actual por si ya estaba en movimiento
        // rb.linearVelocity = Vector2.zero; (Eliminado para evitar frenos antinaturales al desengancharse)

        if (imagenBloqueDesenganche != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = imagenBloqueDesenganche;
        }

        Debug.Log("-> El bloque se desenganchó: Ahora tiene gravedad y etiqueta 'Desenganche'.");
    }

    void Update()
    {
        // RQF28: Desaparece si cae fuera de los límites
        if (transform.position.y < limiteCaidaY)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this.enabled) return;

        // RQF27: Mover mediante contacto o poderes
        Debug.Log("Colisión con bloque Desenganche");
        Vector2 direccionImpacto = (transform.position - collision.transform.position).normalized;
        rb.AddForce(direccionImpacto * fuerzaImpacto, ForceMode2D.Impulse);
    }
}
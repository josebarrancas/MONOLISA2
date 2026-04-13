using UnityEngine;
/// <summary>
/// Gestiona el comportamiento de plataformas que reaccionan a colisiones fisicas.
/// Cambia a estado dinamico al recibir un impacto y se bloquea en una nueva 
/// posicion cinematica cuando la velocidad se reduce a casi cero
/// </summary>
public class Bloque_Flotante : MonoBehaviour
{

    // Declaramos la variable para pode contorlar las fisicas del bloque
    private Rigidbody2D rb;



    // Declaramos una variable para poder controlar el umbral de velocidad minima para
    // forzar la detencion del bloque y no vuele 'infinitamente'. 
    [Header("Configuracion de bloque")]
    public float fuerzaImpacto = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.mass = 8f;
        rb.linearDamping = 0.05f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hay colision con el bloque");
        //rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 dirrecionImpacto = (transform.position - collision.transform.position).normalized;
        rb.AddForce(dirrecionImpacto * fuerzaImpacto, ForceMode2D.Impulse);

    }

    /*private void FixedUpdate()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            Debug.Log("Cambiando a 'Dynamic");
            if (rb.linearVelocity.magnitude <= umbralFreno)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                Debug.Log("Cambiando a 'Kinematic");
                rb.linearVelocity = Vector2.zero;
            }
        }
    }*/

}

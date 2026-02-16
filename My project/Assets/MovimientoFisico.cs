using UnityEngine;

public class MovimientoFisico : MonoBehaviour
{
    public float fuerzaMovimiento = 50f;
    public float velocidadMaxima = 8f;
    public float fuerzaSalto = 12f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private float entradaHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        entradaHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && EstaEnSuelo())
        {
            Saltar();
        }
    }

    void FixedUpdate()
    {
        // Aplicamos fuerza horizontal
        if (Mathf.Abs(rb.linearVelocity.x) < velocidadMaxima)
        {
            rb.AddForce(new Vector2(entradaHorizontal * fuerzaMovimiento, 0));
        }

        // Fricción artificial para frenar si no hay entrada (opcional)
        if (entradaHorizontal == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, rb.linearVelocity.y);
        }
    }

    void Saltar()
    {
        // Usamos Impulse para una reacción inmediata
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
    }

    bool EstaEnSuelo()
    {
        return Physics2D.OverlapCircle(transform.position, 0.2f, capaSuelo);
    }
}
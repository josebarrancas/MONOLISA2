using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour
{
    public float Speed;
    public float JumpForce;

    private Animator Animator;
    private Rigidbody2D rb;
    private float Horizontal;
    private bool Grounded;

    private EmbestifresaPower embestifresa;
    [HideInInspector] public bool enZonaDesplazador = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        embestifresa = GetComponent<EmbestifresaPower>();

        // --- SEGURO DE VIDA ---
        // Obligamos al mundo a tener gravedad normal (hacia abajo) cada vez que inicia el nivel.
        Physics2D.gravity = new Vector2(0, -9.81f);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        Horizontal = Input.GetAxisRaw("Horizontal");

        // Animación y escala relativa
        Animator.SetBool("running", Horizontal != 0.0f);
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-0.8f, Mathf.Abs(transform.localScale.y), 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(0.8f, Mathf.Abs(transform.localScale.y), 1.0f);

        // --- DETECCIÓN DE SUELO INTELIGENTE ---
        Debug.DrawRay(transform.position, -transform.up * 2.3f, Color.red);
        if (Physics2D.Raycast(transform.position, -transform.up, 2.3f))
        {
            Grounded = true;
        }
        else Grounded = false;

        // SALTO RELATIVO
        if (Input.GetKeyDown(KeyCode.W) && Grounded && !enZonaDesplazador)
        {
            rb.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (enZonaDesplazador) return;

        if (embestifresa == null || !embestifresa.estaEmbistiendo)
        {
            Vector2 direccionMovimiento = transform.right * Horizontal * Speed;
            Vector2 velocidadGravedad = Vector2.Dot(rb.linearVelocity, transform.up) * (Vector2)transform.up;
            rb.linearVelocity = direccionMovimiento + velocidadGravedad;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            BrujulaGravitacionalPower brujula = GetComponent<BrujulaGravitacionalPower>();
            if (brujula != null && brujula.estaUsandoBrujula) return;


            // Registramos la muerte del jugador antes de que se reinicie la escena
            if (SkillManager.Instance != null) SkillManager.Instance.intentosNivel++;
           
           
            // Si el jugador cae al vacío, enderezamos el mundo antes de recargar la escena.
            Physics2D.gravity = new Vector2(0, -9.81f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
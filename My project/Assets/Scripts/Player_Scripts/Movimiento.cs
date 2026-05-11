using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour
{
    // ... (Sus variables se mantienen igual) ...
    public float Speed;
    public float JumpForce;
    private float tiempoCoyote;
    private float margenCoyote = 0.15f;
    private Animator Animator;
    private Rigidbody2D rb;
    private float Horizontal;
    private bool Grounded;
    private EmbestifresaPower embestifresa;
    [HideInInspector] public bool enZonaDesplazador = false;
    private bool solicitarSalto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        embestifresa = GetComponent<EmbestifresaPower>();
        Physics2D.gravity = new Vector2(0, -9.81f);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        Horizontal = Input.GetAxisRaw("Horizontal");
        Animator.SetBool("running", Horizontal != 0.0f);

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-0.8f, Mathf.Abs(transform.localScale.y), 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(0.8f, Mathf.Abs(transform.localScale.y), 1.0f);

        // Detección de suelo
        Grounded = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, 2.3f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && !hit.collider.CompareTag("Player") && !hit.collider.isTrigger)
            {
                Grounded = true;
                break;
            }
        }

        if (Grounded) tiempoCoyote = margenCoyote;
        else tiempoCoyote -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z) && tiempoCoyote > 0f && !enZonaDesplazador)
        {
            solicitarSalto = true;
            tiempoCoyote = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (enZonaDesplazador) return;
        if (embestifresa == null || !embestifresa.estaEmbistiendo)
        {
            Vector2 direccionMovimiento = transform.right * Horizontal * Speed;
            Vector2 velocidadGravedad = Vector2.Dot(rb.linearVelocity, transform.up) * (Vector2)transform.up;

            if (solicitarSalto)
            {
                float velocidadCalculada = rb.mass > 0 ? (JumpForce / rb.mass) : JumpForce;
                velocidadGravedad = transform.up * velocidadCalculada;
                solicitarSalto = false;
            }
            rb.linearVelocity = direccionMovimiento + velocidadGravedad;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            string escenaActual = SceneManager.GetActiveScene().name;

            if (escenaActual == "Tutorial")
            {
                SceneManager.LoadScene(escenaActual);
            }
            else
            {
            
                if (SkillManager.Instance != null)
                    SkillManager.Instance.intentosNivel++;

                // Regresamos a la selección para re-intentar con los mismos índices
                SceneManager.LoadScene("Pantalla_Seleccion");
            }
        }
    }
}
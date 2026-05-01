using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour
{
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

    // NUEVO: La "Bandera" de comunicación entre ciclos
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

        // --- DETECCIÓN DE SUELO PROFESIONAL ---
        Debug.DrawRay(transform.position, -transform.up * 2.3f, Color.red);
        Grounded = false;

        // Usamos RaycastAll para ignorar el propio cuerpo del jugador
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, 2.3f);
        foreach (RaycastHit2D hit in hits)
        {
            // Si golpeamos algo que NO sea el jugador y NO sea un trigger (como el vacío)
            if (hit.collider != null && !hit.collider.CompareTag("Player") && !hit.collider.isTrigger)
            {
                Grounded = true;
                break; // Encontramos suelo real, dejamos de buscar
            }
        }

        if (Grounded)
        {
            tiempoCoyote = margenCoyote; // Recargamos el tiempo de perdón
        }
        else
        {
            tiempoCoyote -= Time.deltaTime; // Se acaba el tiempo al caer
        }

        // PASO 1: Escuchamos la orden en Update (Usando el Tiempo Coyote)
        if (Input.GetKeyDown(KeyCode.Z) && tiempoCoyote > 0f && !enZonaDesplazador)
        {
            solicitarSalto = true;
            tiempoCoyote = 0f; // Vaciamos el contador para evitar saltos dobles
        }
    }

    private void FixedUpdate()
    {
        if (enZonaDesplazador) return;

        if (embestifresa == null || !embestifresa.estaEmbistiendo)
        {
            Vector2 direccionMovimiento = transform.right * Horizontal * Speed;

            // Calculamos la gravedad/caída natural actual
            Vector2 velocidadGravedad = Vector2.Dot(rb.linearVelocity, transform.up) * (Vector2)transform.up;

            // PASO 2: Ejecutamos la orden en territorio físico
            if (solicitarSalto)
            {
                // Sobrescribimos puramente la velocidad vertical. 
                // Esto anula cualquier inercia de caída o choque con el bloque flotante al 100%.

                // (Dividimos entre la masa para que la altura final sea idéntica a su AddForce original)
                float velocidadCalculada = rb.mass > 0 ? (JumpForce / rb.mass) : JumpForce;

                velocidadGravedad = transform.up * velocidadCalculada;

                // Bajamos la bandera para no volar infinitamente
                solicitarSalto = false;
            }

            // Aplicamos todo junto y sin interrupciones
            rb.linearVelocity = direccionMovimiento + velocidadGravedad;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ... (Mantenga aquí su código de muerte y SceneManager igual que antes) ...
        if (collision.CompareTag("Dead"))
        {
            string escenaActual = SceneManager.GetActiveScene().name;
            if (escenaActual == "Tutorial")
            {
                SceneManager.LoadScene(escenaActual);
            }
            else
            {
                if (SkillManager.Instance != null) SkillManager.Instance.intentosNivel++;
                if (LevelLoader.Instance != null) LevelLoader.Instance.nivelActualIndice--;
                SceneManager.LoadScene("Pantalla_Seleccion");
            }
        }
    }
}
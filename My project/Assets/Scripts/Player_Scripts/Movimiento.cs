using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el movimiento básico del jugador, el salto y la activación
/// de los poderes especiales mediante la tecla X.
/// </summary>
public class Movimiento : MonoBehaviour
{
    /* Son variables publicas que permiten ajustar la velocidad y
    la fuerza del salto directamente desde el inspector de unity */
    public float Speed;
    public float JumpForce;

    private Animator Animator;
    private Rigidbody2D rb; // Es la referencia al motor de física de unity
    private float Horizontal; // Guarda la dirección del movimiento

    /* Un valor booleano que actúa como 
     interrupctor para saber si el personaje puede saltar */
    private bool Grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        // SEGURIDAD: Si el juego está pausado (Time.timeScale = 0), no procesamos entradas
        if (Time.timeScale == 0) return;

        /* Captura la entrada del teclado de forma instantánea */
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Giro del sprite según la dirección
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-0.8f, 0.8f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

        /* Herramienta visual para comprobar el suelo en la ventana de Escena */
        Debug.DrawRay(transform.position, Vector3.down * 2.3f, Color.red);

        /* Raycast para detectar si el jugador toca el suelo */
        if (Physics2D.Raycast(transform.position, Vector3.down, 2.3f))
        {
            Grounded = true;
        }
        else Grounded = false;

        // --- LÓGICA DE PODER (TECLA X) ---
        if (Input.GetKeyDown(KeyCode.X))
        {
            UsarPoderActual();
        }

        /* Verifica salto con tecla "W" y seguro de suelo */
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }
    }

    // Función encargada de ejecutar el poder seleccionado en la ruleta
    private void UsarPoderActual()
    {
        PowerSelector selector = FindObjectOfType<PowerSelector>();
        if (selector == null) return;

        string nombrePoder = selector.ObtenerNombrePoderActual();
        Debug.Log("Ruleta dice: " + nombrePoder);

        if (nombrePoder == "Embestifresa")
        {
           
            EmbestifresaPower scriptImpulso = GetComponent<EmbestifresaPower>();

            Debug.Log("¿Se encontró el componente?: " + (scriptImpulso != null));

            if (scriptImpulso != null)
            {
                Vector2 direccion = new Vector2(transform.localScale.x, 0).normalized;
                scriptImpulso.EjecutarImpulso(direccion.normalized);
                selector.RegistrarUsoDePoder();
            }
            else
            {
                Debug.LogError("¡ERROR!: El script ImpulsoPower no está pegado al Player");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Jump()
    {
        /* Aplica un impulso físico hacia arriba */
        rb.AddForce(Vector2.up * JumpForce);
    }

    private void FixedUpdate()
    {
        /* Se actualiza la velocidad del jugador en el motor de física */
        EmbestifresaPower fresa = GetComponent<EmbestifresaPower>();

        // SOLO si NO está embistiendo, aplicamos el movimiento normal
        if (fresa == null || !fresa.estaEmbistiendo)
        {
            rb.linearVelocity = new Vector2(Horizontal * Speed, rb.linearVelocity.y);
        }
    }
}

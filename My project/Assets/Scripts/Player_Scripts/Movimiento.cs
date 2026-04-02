using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el movimiento basico del jugador, los estados de las animaciones, el salto y la activación
/// de los poderes especiales mediante la tecla X.
/// </summary>
public class Movimiento : MonoBehaviour
{
    /* Son variables publicas que permiten ajustar la velocidad y
    la fuerza del salto directamente desde el inspector de unity */
    [Header ("Configuracion del Player")]
    public float Speed;
    public float JumpForce;
    public LayerMask capaSuelo;

    [Header("Configuracion de Raycast")]
    public float largoRayo = 2.5f;

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
        // SEGURIDAD: Si el juego esta pausado (Time.timeScale = 0), no procesamos entradas
        if (Time.timeScale == 0) return;

        /* Captura la entrada del teclado de forma instantanea */
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Giro del sprite segun la direccion
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-0.8f, 0.8f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);

        // Si el 'Player' se queda quieto cambia a la animacion de 'Idle'
        Animator.SetBool("running", Horizontal != 0.0f);

        

        // Declaramos un 'Vector2' para cada 'Raycast' que usaremos
        Vector2 Centro = transform.position;
        Vector2 diagonalIzquierda = new Vector2(-0.3f, -1f).normalized; // Aqui para poder hacer que los rayos sean en diagonal tenemos que colocar las coordenas
        Vector2 diagonalDerecha = new Vector2(0.3f, -1f).normalized; // de en donde terminara el 'Raycast' y tenemos que 'normalized' para que se dibuje una diagonal


        // Declaramos una variable de tipo booleano para cada 'Raycast'
        // seran verdaderas si algun 'Raycast' toca alguna plataforma con la etiqueta de tipo 'Suelo'
        bool tocarCentro = Physics2D.Raycast(Centro, Vector2.down, largoRayo, capaSuelo);
        bool tocarIzquierda = Physics2D.Raycast(Centro, diagonalIzquierda, largoRayo, capaSuelo);//Hay que realizar esta suma para que el raycast pueda salir del colliler del player
        bool tocarDerecha = Physics2D.Raycast(Centro, diagonalDerecha, largoRayo, capaSuelo);

        // Sera verdadera si algun 'Raycast' toca alguna plataforma
        Grounded = tocarCentro || tocarIzquierda || tocarDerecha;

        // Herramienta para poder visualizar los 'Raycast' en escena
        Debug.DrawRay(Centro, Vector2.down * largoRayo, Color.red);
        Debug.DrawRay(Centro, diagonalIzquierda * (largoRayo), Color.red);
        Debug.DrawRay(Centro, diagonalDerecha * (largoRayo), Color.red);


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
           
            EmbestifresaPower scriptFresa = GetComponent<EmbestifresaPower>();

            Debug.Log("¿Se encontró el componente?: " + (scriptFresa != null));

            if (scriptFresa != null)
            {
                float inputX = Input.GetAxisRaw("Horizontal");
                float inputY = Input.GetAxisRaw("Vertical");

                if (inputX == 0 && inputY == 0) { inputX = transform.localScale.x > 0 ? 1 : -1; }

                Vector2 direccionDash = new Vector2(inputX, inputY).normalized;

                Debug.Log("Direccion en Y es: " + inputY);

                scriptFresa.EjecutarImpulso(direccionDash);
                selector.RegistrarUsoDePoder();

               /* Vector2 direccion = new Vector2(transform.localScale.x, 0).normalized;
                scriptImpulso.EjecutarImpulso(direccion.normalized);
                selector.RegistrarUsoDePoder();*/
            }
            else
            {
                Debug.LogError("ERROR: Fallo al encontrar el script del poder 'EmbestiFresa'", this);
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

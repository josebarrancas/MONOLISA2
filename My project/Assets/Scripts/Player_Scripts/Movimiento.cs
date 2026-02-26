using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour
{

    /*Son variables publicas que permiten ajustar la velocidad y
    la fuerza ddel salto directamente desde el inspector de unity*/
    public float Speed;
    public float JumpForce;

    private Animator Animator;


    private Rigidbody2D rb; //Es la referencia al mortor de fisicaa de unity
    private float Horizontal;//Guarda la direccion del movimiento que va desde '-1' para izquierda, '1, para derecha y '0' si no se presiona ninguna tecla

    /*Un valor booleano que actuca como 
     interrupctor para saber si el personaje puede saltar*/
    private bool Grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        /*Captura la entrada del teclado de forma instantenea*/
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-0.8f, 0.8f, 1.0f);
        else if(Horizontal > 0.0f) transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

       /*Es una herramienta visual. Solo nosotros podemos visualizar el "rayo invisible", lo observamos de color rojo
        en la ventana de escena en unity, lo usamos para comprobar la distancia correcta*/
        Debug.DrawRay(transform.position, Vector3.down * 2.3f, Color.red);
  
         /*Lanza un "rayo invisible" desde el centro del jugadorhacia abajo. Si este rayo choca 
          con algo, "Grounded" se vuelve  verdadero y el personaje puede volver  a saltar*/
        if (Physics2D.Raycast(transform.position, Vector3.down, 2.3f))
        { 
            Grounded = true;
        }
        else Grounded = false;


        /*Verifica si se presiono la tecla "W". El "Grounded" es un seguro, solo permite llamar a la funcion de salto 
         si el "rayo" detecta suelo, evitando que el jugados salte infinitamente*/
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
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
        /*Aplica un impulso fisico hacia arriba*/
        rb.AddForce(Vector2.up * JumpForce);
    }


    /*Esta funcion de ejecuta en un intervalo de tiempo fijo*/
    private void FixedUpdate()
    {
        /*Se actualiza la velocidad del juga*/
        rb.linearVelocity =  new Vector2 (Horizontal * Speed, rb.linearVelocity.y);
    }
}

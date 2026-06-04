using UnityEngine;

public class GelAdherenteLogic : MonoBehaviour
{
    [Header("Configuración del Gel")]
    public float tiempoDeVida = 8f;
    public float velocidadEscalada = 5f;

    private Rigidbody2D playerRb;
    private bool jugadorEnGel = false;

    void Start()
    {
        // Autodestrucción a los 8 segundos
        Destroy(gameObject, tiempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerRb = col.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                jugadorEnGel = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (jugadorEnGel && playerRb != null)
        {
            // 1. DETECCIÓN DE SALTO INFALIBLE
            // Calculamos su velocidad vertical respecto a su rotación
            float velYLocal = Vector2.Dot(playerRb.linearVelocity, playerRb.transform.up);

            // Si la velocidad es mucho mayor que la escalada, significa que presionó salto.
            if (velYLocal > velocidadEscalada + 1f)
            {
                // Dejamos de actuar y permitimos que la física de su salto siga su curso normal
                return;
            }

            // 2. ESCALADA ESTABLE (Sin tocar el gravityScale)
            // Leemos el input del jugador (1, 0, o -1)
            float inputVertical = Input.GetAxisRaw("Vertical");

            // Calculamos hacia dónde es "arriba" para el jugador (soporte G-Inversor)
            Vector2 direccionArriba = playerRb.transform.up;

            // Sobrescribimos su eje Y para vencer a la gravedad con puro motor, sin apagarla
            Vector2 empujeVertical = direccionArriba * (inputVertical * velocidadEscalada);

            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, empujeVertical.y);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // Simplemente lo soltamos, la gravedad hará el resto porque nunca la apagamos
            jugadorEnGel = false;
            playerRb = null;
        }
    }

    private void OnDestroy()
    {
        jugadorEnGel = false;
        playerRb = null;
    }
}
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
        // El objeto se autodestruirá cuando pasen los 8 segundos
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
        // Usamos OnTriggerStay para forzar la gravedad a 0 constantemente.
        // Así evitamos que dos manchas juntas se peleen por el control.
        if (jugadorEnGel && playerRb != null)
        {
            playerRb.gravityScale = 0f;

            float inputVertical = Input.GetAxisRaw("Vertical");
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, inputVertical * velocidadEscalada);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SoltarJugador();
        }
    }

    private void OnDestroy()
    {
        if (jugadorEnGel)
        {
            SoltarJugador();
        }
    }

    private void SoltarJugador()
    {
        if (playerRb != null)
        {
            // 1. FRENADO: Matamos la inercia vertical para que no salga volando
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);

            // 2. GRAVEDAD SEGURA: Restauramos a 5 (o -5 si el G-Inversor está activo)
            float gravedadRestaurar = 5f;

            GInversorPower inversor = playerRb.GetComponent<GInversorPower>();
            if (inversor != null && inversor.estaInvertido)
            {
                gravedadRestaurar = -5f; // Cae hacia el techo
            }

            playerRb.gravityScale = gravedadRestaurar;
        }

        jugadorEnGel = false;
        playerRb = null;
    }
}
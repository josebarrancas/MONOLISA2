using UnityEngine;
using System.Collections;

public class EmbestifresaPower : MonoBehaviour
{
    [Header("Configuracion del Poder")]
    public float fuerzaImpulso = 30f; // Un valor entre 25 y 40 es ideal
    public float tiempoLibre = 0.2f;  // Duración del Dash

    [HideInInspector]
    public bool estaEmbistiendo = false;

    public int cargasRestantes = 5;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void EjecutarImpulso(Vector2 direccion)
    {
        // Si hay cargas y no estamos ya en un dash, arrancamos
        if (cargasRestantes > 0 && !estaEmbistiendo)
        {
            StartCoroutine(AplicarVelocidadDash(direccion));
            cargasRestantes--;
            Debug.Log("Dash ejecutado. Cargas: " + cargasRestantes);
        }
    }

    private IEnumerator AplicarVelocidadDash(Vector2 dir)
    {
        estaEmbistiendo = true;

        // 1. Guardamos tu Gravedad de 5 y la apagamos para que no "pese" el personaje
        float gravedadOriginal = rb.gravityScale;
        rb.gravityScale = 0;

        // 2. VELOCIDAD DIRECTA: Esto ignora la masa y el rozamiento del suelo.
        // Forzamos al Rigidbody a moverse a la velocidad de la fuerza elegida.
        rb.linearVelocity = dir * fuerzaImpulso;

        // 3. Esperamos el tiempo del dash (0.2s)
        yield return new WaitForSeconds(tiempoLibre);

        // 4. Frenado suave al final (opcional, puedes quitarlo si quieres que siga con inercia)
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // 5. Restauramos la gravedad de 5 y liberamos el movimiento normal
        rb.gravityScale = gravedadOriginal;
        estaEmbistiendo = false;
    }

    public void ResetearCargas() { cargasRestantes = 5; }
}

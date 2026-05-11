using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class EmbestifresaPower : MonoBehaviour
{
    [Header("Configuracion del Poder")]
    public float fuerzaImpulso = 20f; // Un valor entre 25 y 40 es ideal
    public float tiempoLibreHorizontal = 0.2f;  // Duración del Dash
    public float fuerzaGravedad = 2f; // Fuerza de la gravedad para saltos verticales
    public float tiempoLibreVertical = 0.2f; // Duracion del Dash vertical
    public bool esUnico = false;

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
        float gravedadOriginal = rb.gravityScale;
        if (dir.y == 1 || dir.y == -1)
        {

            rb.gravityScale = fuerzaGravedad; ;

        }
        else
        {
            rb.gravityScale = 0;
        }

        rb.linearVelocity = dir * fuerzaImpulso;

        yield return new WaitForSeconds(tiempoLibreHorizontal);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        rb.gravityScale = gravedadOriginal;
        estaEmbistiendo = false;
    }

    public void ResetearCargas() { cargasRestantes = 5; }
}

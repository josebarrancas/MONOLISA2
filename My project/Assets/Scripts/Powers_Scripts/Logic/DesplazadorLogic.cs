using System.Collections.Generic;
using UnityEngine;

public class DesplazadorLogic : MonoBehaviour
{
    [Header("Configuración del Área")]
    public float velocidadOchoDirecciones = 6f;
    [Tooltip("Tiempo antes de que el área desaparezca sola.")]
    public float tiempoDeVida = 6f;

    private Dictionary<Rigidbody2D, float> objetosAfectados = new Dictionary<Rigidbody2D, float>();

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 direccion = new Vector2(h, v).normalized;

        List<Rigidbody2D> aBorrar = new List<Rigidbody2D>();

        foreach (var kvp in objetosAfectados)
        {
            if (kvp.Key != null)
            {
                kvp.Key.linearVelocity = direccion * velocidadOchoDirecciones;
            }
            else
            {
                aBorrar.Add(kvp.Key);
            }
        }

        foreach (var rb in aBorrar)
        {
            objetosAfectados.Remove(rb);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("flotante") || col.CompareTag("desenganche") || col.CompareTag("enganche"))
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null && !objetosAfectados.ContainsKey(rb))
            {
                objetosAfectados.Add(rb, rb.gravityScale);
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;

                if (col.CompareTag("Player"))
                {
                    var mov = col.GetComponent<Movimiento>();
                    if (mov != null) mov.enZonaDesplazador = true;

                    var pod = col.GetComponent<Controlador_Poderes>();
                    if (pod != null) pod.bloqueadoPorDesplazador = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            RestaurarObjeto(rb);
        }
    }

    private void OnDestroy()
    {
        // SOLUCCIÓN AL CONGELAMIENTO: Copiamos las llaves a una lista independiente 
        // para poder vaciar el diccionario original sin romper el bucle foreach.
        List<Rigidbody2D> temporales = new List<Rigidbody2D>(objetosAfectados.Keys);

        foreach (var rb in temporales)
        {
            if (rb != null)
            {
                RestaurarObjeto(rb);
            }
        }

        objetosAfectados.Clear();
    }

    private void RestaurarObjeto(Rigidbody2D rb)
    {
        if (rb != null && objetosAfectados.ContainsKey(rb))
        {
            // Le devolvemos su gravedad original
            rb.gravityScale = objetosAfectados[rb];

            // Frenamos el objeto en seco para matar la inercia acumulada
            rb.linearVelocity = Vector2.zero;

            if (rb.CompareTag("Player"))
            {
                var mov = rb.GetComponent<Movimiento>();
                if (mov != null) mov.enZonaDesplazador = false;

                var pod = rb.GetComponent<Controlador_Poderes>();
                if (pod != null) pod.bloqueadoPorDesplazador = false;
            }

            // Remoción segura
            objetosAfectados.Remove(rb);
        }
    }
}
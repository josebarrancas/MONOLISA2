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
            if (kvp.Key != null) kvp.Key.linearVelocity = direccion * velocidadOchoDirecciones;
            else aBorrar.Add(kvp.Key);
        }

        foreach (var rb in aBorrar) objetosAfectados.Remove(rb);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("flotante") || col.CompareTag("desenganche"))
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null && !objetosAfectados.ContainsKey(rb))
            {
                objetosAfectados.Add(rb, rb.gravityScale);
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;

                if (col.CompareTag("Player"))
                {
                    col.GetComponent<Movimiento>().enZonaDesplazador = true;
                    col.GetComponent<Controlador_Poderes>().bloqueadoPorDesplazador = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        RestaurarObjeto(col.GetComponent<Rigidbody2D>());
    }

    private void OnDestroy()
    {
        foreach (var kvp in objetosAfectados)
        {
            if (kvp.Key != null) RestaurarObjeto(kvp.Key);
        }
    }

    private void RestaurarObjeto(Rigidbody2D rb)
    {
        if (rb != null && objetosAfectados.ContainsKey(rb))
        {
            // Le devolvemos su gravedad original a la normalidad
            rb.gravityScale = objetosAfectados[rb];

            // --- SOLUCIÓN AQUÍ: Frenamos el objeto en seco para matar la inercia ---
            rb.linearVelocity = Vector2.zero;

            objetosAfectados.Remove(rb);

            if (rb.CompareTag("Player"))
            {
                rb.GetComponent<Movimiento>().enZonaDesplazador = false;
                rb.GetComponent<Controlador_Poderes>().bloqueadoPorDesplazador = false;
            }
        }
    }
}
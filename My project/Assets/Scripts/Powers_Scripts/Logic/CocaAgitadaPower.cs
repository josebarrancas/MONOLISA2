using UnityEngine;
using System.Collections; // Necesario para las Corrutinas

public class CocaAgitadaPower : MonoBehaviour
{
    [Header("Configuración del Impulso")]
    [Tooltip("Velocidad a la que subirá el personaje (un valor bajo para que sea lento, ej: 4).")]
    public float velocidadElevacion = 4f;

    [Tooltip("Cuánto tiempo durará el efecto de elevación en segundos.")]
    public float tiempoElevacion = 1.5f;

    [Header("Cargas")]
    public int cargasRestantes = 3;

    [HideInInspector]
    public bool estaElevando = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Se llama una sola vez al presionar X
    /// </summary>
    public void EjecutarElevacion()
    {
        // Solo ejecutamos si hay cargas y no estamos ya en medio de una elevación
        if (cargasRestantes > 0 && !estaElevando)
        {
            StartCoroutine(RutinaElevacion());
            cargasRestantes--;
            Debug.Log("Coca Agitada usada. Cargas restantes: " + cargasRestantes);
        }
        else if (cargasRestantes <= 0)
        {
            Debug.Log("¡No quedan cargas de Coca Agitada!");
        }
    }

    private IEnumerator RutinaElevacion()
    {
        estaElevando = true;

        // 1. Guardamos la gravedad original y la apagamos
        float gravedadOriginal = rb.gravityScale;
        rb.gravityScale = 0f;

        // 2. Anulamos cualquier velocidad vertical previa (por si estaba cayendo rápido)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        float tiempoPasado = 0f;
        float dirGravedad = Mathf.Sign(gravedadOriginal); // Sinergia con G-Inversor

        // 3. Mientras dure el tiempo, forzamos la velocidad vertical hacia arriba
        while (tiempoPasado < tiempoElevacion)
        {
            // Mantenemos la velocidad X que tenga el jugador (para que pueda moverse a los lados)
            // Y forzamos la velocidad Y para la elevación lenta
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocidadElevacion * dirGravedad);

            tiempoPasado += Time.deltaTime;
            yield return null; // Esperamos al siguiente frame
        }

        // 4. Terminó el tiempo: restauramos la gravedad normal
        rb.gravityScale = gravedadOriginal;

        // Dejamos de forzar la velocidad para que empiece a caer naturalmente
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        estaElevando = false;
    }

    public void ResetearCargas()
    {
        cargasRestantes = 3;
        Debug.Log("Cargas de Coca Agitada reseteadas.");
    }
}
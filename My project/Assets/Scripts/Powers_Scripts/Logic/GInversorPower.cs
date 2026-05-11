using UnityEngine;

public class GInversorPower : MonoBehaviour
{
    [Header("Configuración del Medidor")]
    [Tooltip("Cantidad máxima de energía del medidor (Ej. 100).")]
    public float medidorMaximo = 100f;
    public bool esUnico = false;

    [Tooltip("Cuánta energía se gasta por cada segundo de uso (Ej. 30 agota 100 en ~3.3 segundos).")]
    public float costoPorSegundo = 30f;

    [Header("Estado Actual (Solo Lectura)")]
    public float nivelMedidor;
    public bool estaInvertido = false;

    private Rigidbody2D rb;
    private float gravedadOriginal;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravedadOriginal = rb.gravityScale; // Guarda la gravedad normal
        nivelMedidor = medidorMaximo;       // Llenamos el medidor al iniciar
    }

    /// <summary>
    /// Esta función se llama constantemente desde Movimiento.cs
    /// </summary>
    public void ProcesarInversion(bool intentandoUsar)
    {
        // 1. Si el jugador mantiene la tecla apretada Y aún tenemos medidor...
        if (intentandoUsar && nivelMedidor > 0)
        {
            // Restamos energía basándonos en el tiempo real
            nivelMedidor -= costoPorSegundo * Time.deltaTime;

            // Evitamos que baje de cero
            if (nivelMedidor < 0) nivelMedidor = 0;

            // Invertimos la gravedad si no lo estaba ya
            if (!estaInvertido)
            {
                Invertir(true);
            }

            // Si el medidor llegó a cero justo ahora, lo regresamos a la normalidad
            if (nivelMedidor == 0 && estaInvertido)
            {
                Invertir(false);
                Debug.Log("¡Se agotó el medidor del G-Inversor!");
            }
        }
        // 2. Si soltamos la tecla, o cambiamos de poder, o se nos acabó el medidor...
        else
        {
            if (estaInvertido)
            {
                Invertir(false);
            }
        }
    }

    private void Invertir(bool activar)
    {
        estaInvertido = activar;

        float escalaX = transform.localScale.x;
        float escalaYOriginal = Mathf.Abs(transform.localScale.y);

        if (activar)
        {
            rb.gravityScale = -Mathf.Abs(gravedadOriginal); // Gravedad hacia arriba
            transform.localScale = new Vector3(escalaX, -escalaYOriginal, 1f); // Sprite de cabeza
            Debug.Log("Gravedad Invertida. Energía restante: " + nivelMedidor);
        }
        else
        {
            rb.gravityScale = Mathf.Abs(gravedadOriginal); // Gravedad normal
            transform.localScale = new Vector3(escalaX, escalaYOriginal, 1f); // Sprite normal
            Debug.Log("Gravedad Normalizada.");
        }
    }

    public void ResetearCargas()
    {
        nivelMedidor = medidorMaximo;
        Debug.Log("Medidor de gravedad recargado al máximo.");
    }
}
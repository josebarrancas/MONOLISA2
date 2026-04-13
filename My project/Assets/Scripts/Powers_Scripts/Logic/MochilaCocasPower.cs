using UnityEngine;

public class MochilaCocasPower : MonoBehaviour
{
    [Header("Configuración del Medidor")]
    [Tooltip("Cantidad máxima de gas en la mochila.")]
    public float medidorMaximo = 100f;

    [Tooltip("Qué tan rápido se gastan las cocas por segundo.")]
    public float costoPorSegundo = 35f;

    [Header("Físicas de la Mochila")]
    [Tooltip("Velocidad a la que el jugador caerá lentamente (ej: 2 o 3).")]
    public float velocidadDescensoLento = 2f;

    [Header("Estado (Solo lectura)")]
    public float nivelMedidor;
    public bool estaUsando = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nivelMedidor = medidorMaximo;
    }

    /// <summary>
    /// Se ejecuta constantemente desde Movimiento.cs
    /// </summary>
    public void ProcesarVuelo(bool intentandoUsar)
    {
        if (intentandoUsar && nivelMedidor > 0)
        {
            estaUsando = true;

            // Gastamos la mochila
            nivelMedidor -= costoPorSegundo * Time.deltaTime;
            if (nivelMedidor < 0) nivelMedidor = 0;

            // Revisamos si el jugador está tocando el suelo
            float dirGravedad = Mathf.Sign(rb.gravityScale);
            bool tocandoPiso = Physics2D.Raycast(transform.position, Vector3.down * dirGravedad, 2.3f);

            // Si NO está tocando el piso, aplicamos el descenso lento
            if (!tocandoPiso)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -velocidadDescensoLento * dirGravedad);
            }
        }
        else
        {
            estaUsando = false;
        }
    }

    public void ResetearCargas()
    {
        nivelMedidor = medidorMaximo;
        Debug.Log("¡Mochila de Cocas recargada al 100%!");
    }
}
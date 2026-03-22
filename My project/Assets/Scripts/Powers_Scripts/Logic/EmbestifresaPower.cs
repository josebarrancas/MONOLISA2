using UnityEngine;


/// <summary>
/// Logica del poder impulso, la cual permitira que el jugador se pueda desplazadar 
/// de forma rapida hacia la direccion donde el jugador este mirando al momento 
/// de la activacion
/// </summary>
public class ImpulsoPower : MonoBehaviour
{
    [Header("Configuracion del poder")]
    public float fuerzaImpulso = 15f;
    private int cargasRestantes = 5;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void EjecutarImpulso(Vector2 direccionApuntado)
    {
        if (cargasRestantes > 0)
        {
            rb.linearVelocity = Vector2.zero;

            rb.AddForce(direccionApuntado * fuerzaImpulso, ForceMode2D.Impulse);

            cargasRestantes--;
            Debug.Log("Impulso usado. Cargas restantes: " + cargasRestantes);

        }
        else
        {
            Debug.Log("Sin impulsos restantes");
        }
    }

    public void ResetearCargas() { cargasRestantes = 5; }
}

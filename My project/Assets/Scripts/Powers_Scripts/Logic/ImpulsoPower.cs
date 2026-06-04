using UnityEngine;

public class ImpulsoPower : MonoBehaviour
{
    [Header("Configuración del Poder")]
    public int cargasRestantes = 5;

    [Tooltip("Fuerza base necesaria para desplazar un objeto exactamente 1 celda en su juego.")]
    public float fuerzaPorCelda = 5f;

    [Header("Calibración de la Cuadrícula")]
    public float tamanoDeCelda = 1f;

    [Tooltip("Rango 3x3 frontal: 1.5 unidades hacia adelante")]
    public float limite3x3 = 1.5f;
    [Tooltip("Rango 5x5 frontal: 2.5 unidades hacia adelante")]
    public float limite5x5 = 2.5f;

    public void EjecutarImpulso(Vector2 direccionApuntada)
    {
        if (cargasRestantes <= 0)
        {
            Debug.Log("¡No quedan cargas de Impulso!");
            return;
        }

        direccionApuntada = direccionApuntada.normalized;
        if (direccionApuntada == Vector2.zero)
        {
            direccionApuntada = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        cargasRestantes--;

        Rigidbody2D[] todosLosCuerpos = FindObjectsOfType<Rigidbody2D>();

        foreach (Rigidbody2D rbObjeto in todosLosCuerpos)
        {
            if (rbObjeto.gameObject == this.gameObject || rbObjeto.bodyType == RigidbodyType2D.Static)
                continue;

            Vector2 diferencia = rbObjeto.position - (Vector2)transform.position;

           
            if (Vector2.Dot(diferencia.normalized, direccionApuntada) < 0f)
            {
                // Lo ignoramos por completo
                continue;
            }

            // Calculamos la distancia real y en celdas
            float distanciaReal = Mathf.Max(Mathf.Abs(diferencia.x), Mathf.Abs(diferencia.y));
            float distanciaCeldas = distanciaReal / tamanoDeCelda;

            float multiplicadorCeldas = 0f;

            if (distanciaCeldas <= limite3x3)
            {
                multiplicadorCeldas = 6f; 
            }
            else if (distanciaCeldas <= limite5x5)
            {
                multiplicadorCeldas = 2f; 
            }
            else
            {
                multiplicadorCeldas = 1f; 
            }

            Debug.Log($"[Impulso Frontal] Objeto: {rbObjeto.name} | Distancia: {distanciaCeldas} celdas | Multiplicador: {multiplicadorCeldas}x");

            Vector2 fuerzaFinal = direccionApuntada * (multiplicadorCeldas * fuerzaPorCelda);
            rbObjeto.AddForce(fuerzaFinal, ForceMode2D.Impulse);
        }
    }
}
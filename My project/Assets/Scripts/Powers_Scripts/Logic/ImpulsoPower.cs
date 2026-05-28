using UnityEngine;

public class ImpulsoPower : MonoBehaviour
{
    [Header("Configuración del Poder")]
    public int cargasRestantes = 5;

    [Tooltip("Fuerza base necesaria para desplazar un objeto exactamente 1 celda en su juego.")]
    public float fuerzaPorCelda = 5f;

    [Header("Calibración de la Cuadrícula")]
    [Tooltip("¿Cuántas unidades de Unity mide 1 celda de su mapa? (Suele ser 1, pero puede variar según el tamaño de sus sprites)")]
    public float tamanoDeCelda = 1f;

    [Tooltip("Rango 3x3: Centro + 1 celda (Usamos 1.5 para dar margen de error)")]
    public float limite3x3 = 1.5f;
    [Tooltip("Rango 5x5: Centro + 2 celdas (Usamos 2.5 para dar margen de error)")]
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

            // Calculamos la distancia real en Unity
            float distanciaReal = Mathf.Max(Mathf.Abs(diferencia.x), Mathf.Abs(diferencia.y));

            // La dividimos por el tamaño de su celda para saber exactamente a cuántas "Celdas" está
            float distanciaCeldas = distanciaReal / tamanoDeCelda;

            float multiplicadorCeldas = 0f;

            if (distanciaCeldas <= limite3x3)
            {
                multiplicadorCeldas = 12f; // RQF185: Mueve 6 celdas
            }
            else if (distanciaCeldas <= limite5x5)
            {
                multiplicadorCeldas = 6f; // RQF186: Mueve 2 celdas
            }
            else
            {
                multiplicadorCeldas = 1f; // RQF187: Mueve 1 celda
            }

            // CHIVATO PARA LA CONSOLA: Le dirá exactamente qué cálculo hizo con cada objeto
            Debug.Log($"[Impulso] Objeto: {rbObjeto.name} | Distancia: {distanciaCeldas} celdas | Multiplicador: {multiplicadorCeldas}x");

            Vector2 fuerzaFinal = direccionApuntada * (multiplicadorCeldas * fuerzaPorCelda);
            rbObjeto.AddForce(fuerzaFinal, ForceMode2D.Impulse);
        }
    }
}
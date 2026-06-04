using UnityEngine;

public class CuboRepulsorPower : MonoBehaviour
{
    [Header("Configuración del Poder")]
    public GameObject prefabCubo;

    [Tooltip("Aumente este valor en el Inspector para que el cubo salga más lejos de forma horizontal.")]
    public float distanciaAparicion = 4f;

    [Tooltip("Modifique este valor para subir (positivo) o bajar (negativo) el cubo al aparecer.")]
    public float alturaAparicion = 0f;

    public int cargasRestantes = 1;
    public bool esUnico = false;

    public void EjecutarPoder()
    {
        if (cargasRestantes > 0)
        {
            // Detectamos hacia dónde mira el jugador
            float direccionX = transform.localScale.x > 0 ? 1 : -1;

            // Se suman tanto la distancia horizontal como la altura personalizada
            Vector3 posicionSpawn = transform.position + new Vector3(direccionX * distanciaAparicion, alturaAparicion, 0);

            // Creamos el cubo
            Instantiate(prefabCubo, posicionSpawn, Quaternion.identity);

            cargasRestantes--;
            Debug.Log("Cubo Repulsor colocado. Cargas restantes: " + cargasRestantes);
        }
    }

    public void ResetearCargas()
    {
        cargasRestantes = 1;
    }
}
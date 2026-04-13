using UnityEngine;

public class CuboRepulsorPower : MonoBehaviour
{
    [Header("Configuración del Poder")]
    public GameObject prefabCubo;
    public float distanciaAparicion = 2f;
    public int cargasRestantes = 1;

    public void EjecutarPoder()
    {
        if (cargasRestantes > 0)
        {
            // Detectamos hacia dónde mira el jugador
            float direccionX = transform.localScale.x > 0 ? 1 : -1;
            Vector3 posicionSpawn = transform.position + new Vector3(direccionX * distanciaAparicion, 0, 0);

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
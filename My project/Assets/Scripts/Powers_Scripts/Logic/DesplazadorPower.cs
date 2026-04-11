using UnityEngine;

public class DesplazadorPower : MonoBehaviour
{
    public GameObject prefabAreaDesplazador;
    public int cargasRestantes = 1;

    public void EjecutarPoder()
    {
        if (cargasRestantes > 0 && prefabAreaDesplazador != null)
        {
            // Instanciamos el área exactamente en el punto del jugador
            Instantiate(prefabAreaDesplazador, transform.position, Quaternion.identity);
            cargasRestantes--;
            Debug.Log("Desplazador Creado.");
        }
    }

    public void ResetearCargas() { cargasRestantes = 1; }
}
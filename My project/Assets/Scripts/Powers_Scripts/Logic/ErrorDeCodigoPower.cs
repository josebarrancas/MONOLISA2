using UnityEngine;

public class ErrorDeCodigoPower : MonoBehaviour
{
    [Header("Configuración")]
    public int cargasRestantes = 1;

    public void EjecutarIntercambio()
    {
        if (cargasRestantes > 0)
        {
            // Buscamos los prefabs por su Tag en Unity
            GameObject entrada = GameObject.FindGameObjectWithTag("entrada");
            GameObject salida = GameObject.FindGameObjectWithTag("salida");

            // Verificamos que ambos existan en la escena
            if (entrada != null && salida != null)
            {
                // Guardamos la posición temporal de la entrada
                Vector3 posicionTemporal = entrada.transform.position;

                // Intercambiamos posiciones
                entrada.transform.position = salida.transform.position;
                salida.transform.position = posicionTemporal;

                cargasRestantes--; // Gastamos el poder
                Debug.Log("Error de código ejecutado: Entrada y Salida han sido intercambiadas.");
            }
            else
            {
                Debug.LogError("FALLO: No se encontró un objeto con el Tag 'Entrada' o 'Salida' en la escena.");
            }
        }
        else
        {
            Debug.Log("¡No quedan cargas de Error de Código!");
        }
    }

    public void ResetearCargas()
    {
        cargasRestantes = 1;
    }
}
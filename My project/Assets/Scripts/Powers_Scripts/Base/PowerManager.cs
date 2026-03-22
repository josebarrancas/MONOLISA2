using UnityEngine;

public class PowerManager1 : MonoBehaviour
{
    private PowerSelector ruleta;

    void Start()
    {
        ruleta = FindFirstObjectByType<PowerSelector>();
    }

    public void UsarPoderActivo()
    {
        // 1. Le preguntamos a la ruleta cuál es el nombre del poder actual
        string nombrePoder = ruleta.ObtenerNombrePoderActual();

        // 2. Buscamos el script correspondiente y lo ejecutamos
        switch (nombrePoder)
        {
            case "Impulso":
                GetComponent<ImpulsoEscenario>().EjecutarOnda();
                break;

            case "Salto":
                // GetComponent<PoderSalto>().Ejecutar(); 
                break;

                // Aquí irás añadiendo los demás casos conforme los programes
        }

        // 3. Avisamos a la ruleta que descuente la carga
        ruleta.RegistrarUsoDePoder();
    }
}

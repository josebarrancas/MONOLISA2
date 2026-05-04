using System.Net.NetworkInformation;
using UnityEngine;

/// <summary>
/// Encargado de inicializar los iconos de los niveles en el HUD y encargadi
/// de mostrar los estados de los niveles.
/// </summary>

public class GestorProgresoHUD : MonoBehaviour
{
    [System.Serializable]
    public struct IconosNivel
    {
        public GameObject palomitaVerde;
        public GameObject palomitaGris;
        public GameObject circuloAzul;
    }

    [Header("Configuracion de Iconnos")]
    public IconosNivel[] niveles;

    void Start()
    {

        Debug.Log("Se llamo al script");
        ActualizarIndicadores();
    }

    public void ActualizarIndicadores()
    {
        if (LevelLoader.Instance == null) Debug.LogError("Error al cargar el 'LevelLoader");

        int nivelActualIndex = LevelLoader.Instance.nivelActualIndice; // Obtenemos el nivel en el que se encuentra

        // Ciclo para ir cargando los iconos en base a en que numero de nivel se encuentre
        for (int i = 0; i < niveles.Length; i++)
        {
            if (i < nivelActualIndex) // Si el jugador ya lleva por lo menos algun nivel completado entonces se mostraran los iconos de nivel completado
            {
                niveles[i].palomitaVerde.SetActive(true);
                niveles[i].palomitaGris.SetActive(false);
                niveles[i].circuloAzul.SetActive(false);
            }
            else if (i == nivelActualIndex) // esto servira para poder indicarle al jugador que se encuentra en tal numero de nivel
            {
                niveles[i].palomitaVerde.SetActive(false);
                niveles[i].palomitaGris.SetActive(true);
                niveles[i].circuloAzul.SetActive(true);
            }
            else // Y con esto controlaremos los iconoes de los niveles que aun no completa el jugador
            {
                niveles[i].palomitaVerde.SetActive(false);
                niveles[i].palomitaGris.SetActive(true);
                niveles[i].circuloAzul.SetActive(false);
            }
        }

    }
}

   

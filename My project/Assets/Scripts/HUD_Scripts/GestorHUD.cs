using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestorHUD : MonoBehaviour
{
    [Header("Configuración HUD")]
    public GameObject panelPoderes;
    public Image[] espaciosPoderes = new Image[9]; // Asegúrate de asignar los 9 en el Inspector

    void Start()
    {
        // Verificamos si estamos en el tutorial
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            panelPoderes.SetActive(false);
        }
        else
        {
            panelPoderes.SetActive(true);
            CargarIconosDelMazo();
        }
    }

    void CargarIconosDelMazo()
    {
        // Accedemos directamente a la instancia del Logic_Manager
        if (Logic_Manager.instance != null)
        {
            var mazo = Logic_Manager.instance.mazoJugador;

            for (int i = 0; i < espaciosPoderes.Length; i++)
            {
                // Si el mazo tiene un poder para este espacio
                if (i < mazo.Count && mazo[i] != null)
                {
                    // Asumimos que tu ScriptableObject 'PoderData' tiene una variable llamada 'icono' o 'sprite'
                    // Cámbialo por el nombre exacto que tengas en PoderData
                    espaciosPoderes[i].sprite = mazo[i].icono;
                    espaciosPoderes[i].enabled = true;
                }
                else
                {
                    // Si no hay poder, ocultamos el cuadro o ponemos uno por defecto
                    espaciosPoderes[i].enabled = false;
                }
            }
        }
        else
        {
            Debug.LogError("No se encontró la instancia de Logic_Manager.");
        }
    }
}
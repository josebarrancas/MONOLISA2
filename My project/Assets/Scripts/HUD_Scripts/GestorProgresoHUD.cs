using UnityEngine;

/// <summary>
/// Encargado de inicializar los iconos de los niveles en el HUD y encargado
/// de mostrar los estados de los niveles de forma acumulativa (Base 0: 0 al 8).
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

    [Header("Configuracion de Iconos")]
    [Tooltip("Para MONOLISA2, esta lista debería tener 9 elementos.")]
    public IconosNivel[] niveles;

    void Start()
    {
        ActualizarIndicadores();
    }

    public void ActualizarIndicadores()
    {
        if (LevelLoader.Instance == null)
        {
            Debug.LogError("Error: No se encontró el LevelLoader para actualizar el HUD.");
            return;
        }

        // --- ACTUALIZACIÓN BASE 0 ---
        // Ahora nivelGlobal coincide exactamente con el índice del array (0 a 8).
        int indexActualHUD = LevelLoader.Instance.nivelGlobal;

        for (int i = 0; i < niveles.Length; i++)
        {
            // 1. Niveles ya superados
            // Si el índice del icono es menor al nivel actual, es que ya se ganó.
            if (i < indexActualHUD)
            {
                niveles[i].palomitaVerde.SetActive(true);
                niveles[i].palomitaGris.SetActive(false);
                niveles[i].circuloAzul.SetActive(false);
            }
            // 2. Nivel en el que se encuentra actualmente el jugador
            else if (i == indexActualHUD)
            {
                niveles[i].palomitaVerde.SetActive(false);
                niveles[i].palomitaGris.SetActive(true);
                niveles[i].circuloAzul.SetActive(true);
            }
            // 3. Niveles que aún no ha alcanzado
            else
            {
                niveles[i].palomitaVerde.SetActive(false);
                niveles[i].palomitaGris.SetActive(true);
                niveles[i].circuloAzul.SetActive(false);
            }
        }
    }
}
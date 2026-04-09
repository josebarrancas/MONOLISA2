using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Logica para cargar los niveles, para que funcione
/// correctamente, los nombres en todos los codidos
/// de nivel deben de coincidir
/// </summary>

public class LevelLoader : MonoBehaviour
{
  
    public void CargarNivel(LevelData datos)
    {
        if (datos != null)
        {
            SceneManager.LoadScene(datos.nombreEscena);
        }
        else
        {
            Debug.LogError("Error! Se intento cargar el nivel pero el 'LevelData' esta vacio", this);
        }
    }

}

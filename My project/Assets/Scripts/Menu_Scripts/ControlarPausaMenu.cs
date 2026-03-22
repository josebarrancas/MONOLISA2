using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el menu de pausa del juego, ademas de progamar los botonoes de accion
/// de este menu de pausa
/// </summary>
public class ControlarPausaMenu : MonoBehaviour
{
    // Cambiado a lista [] para soportar múltiples objetos del menú
    public GameObject[] objectoMenuPausa;
    private bool juegoPausado = false;

    private void Start()
    {
        Time.timeScale = 1f;
        juegoPausado = false;

        // Bucle para asegurar que todos los objetos de la lista inicien apagados
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    void Update()
    {
        //Detectar teclas "Esc" para entrar en el menu de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }

        if (Input.GetKeyDown(KeyCode.R)) ReiniciarNivel();
    }

    //Con esta funcion controlaremos cuando el escenario entre en pausa
    void Pausar()
    {
        // Recorremos la lista para encender cada elemento 
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(true);
        }

        //Con esto indicaremos a los "FixedUpdate" que dejen de correr
        //por lo que la gravedad y el movimiento se congelaran al instante
        Time.timeScale = 0f;

        //Cambiamos el estado de "juegoPausado" para indicar que el juego se encuentra pausado
        juegoPausado = true;
        Debug.Log("Juego pausado");
    }

    //Funcion para reanudar el tiempo y el movimiento en el escenario
   public void Reanudar()
    {
        // Recorremos la lista para apagar cada elemento
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(false);
        }

        //Indicamos a los "FixedUpdate" que ya pueden seguir trabajando
        //por lo que se reanuda el tiempo y el movimiento del escenario
        Time.timeScale = 1f;

        //Cambiamos el estado de la bandera para indicar que el juego ya no se encuentra en estado "Pausado"
        juegoPausado = false;
    }

    //Funcion para evitar que al volver a cargar la escena se quede congelada
    void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

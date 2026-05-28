using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el menu de pausa del juego, ademas de programar los botones de accion
/// de este menu de pausa.
/// </summary>
public class ControlarPausaMenu : MonoBehaviour
{
    // Propiedad estática accesible desde otros scripts
    public static bool IsPausado { get; private set; }

    public GameObject[] objectoMenuPausa;
    private bool juegoPausado = false;

    private void Start()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        IsPausado = false; // <-- CORRECCIÓN 1: Asegurar que inicie en false al cargar el nivel

        // Bucle para asegurar que todos los objetos de la lista inicien apagados
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    void Update()
    {
        // Detectar teclas "Esc" para entrar o salir del menu de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }

        // CONDICIÓN PROTEGIDA: Solo reinicia con 'R' si el juego está en pausa
        if (juegoPausado && Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarNivel();
        }
    }

    // Con esta funcion controlaremos cuando el escenario entre en pausa
    public void Pausar()
    {
        // Recorremos la lista para encender cada elemento (Fondo, Botones, etc.)
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(true);
        }

        Time.timeScale = 0f;
        juegoPausado = true;

        // <-- CORRECCIÓN 2: Avisar a los demás scripts que el juego REALMENTE está pausado
        IsPausado = true;

        Debug.Log("[QA PAUSA] Juego pausado. IsPausado = true.");
    }

    // Funcion para reanudar el tiempo y el movimiento en el escenario
    public void Reanudar()
    {
        // Recorremos la lista para apagar cada elemento
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(false);
        }

        Time.timeScale = 1f;
        juegoPausado = false;

        // <-- CORRECCIÓN 3: Avisar a los demás scripts que el juego volvió a correr
        IsPausado = false;

        Debug.Log("[QA PAUSA] Juego reanudado. IsPausado = false.");
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        IsPausado = false; // Resetear antes de cambiar de escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        IsPausado = false; // Resetear antes de cambiar de escena
        SceneManager.LoadScene("MenuPrincipal");
    }
}
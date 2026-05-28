using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

/// <summary>
/// Controla el menu de pausa del juego, ademas de programar los botones de accion
/// de este menu de pausa.
/// </summary>
public class ControlarPausaMenu : MonoBehaviour
{
    public static bool IsPausado { get; private set; }

    // Propiedad estática. Por defecto iniciará en false al abrir el juego,
    // pero mantendrá su valor (true) al cambiar de escena si ya arrancó.
    public static bool CronometroActivo { get; set; }

    public GameObject[] objectoMenuPausa;
    private bool juegoPausado = false;

    [Header("Módulo de Tiempo Global (Pausa)")]
    public TextMeshProUGUI txtTiempoGlobalPausa;

    private void Start()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        IsPausado = false;

        string escenaActual = SceneManager.GetActiveScene().name;

        // Si el jugador de alguna manera regresó al menú principal, aseguramos que se apague
        if (escenaActual == "MenuPrincipal")
        {
            CronometroActivo = false;
            PlayerPrefs.SetFloat("Partida_TiempoActual", 0f);
            PlayerPrefs.Save();
        }
        // SI YA PASAMOS EL TUTORIAL: Forzamos que el cronómetro se encienda automáticamente
        // en cualquier escena que tenga este script de pausa (Nivel 1, Nivel 2, etc.)
        else if (escenaActual != "Tutorial" && escenaActual != "MenuPrincipal")
        {
            CronometroActivo = true;
        }

        // Bucle para asegurar que todos los objetos de la lista inicien apagados
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    void Update()
    {
        // --- CRONÓMETRO IMPLACABLE ---
        // Corre si ya se activó el cronómetro, si no está pausado el script y si el timeScale no es 0
        if (CronometroActivo && !juegoPausado && Time.timeScale != 0f)
        {
            float tiempoAcumulado = PlayerPrefs.GetFloat("Partida_TiempoActual", 0f);
            tiempoAcumulado += Time.deltaTime;
            PlayerPrefs.SetFloat("Partida_TiempoActual", tiempoAcumulado);
        }

        // Detectar teclas "Esc" para entrar o salir del menu de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }

        // Reinicio con R
        if (juegoPausado && Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarNivel();
        }
    }

    public void Pausar()
    {
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(true);
        }

        Time.timeScale = 0f;
        juegoPausado = true;
        IsPausado = true;

        if (txtTiempoGlobalPausa != null)
        {
            float tiempoAlPausar = PlayerPrefs.GetFloat("Partida_TiempoActual", 0f);
            txtTiempoGlobalPausa.text = FormatearTiempo(tiempoAlPausar);
        }
    }

    public void Reanudar()
    {
        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(false);
        }

        Time.timeScale = 1f;
        juegoPausado = false;
        IsPausado = false;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        IsPausado = false;
        // Al recargar la escena, NO apagamos CronometroActivo para que el tiempo siga sumando los segundos del intento fallido
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        IsPausado = false;
        CronometroActivo = false;

        PlayerPrefs.SetFloat("Partida_TiempoActual", 0f);
        PlayerPrefs.Save();

        if (LevelLoader.Instance != null) LevelLoader.Instance.nivelGlobal = 1;

        SceneManager.LoadScene("MenuPrincipal");
    }

    private string FormatearTiempo(float tiempoEnSegundos)
    {
        if (tiempoEnSegundos <= 0) return "00:00:00";
        TimeSpan t = TimeSpan.FromSeconds(tiempoEnSegundos);
        return string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
    }
}
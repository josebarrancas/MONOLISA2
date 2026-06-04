using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ControlarPausaMenu : MonoBehaviour
{
    public static bool IsPausado { get; private set; }
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

        if (escenaActual == "MenuPrincipal")
        {
            CronometroActivo = false;
            var datos = ManejadorGuardadoTexto.CargarDatos();
            ManejadorGuardadoTexto.GuardarDatos(0f, datos.recordMejorTiempoNormal, 0, datos.recordMejorPuntajeNormal);
        }

        foreach (GameObject obj in objectoMenuPausa)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    void Update()
    {
        if (CronometroActivo && !juegoPausado && Time.timeScale != 0f)
        {
            var datos = ManejadorGuardadoTexto.CargarDatos();
            datos.partidaTiempoActual += Time.deltaTime;

            ManejadorGuardadoTexto.GuardarDatos(
                datos.partidaTiempoActual,
                datos.recordMejorTiempoNormal,
                datos.partidaPuntajeActual,
                datos.recordMejorPuntajeNormal
            );
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }

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
            var datos = ManejadorGuardadoTexto.CargarDatos();
            txtTiempoGlobalPausa.text =  FormatearTiempo(datos.partidaTiempoActual);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        IsPausado = false;
        CronometroActivo = false;

        var datos = ManejadorGuardadoTexto.CargarDatos();
        ManejadorGuardadoTexto.GuardarDatos(0f, datos.recordMejorTiempoNormal, 0, datos.recordMejorPuntajeNormal);

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
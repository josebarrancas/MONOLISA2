using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("Algoritmo Skill (Estado Actual)")]
    [Range(0, 100)] public float skillActual = 50f;
    public string estadoActual = "Based";

    [Header("Métricas del Nivel (Debug)")]
    public int intentosNivel = 1;
    public float tiempoNivel = 0f;
    public int poderesUtilizados = 0;

    private bool cronometroActivo = false;

    [Header("Herramientas QA")]
    public bool mostrarDebugUI = true;

    public Dictionary<string, int> historialPoderes = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Pantalla_Seleccion" || scene.name == "Hub")
        {
            cronometroActivo = false;
        }
        else
        {
            tiempoNivel = 0f;
            poderesUtilizados = 0;
            cronometroActivo = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) mostrarDebugUI = !mostrarDebugUI;

        if (cronometroActivo)
        {
            tiempoNivel += Time.deltaTime;
        }
    }

    public void RegistraUsoPoder(string nombrePoder)
    {
        poderesUtilizados++;
        if (historialPoderes.ContainsKey(nombrePoder))
            historialPoderes[nombrePoder]++;
        else
            historialPoderes.Add(nombrePoder, 1);
    }

    public void CalcularResultados()
    {
        float tiempoFinal = tiempoNivel;

        int pIntentos = 0;
        if (intentosNivel == 1) pIntentos = 15;
        else if (intentosNivel <= 3) pIntentos = 5;
        else if (intentosNivel <= 5) pIntentos = 0;
        else if (intentosNivel <= 8) pIntentos = -10;
        else pIntentos = -20;

        int pTiempo = 0;
        if (tiempoFinal < 45f) pTiempo = 10;
        else if (tiempoFinal <= 90f) pTiempo = 5;
        else if (tiempoFinal <= 180f) pTiempo = 0;
        else pTiempo = -10;

        int pPoder = 0;
        if (poderesUtilizados == 0) pPoder = 15;
        else if (poderesUtilizados <= 3) pPoder = 5;
        else if (poderesUtilizados <= 6) pPoder = 0;
        else pPoder = -10;

        float puntuacionNivel = pIntentos + pTiempo + pPoder;
        float factorCambio = puntuacionNivel * 0.4f;
        skillActual = Mathf.Clamp(skillActual + factorCambio, 0, 100);

        if (skillActual < 40) estadoActual = "Issue";
        else if (skillActual > 70) estadoActual = "Solution";
        else estadoActual = "Based";

        intentosNivel = 1;
    }

    // --- NUEVO MÉTODO CORREGIDO PARA EL REINICIO DESDE LA PANTALLA FINAL ---
    public void ResetearSkillCompletamente()
    {
        skillActual = 50f;
        estadoActual = "Based";
        intentosNivel = 1; // Tu estado inicial de intentos según tu lógica
        tiempoNivel = 0f;
        poderesUtilizados = 0;
        cronometroActivo = false;
        historialPoderes.Clear();
    }

    // --- UI DE DEBUG ---
    void OnGUI()
    {
        if (!mostrarDebugUI) return;

        int ancho = 380;
        int alto = 200;
        Rect rectPanel = new Rect(20, 20, ancho, alto);

        GUIStyle estiloCaja = new GUIStyle(GUI.skin.box);
        estiloCaja.fontSize = 18;
        estiloCaja.fontStyle = FontStyle.Bold;
        GUI.Box(rectPanel, "QA Debug - MONOLISA2 (F3 ocultar)", estiloCaja);

        GUIStyle estiloTexto = new GUIStyle();
        estiloTexto.normal.textColor = Color.white;
        estiloTexto.fontSize = 22;
        estiloTexto.richText = true;
        estiloTexto.padding = new RectOffset(15, 15, 40, 15);

        string info = $"\n" +
                      $"Intento Actual: <b>{intentosNivel}</b>\n" +
                      $"Tiempo: <b>{tiempoNivel:F1} seg</b>\n" +
                      $"Poderes Usados: <b>{poderesUtilizados}</b>\n" +
                      $"Skill Level: <b>{skillActual:F1}</b> <color=cyan>[{estadoActual}]</color>";

        GUI.Label(rectPanel, info, estiloTexto);
    }
}
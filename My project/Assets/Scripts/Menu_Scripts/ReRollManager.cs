using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReRollManager : MonoBehaviour
{
    [Header("UI - Estadísticas")]
    public TextMeshProUGUI textoIntentos;
    public TextMeshProUGUI textoRerolls;

    [Header("UI - Monitores")]
    public TextMeshProUGUI[] nombresPoderesUI;
    public Image[] iconosCandadoUI;
    public GameObject[] marcosSeleccionUI;
    public Image[] imagenesPoderesUI;

    [Header("Ajustes de Partida")]
    public int intentosRestantes = 11;
    public int rerollsActuales = 2;

    [Header("Estado de Selección")]
    public string[] poderesEnPantalla = new string[3];
    public bool[] slotsBloqueados = new bool[3];
    public int slotSeleccionado = 0;

    [System.Serializable]
    public class EstadisticasPoder
    {
        public string nombre;
        public int victoriasTotales;
        [HideInInspector] public float pesoActual;
    }

    private List<EstadisticasPoder> mazoMatematico;

    void Start()
    {
        ConstruirMazoDesdeLogicManager();
        RealizarReRoll(true);
        AplicarRestriccionesDelJuez();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) CambiarSeleccion(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) CambiarSeleccion(1);

        if (Input.GetKeyDown(KeyCode.X))
        {
            // Bloqueo por Poder Obligatorio sigue mandando
            if (EsSlotBloqueadoPorJuez(slotSeleccionado)) return;
            // En Monitor Fallando SÍ dejamos bloquear
            if (EsReglasDeLaCasaActivo()) return;

            AlternarBloqueo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // En Monitor Fallando SÍ dejamos rerollear
            if (EsReglasDeLaCasaActivo()) return;
            RealizarReRoll(false);
        }

        if (Input.GetKeyDown(KeyCode.Z)) IniciarIntento();
    }

    private bool EsReglasDeLaCasaActivo() => CondicionesManager.Instance != null && CondicionesManager.Instance.condicionActual == TipoCondicion.ReglasDeLaCasa;
    private bool EsSlotBloqueadoPorJuez(int index) => CondicionesManager.Instance != null && CondicionesManager.Instance.condicionActual == TipoCondicion.PoderObligatorio && CondicionesManager.Instance.indicePoderBloqueado == index;

    private void CambiarSeleccion(int direccion)
    {
        slotSeleccionado = (slotSeleccionado + direccion + 3) % 3;
        ActualizarVisuales();
    }

    private void AlternarBloqueo()
    {
        slotsBloqueados[slotSeleccionado] = !slotsBloqueados[slotSeleccionado];
        ActualizarVisuales();
    }

    private void AplicarRestriccionesDelJuez()
    {
        if (CondicionesManager.Instance == null) return;
        if (CondicionesManager.Instance.condicionActual == TipoCondicion.PoderObligatorio)
        {
            int index = CondicionesManager.Instance.indicePoderBloqueado;
            if (index >= 0 && index < slotsBloqueados.Length) slotsBloqueados[index] = true;
        }
        ActualizarVisuales();
    }

    public void ActualizarVisuales()
    {
        bool monitorFallando = CondicionesManager.Instance != null && CondicionesManager.Instance.condicionActual == TipoCondicion.MonitorFallando;
        bool reglasCasa = EsReglasDeLaCasaActivo();

        if (textoIntentos) textoIntentos.text = "Intentos: " + intentosRestantes;
        if (textoRerolls) textoRerolls.text = reglasCasa ? "<color=red>BLOQUEADO</color>" : "Re-rolls: " + rerollsActuales;

        for (int i = 0; i < 3; i++)
        {
            bool obligatorio = EsSlotBloqueadoPorJuez(i);

            // --- LÓGICA DE MONITOR FALLANDO ---
            if (monitorFallando)
            {
                nombresPoderesUI[i].text = GlitchText("ERROR_SYSTEM_DATA"); // Ocultamos el nombre real
                imagenesPoderesUI[i].color = Color.black; // Ocultamos el icono
            }
            else
            {
                nombresPoderesUI[i].text = poderesEnPantalla[i];
                nombresPoderesUI[i].color = obligatorio ? Color.red : Color.white;

                PoderData original = Logic_Manager.instance.mazoJugador.FirstOrDefault(p => p.nombre == poderesEnPantalla[i]);
                if (original != null && original.icono != null)
                {
                    imagenesPoderesUI[i].sprite = original.icono;
                    imagenesPoderesUI[i].color = Color.white;
                }
            }

            // El candado se sigue viendo para que sepa qué tiene bloqueado
            if (iconosCandadoUI[i])
            {
                iconosCandadoUI[i].enabled = slotsBloqueados[i];
                iconosCandadoUI[i].color = obligatorio ? Color.red : (reglasCasa ? Color.gray : Color.white);
            }

            if (marcosSeleccionUI[i]) marcosSeleccionUI[i].SetActive(i == slotSeleccionado);
        }
    }

    private string GlitchText(string input)
    {
        string chars = "!@#$%^&*()_+-=[]{}|;':,.<>?/0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] glitch = new char[input.Length];
        for (int i = 0; i < glitch.Length; i++) glitch[i] = chars[Random.Range(0, chars.Length)];
        return new string(glitch);
    }

    private void RealizarReRoll(bool esGratis)
    {
        if (!esGratis)
        {
            if (rerollsActuales <= 0) return;
            rerollsActuales--;
        }

        CalcularPesosPorDificultad();
        List<string> excluidos = new List<string>();
        for (int i = 0; i < 3; i++) if (slotsBloqueados[i] && !string.IsNullOrEmpty(poderesEnPantalla[i])) excluidos.Add(poderesEnPantalla[i]);

        for (int i = 0; i < 3; i++)
        {
            if (!slotsBloqueados[i])
            {
                poderesEnPantalla[i] = SortearPoder(excluidos);
                excluidos.Add(poderesEnPantalla[i]);
            }
        }
        ActualizarVisuales();
    }

    private void IniciarIntento()
    {
        if (intentosRestantes <= 0) return;
        intentosRestantes--;
        if (Logic_Manager.instance != null)
        {
            Logic_Manager.instance.poderesSeleccionados.Clear();
            for (int i = 0; i < 3; i++)
            {
                PoderData data = Logic_Manager.instance.mazoJugador.FirstOrDefault(p => p.nombre == poderesEnPantalla[i]);
                if (data != null) Logic_Manager.instance.poderesSeleccionados.Add(data);
            }
        }
        if (LevelLoader.Instance != null && !string.IsNullOrEmpty(LevelLoader.Instance.proximaEscenaCargar))
            SceneManager.LoadScene(LevelLoader.Instance.proximaEscenaCargar);
    }

    // (Funciones de ConstruirMazo, CalcularPesos y SortearPoder se mantienen igual que antes)
    private void ConstruirMazoDesdeLogicManager()
    {
        mazoMatematico = new List<EstadisticasPoder>();
        if (Logic_Manager.instance != null && Logic_Manager.instance.mazoJugador != null)
        {
            foreach (PoderData data in Logic_Manager.instance.mazoJugador)
            {
                EstadisticasPoder stat = new EstadisticasPoder { nombre = data.nombre, victoriasTotales = 0 };
                if (SkillManager.Instance != null && SkillManager.Instance.historialPoderes.TryGetValue(stat.nombre, out int usos))
                    stat.victoriasTotales = usos;
                mazoMatematico.Add(stat);
            }
        }
    }

    private void CalcularPesosPorDificultad()
    {
        string estado = SkillManager.Instance != null ? SkillManager.Instance.estadoActual : "Based";
        var ordenados = mazoMatematico.OrderByDescending(p => p.victoriasTotales).ToList();
        for (int i = 0; i < ordenados.Count; i++)
        {
            var p = ordenados[i]; p.pesoActual = 100f;
            if (estado == "Issue" && i < 3) p.pesoActual *= (p.victoriasTotales >= 35) ? 1.1f : 1.5f;
            else if (estado == "Solution")
            {
                if (i < 3) p.pesoActual *= 0.75f;
                else if (i >= ordenados.Count - 3) p.pesoActual *= 1.10f;
            }
        }
    }

    private string SortearPoder(List<string> excluidos)
    {
        var candidatos = mazoMatematico.Where(p => !excluidos.Contains(p.nombre)).ToList();
        if (candidatos.Count == 0) return "Vacío";
        float total = candidatos.Sum(p => p.pesoActual);
        float aleatorio = Random.Range(0, total);
        foreach (var c in candidatos)
        {
            aleatorio -= c.pesoActual;
            if (aleatorio <= 0) return c.nombre;
        }
        return candidatos.Last().nombre;
    }
}
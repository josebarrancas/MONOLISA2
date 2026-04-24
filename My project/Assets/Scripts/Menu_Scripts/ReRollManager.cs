using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

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
        RealizarReRoll(true); // Tirada inicial gratuita
    }

    void Update()
    {
        // Controles
        if (Input.GetKeyDown(KeyCode.LeftArrow)) CambiarSeleccion(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) CambiarSeleccion(1);
        if (Input.GetKeyDown(KeyCode.X)) AlternarBloqueo();
        if (Input.GetKeyDown(KeyCode.R)) RealizarReRoll(false);
        if (Input.GetKeyDown(KeyCode.Z)) IniciarIntento();
    }

    private void CambiarSeleccion(int direccion)
    {
        // Fórmula matemática para navegar en círculos (0 -> 1 -> 2 -> 0)
        slotSeleccionado = (slotSeleccionado + direccion + 3) % 3;
        ActualizarVisuales();
    }

    private void AlternarBloqueo()
    {
        slotsBloqueados[slotSeleccionado] = !slotsBloqueados[slotSeleccionado];
        ActualizarVisuales();
    }

    private void RealizarReRoll(bool esGratis)
    {
        if (!esGratis)
        {
            if (rerollsActuales <= 0) return; // Ya no hay rerolls
            rerollsActuales--;
        }

        CalcularPesosPorDificultad();

        // Anotamos qué poderes ya están en los monitores bloqueados para no repetirlos
        List<string> excluidos = new List<string>();
        for (int i = 0; i < 3; i++)
        {
            if (slotsBloqueados[i] && !string.IsNullOrEmpty(poderesEnPantalla[i]))
            {
                excluidos.Add(poderesEnPantalla[i]);
            }
        }

        // Sorteamos nuevos poderes para los monitores libres
        for (int i = 0; i < 3; i++)
        {
            if (!slotsBloqueados[i])
            {
                string nuevo = SortearPoder(excluidos);
                poderesEnPantalla[i] = nuevo;
                excluidos.Add(nuevo); // Lo añadimos a la lista de exclusión para que el siguiente monitor no saque el mismo
            }
        }

        ActualizarVisuales();
    }

    private void ActualizarVisuales()
    {
        // 1. Textos generales
        if (textoIntentos) textoIntentos.text = "Intentos: " + intentosRestantes;
        if (textoRerolls) textoRerolls.text = "Re-rolls: " + rerollsActuales;

        for (int i = 0; i < 3; i++)
        {
            string nombreActual = poderesEnPantalla[i];

            // 2. Nombre del poder
            if (nombresPoderesUI[i]) nombresPoderesUI[i].text = nombreActual;

            // 3. Candado y Marco
            if (iconosCandadoUI[i]) iconosCandadoUI[i].enabled = slotsBloqueados[i];
            if (marcosSeleccionUI[i]) marcosSeleccionUI[i].SetActive(i == slotSeleccionado);

            // 4. Asignación automática de la imagen (Sprite)
            if (imagenesPoderesUI[i] != null && Logic_Manager.instance != null && Logic_Manager.instance.mazoJugador != null)
            {
                // Buscamos el objeto original en el Logic_Manager
                PoderData original = Logic_Manager.instance.mazoJugador.FirstOrDefault(p => p.nombre == nombreActual);

                if (original != null && original.icono != null)
                {
                    imagenesPoderesUI[i].sprite = original.icono;
                    imagenesPoderesUI[i].color = Color.white; // Evita que se vea transparente
                }
            }
        }
    }

    private void ConstruirMazoDesdeLogicManager()
    {
        mazoMatematico = new List<EstadisticasPoder>();

        // Verificamos que el Logic_Manager esté vivo y tenga datos
        if (Logic_Manager.instance != null && Logic_Manager.instance.mazoJugador != null && Logic_Manager.instance.mazoJugador.Count > 0)
        {
            foreach (PoderData data in Logic_Manager.instance.mazoJugador)
            {
                if (data == null) continue;

                EstadisticasPoder stat = new EstadisticasPoder();
                stat.nombre = data.nombre;
                stat.victoriasTotales = 0; // Valor por defecto

                // Leemos las victorias del historial del SkillManager
                if (SkillManager.Instance != null)
                {
                    if (SkillManager.Instance.historialPoderes.TryGetValue(stat.nombre, out int usos))
                    {
                        stat.victoriasTotales = usos;
                    }
                }

                mazoMatematico.Add(stat);
            }
            Debug.Log($"<color=green>ReRollManager:</color> Mazo de {mazoMatematico.Count} poderes importado automáticamente.");
        }
        else
        {
            Debug.LogError("Error crítico: No se encontró el Logic_Manager o su mazo está vacío. Recuerde jugar desde la escena del Hub.");
        }
    }

    private void CalcularPesosPorDificultad()
    {
        // Determinamos el estado (Issue, Solution, Based)
        string estado = SkillManager.Instance != null ? SkillManager.Instance.estadoActual : "Based";

        // Ordenamos los poderes de más a menos victorias para saber el TOP y BOTTOM
        var ordenados = mazoMatematico.OrderByDescending(p => p.victoriasTotales).ToList();

        for (int i = 0; i < ordenados.Count; i++)
        {
            var p = ordenados[i];
            p.pesoActual = 100f; // Peso normal

            if (estado == "Issue" && i < 3) // TOP 3 ayudas
            {
                if (p.victoriasTotales >= 35) p.pesoActual *= 1.1f;
                else if (p.victoriasTotales >= 28) p.pesoActual *= 1.2f;
                else if (p.victoriasTotales >= 21) p.pesoActual *= 1.3f;
                else if (p.victoriasTotales >= 14) p.pesoActual *= 1.4f;
                else p.pesoActual *= 1.5f;
            }
            else if (estado == "Solution") // Penalización y ayuda a los menos usados
            {
                if (i < 3) p.pesoActual *= 0.75f; // TOP 3 (25% menos probable)
                else if (i >= ordenados.Count - 3) p.pesoActual *= 1.10f; // BOTTOM 3 (10% más probable)
            }
        }
    }

    private string SortearPoder(List<string> excluidos)
    {
        // Filtramos para evitar los que ya están en monitores bloqueados
        var candidatos = mazoMatematico.Where(p => !excluidos.Contains(p.nombre)).ToList();

        if (candidatos.Count == 0) return "Vacío";

        // Sistema de Ruleta de Probabilidades
        float total = candidatos.Sum(p => p.pesoActual);
        float aleatorio = Random.Range(0, total);

        foreach (var c in candidatos)
        {
            aleatorio -= c.pesoActual;
            if (aleatorio <= 0) return c.nombre;
        }

        return candidatos.Last().nombre; // Respaldo de seguridad
    }

    private void IniciarIntento()
    {
        if (intentosRestantes <= 0) return;
        intentosRestantes--;

        if (Logic_Manager.instance != null && Logic_Manager.instance.mazoJugador != null)
        {
            Logic_Manager.instance.poderesSeleccionados.Clear();

            for (int i = 0; i < 3; i++)
            {
                string nombreElegido = poderesEnPantalla[i];

                // Buscamos el archivo Data original que coincide con ese nombre
                PoderData dataOriginal = Logic_Manager.instance.mazoJugador.FirstOrDefault(p => p.nombre == nombreElegido);

                if (dataOriginal != null)
                {
                    Logic_Manager.instance.poderesSeleccionados.Add(dataOriginal);
                }
            }

            Debug.Log($"<color=cyan>Poderes confirmados y empacados para el nivel:</color> {poderesEnPantalla[0]}, {poderesEnPantalla[1]}, {poderesEnPantalla[2]}");
        }

        if (LevelLoader.Instance != null && !string.IsNullOrEmpty(LevelLoader.Instance.proximaEscenaCargar))
        {
            SceneManager.LoadScene(LevelLoader.Instance.proximaEscenaCargar);
        }
        else
        {
            Debug.LogError("Error: El LevelLoader no tiene registrada la próxima escena a cargar.");
        }
    }
}
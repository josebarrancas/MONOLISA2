using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Compara las etiquetas de los poderes y dictamita cuales son las 3 etiquetas 
/// que mas se repiten en los 9 poderes que tiene el jugador
/// </summary>
public class Logic_Manager : MonoBehaviour
{
    public static Logic_Manager instance; // Para acceder fácil: Logic_Manager.instance

    [Header("Configuracion de Inicio")]
    public List<PoderData> bibliotecaTotalPoderes;

    public List<PoderData> mazoJugador;

    [Header("Poderes Activos (Para el Nivel)")]
    public List<PoderData> poderesSeleccionados = new List<PoderData>();

    private void Start()
    {
        if (mazoJugador.Count == 0)
        {
            RepartirPoderesIniciales(9);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<string> ObtenerMayoriaEtiquetas()
    {
        if (mazoJugador == null || mazoJugador.Count == 0)
        {
            Debug.LogWarning("El mazo de poderes esta vacio! Devolviendo lista vacia");
            Debug.Log($"Analizando mazo. Cantidad de poderes: {mazoJugador.Count}");
            return new List<string>();
        }

        // Tu lógica de LINQ que está excelente
        return mazoJugador.SelectMany(p => p.etiquetas)
            .GroupBy(e => e)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key)
            .ToList();
    }

    public void RepartirPoderesIniciales(int cantidad)
    {
        mazoJugador = bibliotecaTotalPoderes.OrderBy(x => Random.value).Take(cantidad).ToList();
        Debug.Log($"Se han otorgado {mazoJugador.Count} poderes.");

        // LE AVISAMOS AL HUD QUE YA PUEDE LEER EL MAZO
        FindObjectOfType<PowerSelector>().PrepararPartida();
    }

}

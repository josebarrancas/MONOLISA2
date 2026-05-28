using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CajasPoderesManager : MonoBehaviour
{
    public static CajasPoderesManager Instance;

    [Header("Configuración del Nivel")]
    public List<CajaPoder> cajasEnNivel = new List<CajaPoder>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += AlCargarNuevaEscena;
    private void OnDisable() => SceneManager.sceneLoaded -= AlCargarNuevaEscena;

    private void AlCargarNuevaEscena(Scene escena, LoadSceneMode modo)
    {
        CajaPoder[] cajasEncontradas = FindObjectsOfType<CajaPoder>();
        cajasEnNivel = new List<CajaPoder>(cajasEncontradas);

        if (cajasEnNivel.Count > 0)
        {
            // Retrasamos 0.1s para darle tiempo al HUD de prepararse primero
            Invoke(nameof(DistribuirPoderesEnCajas), 0.1f);
        }
    }

    public void DistribuirPoderesEnCajas()
    {
        // Tomamos la lista original de los 9 poderes del jugador (mazoJugador)
        if (Logic_Manager.instance == null || Logic_Manager.instance.mazoJugador == null) return;

        List<PoderData> poolDisponibles = new List<PoderData>(Logic_Manager.instance.mazoJugador);

        // FILTRO ANTI-DUPLICADOS: Leemos el HUD y eliminamos los que ya tiene equipados
        PowerSelectorPrincipal hud = FindObjectOfType<PowerSelectorPrincipal>();
        if (hud != null && hud.poderesEnUso != null)
        {
            foreach (PoderData equipado in hud.poderesEnUso)
            {
                if (equipado != null)
                {
                    poolDisponibles.RemoveAll(p => p.nombre == equipado.nombre);
                }
            }
        }

        // Mezclar aleatoriamente las opciones restantes (Fisher-Yates)
        for (int i = 0; i < poolDisponibles.Count; i++)
        {
            PoderData temp = poolDisponibles[i];
            int randomIndex = Random.Range(i, poolDisponibles.Count);
            poolDisponibles[i] = poolDisponibles[randomIndex];
            poolDisponibles[randomIndex] = temp;
        }

        // Asignar los poderes ya filtrados a las cajas
        for (int i = 0; i < cajasEnNivel.Count; i++)
        {
            if (i < poolDisponibles.Count)
                cajasEnNivel[i].AsignarPoder(poolDisponibles[i]);
            else
                cajasEnNivel[i].AsignarPoder(null); // Caja vacía si se acaban los poderes
        }
    }

    public void ResetearCajasPorIntentoPerdido()
    {
        DistribuirPoderesEnCajas();
    }
}
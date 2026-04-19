using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaTutorial : MonoBehaviour
{
    [Header("Referencias de Scripts")]
    public Logic_Manager logicManager;    // Arrastra el objeto que tiene el Logic_Manager
    public SorteoNiveles sorteador;      // Arrastra el objeto que tiene el SorteoNiveles

    [Header("Configuración de Partida")]
    public int nivelesPorPartida = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos que sea el jugador quien cruza
        if (other.CompareTag("Player"))
        {
            EjecutarConfiguracionDePartida();
        }
    }

    void EjecutarConfiguracionDePartida()
    {
        // PASO 1: Obtener el Top 3 de etiquetas del mazo de 9 poderes
        List<string> top3 = logicManager.ObtenerMayoriaEtiquetas();

        if (top3.Count > 0)
        {
            // PASO 2: Mandar ese Top 3 al sorteador para que elija la mejor ronda
            List<LevelData> rondaGanadora = sorteador.mejorRondaNiveles(top3, nivelesPorPartida);

            // PASO 3: Guardar la lista de niveles en GameData para que persistan entre escenas
            GameData.nivelesDeEstaPartida = rondaGanadora;
            GameData.indiceNivelActual = 0;

            // PASO 4: Cargar el primer nivel de la ronda ganadora
            if (rondaGanadora.Count > 0)
            {
                SceneManager.LoadScene(rondaGanadora[0].nombreEscena);
            }
            else
            {
                Debug.LogError("El sorteador no devolvió niveles. Revisa tu lista de niveles guardados.");
            }
        }
        else
        {
            Debug.LogError("No se pudieron obtener etiquetas del mazo. ¿El mazo tiene 9 poderes?");
        }
    }
}
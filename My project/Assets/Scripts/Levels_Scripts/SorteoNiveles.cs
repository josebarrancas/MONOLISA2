using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gestiona el sorteo de niveles al tocar la puerta física en el mundo (Hub).
/// </summary>
public class SorteoNiveles : MonoBehaviour
{
    [Header("Base de Datos")]
    public List<LevelData> nivelesGuardados;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que quien toca la puerta sea el jugador
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡El jugador tocó la puerta de entrada! Calculando niveles...");
            IniciarPartida();
        }
    }

    private void IniciarPartida()
    {
        // 1. Definir cuántos niveles quieres en la ronda
        int cantidadDeNivelesAJugar = 3;

        // 2. Etiquetas temporales (luego las conectarás a tu sistema de guardado)
        List<string> etiquetasJugador = new List<string>() { "movimiento", "fisicas", "impulso" };

        // 3. Ejecutamos tu lógica matemática
        List<LevelData> rondaGanadora = mejorRondaNiveles(etiquetasJugador, cantidadDeNivelesAJugar);

        // 4. Enviamos la lista al LevelLoader y arrancamos
        if (LevelLoader.Instance != null)
        {
            LevelLoader.Instance.EstablecerRondaGanadora(rondaGanadora);
            LevelLoader.Instance.CargarSiguienteNivel("Based");
        }
        else
        {
            Debug.LogError("Fallo Crítico: No se encontró el LevelLoader. Asegúrate de tener uno en esta escena inicial.");
        }
    }


    public List<LevelData> mejorRondaNiveles(List<string> top3Etiquetas, int nivelesPorRonda)
    {
        List<List<LevelData>> rondasSimuladas = new List<List<LevelData>>();
        List<float> probabilidadRondas = new List<float>();

        for (int i = 0; i < 3; i++)
        {
            List<LevelData> rondaActual = sortearNiveles(nivelesPorRonda);
            rondasSimuladas.Add(rondaActual);

            float puntajeTotalRonda = 0;
            foreach (LevelData nivel in rondaActual)
            {
                int coincidencias = 0;
                foreach (string problemaNivel in nivel.etiquetasNivel)
                {
                    if (LogicaPoderes.Relaciones.ContainsKey(problemaNivel))
                    {
                        string[] solucionesValidas = LogicaPoderes.Relaciones[problemaNivel];
                        if (top3Etiquetas.Any(poderJugador => solucionesValidas.Contains(poderJugador)))
                        {
                            coincidencias++;
                        }
                    }
                }
                float compatibilidadNivel = (nivel.etiquetasNivel.Length > 0) ? (float)coincidencias / nivel.etiquetasNivel.Length : 0;
                puntajeTotalRonda += compatibilidadNivel;
            }
            probabilidadRondas.Add(puntajeTotalRonda / nivelesPorRonda);
        }

        int mejorIndice = probabilidadRondas.IndexOf(probabilidadRondas.Max());
        Debug.Log($"Simulación finalizada. Elegida Ronda {mejorIndice} con {probabilidadRondas[mejorIndice] * 100}% de compatibilidad.");

        return rondasSimuladas[mejorIndice];
    }

    private List<LevelData> sortearNiveles(int cantidad)
    {
        return nivelesGuardados.OrderBy(x => Random.value).Take(cantidad).ToList();
    }
}
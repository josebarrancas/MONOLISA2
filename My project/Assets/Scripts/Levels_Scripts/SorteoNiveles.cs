using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gestiona la simulacion de rondas de niveles para elegir la que tenga
/// mayor compatibilidad con los poderes actuales del jugador
/// </summary>
public class SorteoNiveles : MonoBehaviour
{
    [Header("Base de Datos")]
    public List<LevelData> nivelesGuardados;

    /// <summary>
    /// Simula 3 rondas de niveles al azar y devuelve la que tiene 
    /// mayor probabilidad de exito segun el Top 3 de etiquetas del jugador
    /// </summary>
    public List<LevelData> mejorRondaNiveles(List<string> top3Etiquetas, int nivelesPorRonda)
    {
        List<List<LevelData>> rondasSimuladas = new List<List<LevelData>>();
        List<float> probabilidadRondas = new List<float>();

        // 1.-Ejecutamos 3 simulaciones independientes
        for (int i = 0; i < 3; i++)
        {
            List<LevelData> rondaActual = sortearNiveles(nivelesPorRonda);
            rondasSimuladas.Add(rondaActual);

            // --- Logica de probabilidad ---
            float puntajeTotalRonda = 0;

            foreach (LevelData nivel in rondaActual)
            {
                int coincidencias = 0;

                // Analizamos cada etiqueta (problema) que presenta el nivel
                foreach (string problemaNivel in nivel.etiquetasNivel)
                {
                    // 1. Buscamos en nuestra BIBLIOTECA si el problema tiene solución
                    if (LogicaPoderes.Relaciones.ContainsKey(problemaNivel))
                    {
                        // 2. Obtenemos la lista de etiquetas de poder que SI resuelven este problema
                        string[] solucionesValidas = LogicaPoderes.Relaciones[problemaNivel];

                        // 3. Verificamos si el jugador tiene AL MENOS UNA de esas soluciones en su Top 3
                        // Usamos .Any() de LINQ para buscar rápido
                        if (top3Etiquetas.Any(poderJugador => solucionesValidas.Contains(poderJugador)))
                        {
                            coincidencias++;
                        }
                    }
                }

                // Calculamos la compatibilidad final del nivel (0.0 a 1.0)
                float compatibilidadNivel = (nivel.etiquetasNivel.Length > 0) ? (float)coincidencias / nivel.etiquetasNivel.Length : 0;

                puntajeTotalRonda += compatibilidadNivel;
            }

            // Guardamos el promedio de probabilidad de esta ronda
            probabilidadRondas.Add(puntajeTotalRonda / nivelesPorRonda);
        }

        // 2. Buscamos el indice de la ronda que obtuvo el valor maximo de probabilidad
        int mejorIndice = probabilidadRondas.IndexOf(probabilidadRondas.Max());

        Debug.Log($"Simulación finalizada. Elegida Ronda {mejorIndice} con {probabilidadRondas[mejorIndice] * 100}% de compatibilidad.");

        return rondasSimuladas[mejorIndice];
    }

    /// <summary>
    /// Baraja los niveles disponibles y selecciona una cantidad específica.
    /// </summary>
    private List<LevelData> sortearNiveles(int cantidad)
    {
        // Usamos LINQ para desordenar la lista y tomar el numero de niveles pedido
        return nivelesGuardados.OrderBy(x => Random.value).Take(cantidad).ToList();
    }
}

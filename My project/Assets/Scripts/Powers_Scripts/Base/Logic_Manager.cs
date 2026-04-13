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
    //Declaramos la lista en donde guardaremos los 9 poderes que se le otorgen al jugador
    //una vez empezada la partida
    public List<PoderData> mazoJugador;

    //Se declara la funcion en donde se guardaran las 3 etiquetas que mas se repiten en los 9 poderes
    public List<string> ObtenerMayoriaEtiquetas()
    {
        //Primero se juntan todas las etiquetas en una sola variable 
        var etiquetas = mazoJugador.SelectMany(p => p.etiquetas)
            .GroupBy(e => e)//Se agrupan por nombre y cuantas veces se repite esa etiqueta
            .OrderByDescending(g => g.Count())//Se orden los grupos de etiquetas de mayor a menor numero de repeticiones
            .Take(3).Select(g => g.Key)//Se toman las 3 etiquetas que mas se repiten
            .ToList();//Se convierten es una lista de texto

        return etiquetas;
    }

}

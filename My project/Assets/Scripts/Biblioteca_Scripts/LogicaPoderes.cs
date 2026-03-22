using System.Collections.Generic;

public static class LogicaPoderes
{
    // Este diccionario conecta los PROBLEMAS del nivel con las SOLUCIONES (Poderes)
    public static Dictionary<string, string[]> Relaciones = new Dictionary<string, string[]>()
    {
        // Formato: { "Etiqueta del Nivel", new string[] { "Etiqueta Poder 1", "Etiqueta Poder 2" } }
        { "Cruzar Abismo", new string[] { "Salto", "Gravedad", "Crear plataforma", "Impulsar" } },
        { "Vertical", new string[] { "Vertical", "Gravedad", "Teletransportacion" } },
        { "Horizontal", new string[] { "Horizontal", "Impulsar", "Velocidad" } },
        { "General", new string[] { "Horizontal", "Vertical", "Salto" } }
    };
}

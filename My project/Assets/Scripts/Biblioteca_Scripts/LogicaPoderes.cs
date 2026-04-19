using System.Collections.Generic;

public static class LogicaPoderes
{
    // Este diccionario conecta los PROBLEMAS del nivel con las SOLUCIONES (Poderes)
    public static Dictionary<string, string[]> Relaciones = new Dictionary<string, string[]>()
    {
        // Formato: { "Etiqueta del Nivel", new string[] { "Etiqueta Poder 1", "Etiqueta Poder 2" } }
        { "Cruzar Abismo", new string[] { "Salto", "Gravedad","New_Gravedad", "Crear_Plataforma", "Impulsar", "Crer_Obstaculos" } },
        { "Vertical", new string[] { "Vertical", "Gravedad","New_Gravedad" ,"Teletransportacion" } },
        { "Horizontal", new string[] { "Horizontal", "Impulsar", "Velocidad", "Crer_Obstaculos" } },
        { "General", new string[] { "Horizontal", "Vertical", "Salto" } }
    };
}

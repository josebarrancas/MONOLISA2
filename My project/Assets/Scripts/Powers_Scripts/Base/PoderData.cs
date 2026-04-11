using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPoder", menuName = "Poderes/Base")]
public class PoderData : ScriptableObject
{
    public string nombre;
    [TextArea] public string explicacion;
    public Sprite icono;

    [Header("Clasificación")]
    public string[] etiquetas; // Horizontal, Vertical, Gravedad, etc.

    [Header("Uso")]
    public TipoUso tipo;
    public int cantidadUsos; // Para varios{3-5} o Único
    public float cargaMedidor; // Para tipo Medidor
}

public enum TipoUso { Unico, Varios, Medidor }

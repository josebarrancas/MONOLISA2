using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public string nombreEscena;
    public string[] etiquetasNivel;
    public Sprite capturaPantalla;
}

using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Levels/LevelData")]
public class LevelData : ScriptableObject
{
    public string nombreEscena;
    public string ID;
    public string[] etiquetasNivel;
   
}

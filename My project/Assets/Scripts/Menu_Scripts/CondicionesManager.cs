using UnityEngine;

public enum TipoCondicion { Ninguna, ReglasDeLaCasa, PoderObligatorio, MonitorFallando, NoTeAcabesLosPoderes }

public class CondicionesManager : MonoBehaviour
{
    public static CondicionesManager Instance;

    [Header("Estado Actual")]
    public TipoCondicion condicionActual = TipoCondicion.Ninguna;
    public int indicePoderBloqueado = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void EvaluarCondicionesParaNivel(string nombreProximoNivel, float skillActual)
    {
        condicionActual = TipoCondicion.Ninguna;
        indicePoderBloqueado = -1;

        // MODO PRUEBA
         if (!EsNivelPermitido(nombreProximoNivel)) return;

        int probabilidad = CalcularProbabilidad(skillActual);

        if (probabilidad == 0) return;

        int tirada = Random.Range(0, 100);
        if (tirada < probabilidad)
        {
            condicionActual = (TipoCondicion)Random.Range(1, 5);
            if (condicionActual == TipoCondicion.PoderObligatorio) indicePoderBloqueado = Random.Range(0, 3);

            Debug.Log($"<color=red>¡CONDICIÓN ACTIVADA!</color> Mutación: {condicionActual}.");
        }
    }

    public bool EsNivelPermitido(string nombreNivel)
    {
        if (string.IsNullOrEmpty(nombreNivel)) return false;
        string[] partes = nombreNivel.Split('_');
        if (partes.Length >= 2 && int.TryParse(partes[1], out int numeroNivel))
        {
            if (numeroNivel >= 16 && numeroNivel <= 30) return true;
        }
        return false;
    }

    public int CalcularProbabilidad(float skill)
    {
        // MODO PRUEBA
         if (skill < 60f) return 0;

        // Todo al 100% para que siempre caiga una mutación
        if (skill >= 0f && skill < 70f) return 10; // <- Modificado aquí para abarcar desde 0
        if (skill >= 70f && skill < 80f) return 20;
        if (skill >= 80f && skill < 90f) return 30;
        if (skill >= 90f && skill < 100f) return 40;
        return 50;
    }
}
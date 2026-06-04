using UnityEngine;
using System;
using System.IO;

public class ManejadorGuardadoTexto : MonoBehaviour
{
    // PROPIEDAD INTELIGENTE: Si la ruta está vacía, se calcula sola al instante
    private static string _rutaArchivo;
    private static string rutaArchivo
    {
        get
        {
            if (string.IsNullOrEmpty(_rutaArchivo))
            {
                _rutaArchivo = Path.Combine(Application.persistentDataPath, "save_data.txt");
            }
            return _rutaArchivo;
        }
    }

    [Serializable]
    public class DatosJuego
    {
        public float partidaTiempoActual;
        public float recordMejorTiempoNormal;
        public int partidaPuntajeActual;
        public int recordMejorPuntajeNormal;
    }

    public static void GuardarDatos(float tiempoActual, float mejorTiempo, int puntajeActual, int mejorPuntaje)
    {
        DatosJuego datos = new DatosJuego
        {
            partidaTiempoActual = tiempoActual,
            recordMejorTiempoNormal = mejorTiempo,
            partidaPuntajeActual = puntajeActual,
            recordMejorPuntajeNormal = mejorPuntaje
        };

        string textoJson = JsonUtility.ToJson(datos, true);

        try
        {
            // Ahora 'rutaArchivo' siempre tendrá la dirección segura de AppData
            File.WriteAllText(rutaArchivo, textoJson);
        }
        catch (Exception e)
        {
            Debug.LogError($"[GUARDADO TXT] Error al escribir: {e.Message}");
        }
    }

    public static DatosJuego CargarDatos()
    {
        if (!File.Exists(rutaArchivo))
        {
            DatosJuego datosNuevos = new DatosJuego
            {
                partidaTiempoActual = 0f,
                recordMejorTiempoNormal = 999999f,
                partidaPuntajeActual = 0,
                recordMejorPuntajeNormal = 0
            };
            string inicial = JsonUtility.ToJson(datosNuevos, true);

            string carpeta = Path.GetDirectoryName(rutaArchivo);
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            File.WriteAllText(rutaArchivo, inicial);
            return datosNuevos;
        }

        string textoArchivo = File.ReadAllText(rutaArchivo);
        return JsonUtility.FromJson<DatosJuego>(textoArchivo);
    }
}
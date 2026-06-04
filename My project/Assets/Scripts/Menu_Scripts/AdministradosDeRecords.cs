using UnityEngine;
using System;

public class AdministradorDeRecords : MonoBehaviour
{
    /// <summary>
    /// Llama a esta función exactamente en el momento en que el jugador gana la campaña/juego.
    /// </summary>
    public void RegistrarFinDePartida()
    {
        // 1. Apagamos el motor del reloj global para que ya no sume más frames
        ControlarPausaMenu.CronometroActivo = false;

        // 2. Traemos el tiempo que hizo el jugador en esta carrera
        float tiempoFinalCarrera = PlayerPrefs.GetFloat("Partida_TiempoActual", 0f);

        // 3. Traemos el récord histórico que estaba guardado (Si es la primera vez, devolvemos un tiempo infinito 999999f)
        float mejorTiempoRegistrado = PlayerPrefs.GetFloat("Record_MejorTiempo_Normal", 999999f);

        Debug.Log($"[QA RECODS] Partida terminada. Tiempo actual: {tiempoFinalCarrera}s | Mejor tiempo previo: {mejorTiempoRegistrado}s");

        // 4. Como es un Speedrun/RTA, entre MENOR sea el tiempo, MEJOR es el récord.
        // Si el tiempo actual es menor que el récord viejo (o si es la primera partida), se actualiza.
        if (tiempoFinalCarrera < mejorTiempoRegistrado && tiempoFinalCarrera > 0.1f)
        {
            Debug.Log("<color=green>¡NUEVO RÉCORD DE TIEMPO DETECTADO!</color> Guardando en el registro...");

            // Guardamos el nuevo récord invicto
            PlayerPrefs.SetFloat("Record_MejorTiempo_Normal", tiempoFinalCarrera);
        }
        else
        {
            Debug.Log("<color=yellow>No superaste tu mejor tiempo.</color> Sigue intentando.");
        }

        // --- EL PASO CRUCIAL ---
        // Forzamos a Windows a plasmar los datos de los floats en el Regedit de inmediato.
        PlayerPrefs.Save();
    }
}
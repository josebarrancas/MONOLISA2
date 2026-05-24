using UnityEngine;
using UnityEngine.UI;

public class HUDPoderesManager : MonoBehaviour
{
    public static HUDPoderesManager Instance { get; private set; }

    [Header("Componentes Visuales")]
    public Animator animatorBarra;
    public Image espacioIconoPoder; // La ranura donde se pintará el poder actual

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Envía directamente el número entero de cargas al Animator del HUD.
    /// </summary>
    /// <param name="cargasActuales">Cuántas cargas le quedan al poder en este momento.</param>
    public void ActualizarCargasVisuales(int cargasActuales)
    {
        if (animatorBarra != null)
        {
            // Enviamos el número entero directamente al parámetro "Cargas" del Animator
            animatorBarra.SetInteger("Cargas", cargasActuales);
            Debug.Log($"HUD Actualizado - Mostrando exactamente {cargasActuales} cargas.");
        }
    }

    /// <summary>
    /// Cambia el sprite de la ranura visual al del poder en uso.
    /// </summary>
    public void CambiarIconoPoder(Sprite nuevoIcono)
    {
        if (espacioIconoPoder != null && nuevoIcono != null)
        {
            espacioIconoPoder.sprite = nuevoIcono;
        }
    }
}
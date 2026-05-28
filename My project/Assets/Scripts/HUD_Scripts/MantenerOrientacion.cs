using UnityEngine;

public class MantenerOrientacion : MonoBehaviour
{
    private Quaternion rotacionInicial;
    private Vector3 escalaInicial;
    private Canvas canvasFamiliar;

    [Header("Ajuste de Posición")]
    [Tooltip("Altura fija de la barra sobre la cabeza del jugador")]
    public float alturaSobreElJugador = 2.0f;

    [Header("QA Debug System")]
    [Tooltip("Activa para ver reportes en consola y líneas de guía en la pestaña Scene")]
    public bool activarQADebug = true;

    void Awake()
    {
        rotacionInicial = transform.rotation;
        escalaInicial = transform.localScale;
        canvasFamiliar = GetComponent<Canvas>();
    }

    void Start()
    {
        // 1. AUTO-ASIGNACIÓN DE LA CÁMARA & QA
        if (canvasFamiliar != null && canvasFamiliar.renderMode == RenderMode.WorldSpace)
        {
            if (canvasFamiliar.worldCamera == null)
            {
                canvasFamiliar.worldCamera = Camera.main;

                if (activarQADebug)
                {
                    if (canvasFamiliar.worldCamera != null)
                        Debug.Log($"<color=green><b>[QA SUCCESS]</b></color> Cámara asignada automáticamente al Canvas en escena: {gameObject.scene.name}");
                    else
                        Debug.LogError($"<color=red><b>[QA CRITICAL]</b></color> No se encontró Camera.main con el Tag 'MainCamera' en la escena: {gameObject.scene.name}. El HUD no se renderizará.");
                }
            }
        }

        // 2. CORRECCIÓN DE DESFASE EN LA NUEVA ESCENA & QA
        if (transform.parent != null)
        {
            transform.localPosition = new Vector3(0, alturaSobreElJugador, 0);

            if (activarQADebug)
            {
                Debug.Log($"<color=cyan><b>[QA INFO]</b></color> Posición local del HUD reseteada sobre el objeto padre: {transform.parent.name} en {transform.localPosition}");
            }
        }
        else if (activarQADebug)
        {
            Debug.LogWarning($"<color=yellow><b>[QA WARNING]</b></color> El HUD '{gameObject.name}' no tiene un objeto padre asignado en la jerarquía. Podría quedarse flotando en la nada.");
        }
    }

    void LateUpdate()
    {
        // Mantiene la rotación global fija
        transform.rotation = rotacionInicial;

        // Tu lógica matemática para evitar que se invierta el HUD
        if (transform.parent != null)
        {
            Vector3 escalaPadre = transform.parent.localScale;
            transform.localScale = new Vector3(
                escalaInicial.x / Mathf.Sign(escalaPadre.x),
                escalaInicial.y,
                escalaInicial.z
            );

            // ==========================================================
            // DEBUG VISUAL EN LA PESTAÑA 'SCENE'
            // ==========================================================
            if (activarQADebug)
            {
                // Dibuja una línea verde desde el centro del jugador hasta la barra del HUD
                Debug.DrawLine(transform.parent.position, transform.position, Color.green);

                // Dibuja una cruz roja sobre la barra para ubicarla rápido visualmente si se aleja
                Debug.DrawRay(transform.position, Vector3.up * 0.5f, Color.red);
                Debug.DrawRay(transform.position, Vector3.right * 0.5f, Color.red);
            }
        }
    }
}
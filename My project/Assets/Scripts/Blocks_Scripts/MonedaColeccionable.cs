using UnityEngine;
using System.Collections.Generic;

public class MonedaColeccionable : MonoBehaviour
{
    [Header("Estado")]
    public bool recolectada = false;

    // --- NUEVO: HUELLAS DACTILARES PARA EL RESETEO LOCAL ---
    [HideInInspector] public Vector2 posicionInicial;
    public static List<Vector2> monedasCaidasAlVacio = new List<Vector2>();

    // Contadores estáticos
    public static int monedasTotalesNivel = 0;
    public static int monedasRecogidasNivel = 0;

    private SpriteRenderer sprite;
    private bool seEstaCerrandoEscena = false;

    private static int ultimoFrameDeReinicio = -1;

    void Awake()
    {
        // Guardamos la huella de posición inmutable en cuanto nace la moneda
        posicionInicial = transform.position;

        if (Time.frameCount != ultimoFrameDeReinicio)
        {
            monedasTotalesNivel = 0;
            monedasRecogidasNivel = 0;
            monedasCaidasAlVacio.Clear(); // Limpiamos el registro de caídas al cambiar de nivel
            ultimoFrameDeReinicio = Time.frameCount;
        }
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        monedasTotalesNivel++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // REGLA 1: Toque directo del jugador
        if (collision.gameObject.CompareTag("Player") && !recolectada)
        {
            TomarMoneda();
        }
        // REGLA 2: Cae en una zona de muerte (si la zona es un colisionador sólido)
        else if (collision.gameObject.CompareTag("Dead"))
        {
            Debug.Log("<color=orange>Moneda tocó el vacío (Collision).</color> Destruyendo para evitar bloqueo.");
            if (!recolectada) monedasCaidasAlVacio.Add(posicionInicial); // Registramos la huella antes de destruir
            Destroy(gameObject);
        }
    }

    // --- DETECCIÓN PARA BARRERAS INVISIBLES (TRIGGERS) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // REGLA 3: Cae en una zona de muerte (si la barrera es un Trigger)
        if (collision.CompareTag("Dead"))
        {
            Debug.Log("<color=orange>Moneda tocó el vacío (Trigger).</color> Destruyendo para evitar bloqueo.");
            if (!recolectada) monedasCaidasAlVacio.Add(posicionInicial); // Registramos la huella antes de destruir
            Destroy(gameObject);
        }
    }

    // --- REGLA DE SINGULARIDAD E INACCESIBILIDAD ---
    private void OnDestroy()
    {
        if (!recolectada && !seEstaCerrandoEscena && Application.isPlaying)
        {
            monedasRecogidasNivel++;
            Debug.Log($"Moneda eliminada/procesada por el vacío o singularidad. Progreso: {monedasRecogidasNivel}/{monedasTotalesNivel}");
        }
    }

    // CORRECCIÓN CRÍTICA: Ahora es 'public' para que ReseteoLocalPower pueda llamarlo libremente
    public void TomarMoneda()
    {
        recolectada = true;
        monedasRecogidasNivel++;

        if (sprite != null)
        {
            sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        }

        Debug.Log($"Moneda tocada. Progreso: {monedasRecogidasNivel}/{monedasTotalesNivel}");
    }

    private void OnApplicationQuit() { seEstaCerrandoEscena = true; }

    public static bool TodasLasMonedasRecogidas()
    {
        return monedasRecogidasNivel >= monedasTotalesNivel;
    }
}
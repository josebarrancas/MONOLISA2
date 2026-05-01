using UnityEngine;

public class MonedaColeccionable : MonoBehaviour
{
    [Header("Estado")]
    public bool recolectada = false;

    // Contadores estáticos
    public static int monedasTotalesNivel = 0;
    public static int monedasRecogidasNivel = 0;

    private SpriteRenderer sprite;
    private bool seEstaCerrandoEscena = false;

    private static int ultimoFrameDeReinicio = -1;

    void Awake()
    {
        if (Time.frameCount != ultimoFrameDeReinicio)
        {
            monedasTotalesNivel = 0;
            monedasRecogidasNivel = 0;
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
            Destroy(gameObject);
        }
    }

    // --- NUEVO: Detección para barreras invisibles (Triggers) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // REGLA 3: Cae en una zona de muerte (si la barrera es un Trigger)
        if (collision.CompareTag("Dead"))
        {
            Debug.Log("<color=orange>Moneda tocó el vacío (Trigger).</color> Destruyendo para evitar bloqueo.");
            Destroy(gameObject); // Esto invocará tu OnDestroy automáticamente
        }
    }

    // --- REGLA DE SINGULARIDAD E INACCESIBILIDAD ---
    private void OnDestroy()
    {
        // Esta genialidad de código que usted escribió se encargará de sumarla
        if (!recolectada && !seEstaCerrandoEscena && Application.isPlaying)
        {
            monedasRecogidasNivel++;
            Debug.Log($"Moneda eliminada/procesada por el vacío o singularidad. Progreso: {monedasRecogidasNivel}/{monedasTotalesNivel}");
        }
    }

    private void TomarMoneda()
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
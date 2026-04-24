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
        // Borramos la memoria si acabamos de cargar una escena nueva
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
        // REGLA: Toque directo del jugador
        if (collision.gameObject.CompareTag("Player") && !recolectada)
        {
            TomarMoneda();
        }
    }

    // --- REGLA DE SINGULARIDAD E INACCESIBILIDAD ---
    private void OnDestroy()
    {
        if (!recolectada && !seEstaCerrandoEscena && Application.isPlaying)
        {
            monedasRecogidasNivel++;
            Debug.Log($"Moneda eliminada/procesada. Progreso: {monedasRecogidasNivel}/{monedasTotalesNivel}");
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
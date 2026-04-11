using UnityEngine;

public class Bloque_Fragil : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Configuración de Tiempo")]
    public float tiempoParaRomper = 2.0f;
    public float intensidadVibracion = 0.1f;
    public float gravedad = 3f;

    private float cronometro = 0;
    private bool seActivo = false;
    private Vector3 posicionOriginal;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        posicionOriginal = transform.position;
    }

    void Update()
    {
        if (seActivo)
        {
            cronometro += Time.deltaTime;

            // --- 1. LÓGICA DE ANIMACIÓN ---
            // Calculamos el progreso de 0 a 3.
            float progreso = (cronometro / tiempoParaRomper) * 1f;
            anim.SetFloat("status", progreso);

            // --- 2. LÓGICA DE VIBRACIÓN (FASE FINAL) ---
            // Cambiamos 0.5f por 0.75f para que solo vibre al final (última fase)
            if (cronometro > tiempoParaRomper * 0.25f && cronometro < tiempoParaRomper)
            {
                float offsetX = Random.Range(-intensidadVibracion, intensidadVibracion);
                float offsetY = Random.Range(-intensidadVibracion, intensidadVibracion);
                transform.position = posicionOriginal + new Vector3(offsetX, offsetY, 0);
            }

            // --- 3. LÓGICA DE CAÍDA ---
            if (cronometro >= tiempoParaRomper)
            {
                Caer();
            }
        }
    }

    private void Caer()
    {
        seActivo = false;
        transform.position = posicionOriginal;

        // IMPORTANTE: Cambiamos a Dynamic e inmediatamente aplicamos la gravedad
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravedad;

        // Esto fuerza a la física a despertar y caer de inmediato
        rb.WakeUp();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("¡EL BLOQUE CAE!");
        GetComponent<Collider2D>().enabled = true;

        // Destrucción opcional para no llenar la memoria de bloques caídos
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!seActivo)
        {
            seActivo = true;
        }
    }
}

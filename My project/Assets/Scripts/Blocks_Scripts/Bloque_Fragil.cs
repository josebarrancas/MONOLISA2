using System.Collections;
using UnityEngine;

public class Bloque_Fragil : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Configuración de Tiempo")]
    public float tiempoParaRomper = 2.0f;
    public float intensidadVibracion = 0.1f;
    public float gravedad = 3f;
    [Tooltip("Cuánto tiempo dura cayendo antes de volverse flotante")]
    public float tiempoCayendo = 1.0f;

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
            float progreso = (cronometro / tiempoParaRomper) * 1f;
            anim.SetFloat("status", progreso);

            // --- 2. LÓGICA DE VIBRACIÓN (FASE FINAL) ---
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

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravedad;

        rb.WakeUp();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("¡EL BLOQUE CAE!");
        GetComponent<Collider2D>().enabled = true;

        // Quitamos el Destroy(gameObject, 10f) para que no se borre el bloque,
        // y en su lugar iniciamos la espera para activar el modo flotante.
        StartCoroutine(EsperarParaFlotar());
    }

    private IEnumerator EsperarParaFlotar()
    {
        // Espera el tiempo configurado mientras el bloque va cayendo por gravedad
        yield return new WaitForSeconds(tiempoCayendo);

        // Buscamos el segundo script en este mismo bloque
        Bloque_Fragil_A_Flotante scriptFlotante = GetComponent<Bloque_Fragil_A_Flotante>();
        if (scriptFlotante != null)
        {
            scriptFlotante.enabled = true; // Activa el comportamiento flotante
        }

        this.enabled = false; // Apaga este script original para que no vuelva a calcular el Update
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Modificación sutil: solo se activa si el script flotante no ha tomado el control
        if (!seActivo && (GetComponent<Bloque_Fragil_A_Flotante>() == null || !GetComponent<Bloque_Fragil_A_Flotante>().enabled))
        {
            seActivo = true;
        }
    }
}
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
    [Tooltip("Cuánto tiempo dura cayendo antes de volverse desenganche")]
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

            float progreso = (cronometro / tiempoParaRomper) * 1f;
            anim.SetFloat("status", progreso);

            if (cronometro > tiempoParaRomper * 0.25f && cronometro < tiempoParaRomper)
            {
                float offsetX = Random.Range(-intensidadVibracion, intensidadVibracion);
                float offsetY = Random.Range(-intensidadVibracion, intensidadVibracion);
                transform.position = posicionOriginal + new Vector3(offsetX, offsetY, 0);
            }

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

        StartCoroutine(EsperarParaFlotar());
    }

    private IEnumerator EsperarParaFlotar()
    {
        yield return new WaitForSeconds(tiempoCayendo);

        Bloque_Fragil_A_Desenganche scriptFlotante = GetComponent<Bloque_Fragil_A_Desenganche>();
        if (scriptFlotante != null)
        {
            scriptFlotante.enabled = true;
        }

        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!seActivo && (GetComponent<Bloque_Fragil_A_Desenganche>() == null || !GetComponent<Bloque_Fragil_A_Desenganche>().enabled))
        {
            seActivo = true;
        }
    }
}
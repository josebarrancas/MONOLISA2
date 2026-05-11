using UnityEngine;
using System.Collections;

public class BrujulaGravitacionalPower : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadElevacion = 12f;
    public int cargasRestantes = 1;
    public GameObject flechaVisual;
    public bool esUnico = false;
    [Header("Configuración Visual")]
    public float distanciaFlecha = 1.5f; 

    [HideInInspector] public bool estaUsandoBrujula = false;

    private Rigidbody2D rb;
    private Vector3 posicionEntrada;
    private Vector2 direccionGravedadElegida;
    private PowerSelector selectorUI;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionEntrada = transform.position;
        selectorUI = FindObjectOfType<PowerSelector>();
        if (flechaVisual != null) flechaVisual.SetActive(false);
    }

    private void Update()
    {
        if (selectorUI == null || Time.timeScale == 0) return;
        string poderActivo = selectorUI.ObtenerNombrePoderActual();

        if (poderActivo == "Brujula gravitacional" || poderActivo == "Brujula Gravitacional")
        {
            if (flechaVisual != null) flechaVisual.SetActive(true);
            ActualizarDireccionFlecha();
        }
        else if (!estaUsandoBrujula)
        {
            if (flechaVisual != null) flechaVisual.SetActive(false);
        }
    }

    private void ActualizarDireccionFlecha()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Determinamos la dirección elegida
        if (Mathf.Abs(v) > 0) direccionGravedadElegida = new Vector2(0, Mathf.Sign(v));
        else if (Mathf.Abs(h) > 0) direccionGravedadElegida = new Vector2(Mathf.Sign(h), 0);
        else direccionGravedadElegida = new Vector2(Mathf.Sign(transform.localScale.x), 0);

        if (flechaVisual != null)
        {
            // --- SOLUCIÓN: Usar "position" y "rotation" GLOBALES ---

            // 1. Posición absoluta en el mundo (ignorando el espejo)
            flechaVisual.transform.position = transform.position + (Vector3)direccionGravedadElegida * distanciaFlecha;

            // 2. Rotación absoluta en el mundo
            float angulo = Mathf.Atan2(direccionGravedadElegida.y, direccionGravedadElegida.x) * Mathf.Rad2Deg;
            flechaVisual.transform.rotation = Quaternion.Euler(0, 0, angulo); // Si ocupó ponerle -90f antes, póngaselo aquí también

            // 3. Seguro anti-deformación: "Menos por Menos = Más"
            // Leemos el tamaño que usted le puso a la flecha en Unity y lo multiplicamos por el espejo del jugador.
            float grosorX = Mathf.Abs(flechaVisual.transform.localScale.x);
            float grosorY = Mathf.Abs(flechaVisual.transform.localScale.y);
            float espejoX = Mathf.Sign(transform.localScale.x);

            flechaVisual.transform.localScale = new Vector3(grosorX * espejoX, grosorY, 1);
        }
    }

    public void EjecutarBrujula()
    {
        if (cargasRestantes > 0 && !estaUsandoBrujula)
        {
            cargasRestantes--;
            StartCoroutine(RutinaBrujula());
        }
    }

    private IEnumerator RutinaBrujula()
    {
        estaUsandoBrujula = true;
        Vector2 dirElevacion = -direccionGravedadElegida;

        rb.gravityScale = 0f;
        GetComponent<Collider2D>().isTrigger = true;

        // Aplicamos la nueva gravedad global
        Physics2D.gravity = direccionGravedadElegida * 9.81f;

        while (estaUsandoBrujula)
        {
            transform.Translate(dirElevacion * velocidadElevacion * Time.deltaTime, Space.World);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!estaUsandoBrujula) return;

        if (collision.CompareTag("enganche"))
        {
            // Teletransporte al otro lado
            Vector2 tamañoBloque = collision.bounds.size;
            Vector2 dirElevacion = -direccionGravedadElegida;
            float grosor = (dirElevacion.x != 0) ? tamañoBloque.x : tamañoBloque.y;
            transform.position = transform.position + (Vector3)(dirElevacion * (grosor + 1.5f));

            FinalizarPoder(false);
        }
        else if (collision.CompareTag("Dead"))
        {
            transform.position = posicionEntrada;
            FinalizarPoder(true);
        }
    }

    private void FinalizarPoder(bool resetTotal)
    {
        estaUsandoBrujula = false;
        GetComponent<Collider2D>().isTrigger = false;
        rb.gravityScale = 5f; // Valor estándar de su juego
        rb.linearVelocity = Vector2.zero;

        if (resetTotal)
        {
            Physics2D.gravity = new Vector2(0, -9.81f);
            transform.rotation = Quaternion.identity;
        }
        else
        {
            // --- ROTACIÓN DINÁMICA ---
            // Rotamos al jugador para que sus pies (el vector negativo de transform.up)
            // coincidan con la dirección de la gravedad.
            float anguloRotacion = Mathf.Atan2(direccionGravedadElegida.y, direccionGravedadElegida.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(0, 0, anguloRotacion);
        }
    }

    public void ResetearCargas() { cargasRestantes = 1; }
}
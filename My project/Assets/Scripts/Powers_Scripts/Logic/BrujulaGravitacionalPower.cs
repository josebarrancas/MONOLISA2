using UnityEngine;

public class BrujulaGravitacionalPower : MonoBehaviour
{
    [Header("Configuración")]
    public int cargasRestantes = 1;
    public GameObject flechaVisual;
    public bool esUnico = false; // Reintegrado para CentinelaDePoderes

    [Header("Configuración Visual")]
    public float distanciaFlecha = 1.5f;

    [HideInInspector] public bool estaUsandoBrujula = false; // Reintegrado para CentinelaDePoderes

    private Rigidbody2D rb;
    private Vector3 posicionPuertaInicial;
    private Vector2 direccionGravedadElegida;
    private PowerSelectorPrincipal selectorUI;

    // Constante que define la gravedad normal de inicio (hacia abajo)
    private readonly Vector2 GRAVEDAD_NORMAL = new Vector2(0, -9.81f);

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        selectorUI = FindObjectOfType<PowerSelectorPrincipal>();

        posicionPuertaInicial = transform.position;

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
        else
        {
            if (flechaVisual != null) flechaVisual.SetActive(false);
        }
    }

    private void ActualizarDireccionFlecha()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(v) > 0) direccionGravedadElegida = new Vector2(0, Mathf.Sign(v));
        else if (Mathf.Abs(h) > 0) direccionGravedadElegida = new Vector2(Mathf.Sign(h), 0);
        else direccionGravedadElegida = new Vector2(Mathf.Sign(transform.localScale.x), 0);

        if (flechaVisual != null)
        {
            flechaVisual.transform.position = transform.position + (Vector3)direccionGravedadElegida * distanciaFlecha;
            float angulo = Mathf.Atan2(direccionGravedadElegida.y, direccionGravedadElegida.x) * Mathf.Rad2Deg;
            flechaVisual.transform.rotation = Quaternion.Euler(0, 0, angulo);

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
            estaUsandoBrujula = true; // Notificamos al Centinela que el poder está activo

            Physics2D.gravity = direccionGravedadElegida * 9.81f;

            float anguloRotacion = Mathf.Atan2(direccionGravedadElegida.y, direccionGravedadElegida.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(0, 0, anguloRotacion);

            Debug.Log($"[Brújula] Gravedad alterada hacia: {direccionGravedadElegida}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            if (Physics2D.gravity != GRAVEDAD_NORMAL)
            {
                transform.position = posicionPuertaInicial;
                rb.linearVelocity = Vector2.zero;
                Physics2D.gravity = GRAVEDAD_NORMAL;
                transform.rotation = Quaternion.identity;

                estaUsandoBrujula = false; // Apagamos el estado para el Centinela
                Debug.Log("[Brújula] Salvado por la gravedad alterada. Retorno a la puerta exitoso.");
            }
        }
    }
}
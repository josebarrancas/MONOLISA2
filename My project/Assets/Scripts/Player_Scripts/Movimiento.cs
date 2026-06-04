using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimiento : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    private float tiempoCoyote;
    private float margenCoyote = 0.15f;
    private Animator Animator;
    private Rigidbody2D rb;
    private float Horizontal;
    private bool Grounded;
    private EmbestifresaPower embestifresa;
    [HideInInspector] public bool enZonaDesplazador = false;
    private bool solicitarSalto = false;

    private Vector3 posicionInicialNivel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        embestifresa = GetComponent<EmbestifresaPower>();
        Physics2D.gravity = new Vector2(0, -9.81f);

        posicionInicialNivel = transform.position;

        CargarCanvasFondoAutomatico();
    }

    private void CargarCanvasFondoAutomatico()
    {
        if (GameObject.Find("Canvas_Fondo(Clone)") == null && GameObject.Find("Canvas_Fondo") == null)
        {
            GameObject prefabFondo = Resources.Load<GameObject>("Canvas_Fondo");

            if (prefabFondo != null)
            {
                GameObject fondoInstanciado = Instantiate(prefabFondo);
                fondoInstanciado.transform.position = Vector3.zero;

                Camera camaraActual = Camera.main;

                if (camaraActual == null)
                {
                    camaraActual = FindObjectOfType<Camera>();
                }

                if (camaraActual != null)
                {
                    Canvas canvasComponent = fondoInstanciado.GetComponent<Canvas>();
                    if (canvasComponent != null)
                    {
                        canvasComponent.worldCamera = camaraActual;
                        canvasComponent.sortingOrder = -100;
                    }
                }
                else
                {
                    Debug.LogError("Fondo Creado: ¡No se encontró ninguna Cámara en esta escena para acoplar el fondo!");
                }
            }
            else
            {
                Debug.LogWarning("Movimiento Player: No se encontró el Prefab 'Canvas_Fondo' en la carpeta Assets/Resources.");
            }
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        Horizontal = Input.GetAxisRaw("Horizontal");
        Animator.SetBool("running", Horizontal != 0.0f);

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-0.8f, Mathf.Abs(transform.localScale.y), 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(0.8f, Mathf.Abs(transform.localScale.y), 1.0f);

        // Detección de suelo
        Grounded = false;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, 2.3f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && !hit.collider.CompareTag("Player") && !hit.collider.isTrigger)
            {
                Grounded = true;
                break;
            }
        }

        if (Grounded) tiempoCoyote = margenCoyote;
        else tiempoCoyote -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z) && tiempoCoyote > 0f && !enZonaDesplazador)
        {
            solicitarSalto = true;
            tiempoCoyote = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (enZonaDesplazador) return;
        if (embestifresa == null || !embestifresa.estaEmbistiendo)
        {
            Vector2 direccionMovimiento = transform.right * Horizontal * Speed;
            Vector2 velocidadGravedad = Vector2.Dot(rb.linearVelocity, transform.up) * (Vector2)transform.up;

            if (solicitarSalto)
            {
                float velocidadCalculada = rb.mass > 0 ? (JumpForce / rb.mass) : JumpForce;
                velocidadGravedad = transform.up * velocidadCalculada;
                solicitarSalto = false;
            }
            rb.linearVelocity = direccionMovimiento + velocidadGravedad;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            // Candado de la Brújula Gravitacional
            if (Physics2D.gravity != new Vector2(0, -9.81f))
            {
                return;
            }

            string escenaActual = SceneManager.GetActiveScene().name;

            if (escenaActual == "Tutorial" || escenaActual == "Lvl_Based_Tutorial")
            {
                SceneManager.LoadScene(escenaActual);
            }
            else
            {
                if (SkillManager.Instance != null)
                {
                    SkillManager.Instance.intentosNivel++;

                    // --- REINICIO AUTOMÁTICO POR EXCESO DE MUERTES (>10) ---
                    if (SkillManager.Instance.intentosNivel > 10)
                    {
                        Debug.Log("<color=red>[GAME OVER]</color> El jugador murió más de 10 veces. Ejecutando Hard Reset...");

                        // 1. Limpiar el archivo .txt para que no se hereden tiempos ni scores viejos
                        var datos = ManejadorGuardadoTexto.CargarDatos();
                        ManejadorGuardadoTexto.GuardarDatos(
                            0f,
                            datos.recordMejorTiempoNormal,
                            0,
                            datos.recordMejorPuntajeNormal
                        );

                        // 2. Reiniciar los índices del LevelLoader si existe en escena antes de destruirlo
                        if (LevelLoader.Instance != null)
                        {
                            LevelLoader.Instance.nivelGlobal = 0;
                            LevelLoader.Instance.nivelActualIndice = 0;
                        }

                        // 3. DETONACIÓN DE MEMORIA RAM (Eliminamos todas las instancias persistentes acumuladas)
                        if (SkillManager.Instance != null) Destroy(SkillManager.Instance.gameObject);
                        if (LevelLoader.Instance != null) Destroy(LevelLoader.Instance.gameObject);

                        var logicManager = FindObjectOfType<Logic_Manager>();
                        if (logicManager != null) Destroy(logicManager.gameObject);

                        var condicionesManager = FindObjectOfType<CondicionesManager>();
                        if (condicionesManager != null) Destroy(condicionesManager.gameObject);

                        var reRollManager = FindObjectOfType<ReRollManager>();
                        if (reRollManager != null) Destroy(reRollManager.gameObject);

                        // 4. Carga nativa del Tutorial desde cero
                        SceneManager.LoadScene("Lvl_Based_Tutorial");
                        return;
                    }
                }

                // Si no ha superado las 10 muertes, sigue el flujo normal a la selección
                SceneManager.LoadScene("Pantalla_Seleccion");
            }
        }
    }
}
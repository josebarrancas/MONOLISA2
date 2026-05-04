using UnityEngine;

public class Controlador_Poderes_Principal : MonoBehaviour
{
    private PowerSelectorPrincipal miHUD;
    [HideInInspector] public bool bloqueadoPorDesplazador = false;

    [Header("Barra de Combustible (Seguimiento)")]
    public BarraSeguimiento barraSeguimiento; // Arrastra aquí el objeto con el script BarraSeguimiento

    void Start()
    {
        miHUD = FindObjectOfType<PowerSelectorPrincipal>();

        if (miHUD == null)
        {
            Debug.LogError("¡ALERTA!: No se encontró PowerSelectorPrincipal en esta escena.");
        }
    }

    void Update()
    {
        if (Time.timeScale == 0 || miHUD == null) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            EjecutarPoderDeToque();
        }

        ProcesarPoderesContinuos();
    }

    private void EjecutarPoderDeToque()
    {
        string rawNombre = miHUD.ObtenerNombrePoderActual();
        if (string.IsNullOrEmpty(rawNombre)) return;

        string nombre = rawNombre.Trim().ToLower();

        // -- PODERES DE TOQUE (DASH, PLATAFORMAS, ETC) --
        if (nombre == "embestifresa")
        {
            if (bloqueadoPorDesplazador) return;
            EmbestifresaPower script = GetComponent<EmbestifresaPower>();
            if (script != null)
            {
                float inputX = Input.GetAxisRaw("Horizontal");
                float inputY = Input.GetAxisRaw("Vertical");
                if (inputX == 0 && inputY == 0) inputX = transform.localScale.x > 0 ? 1 : -1;

                Vector2 direccionDash = new Vector2(inputX, inputY).normalized;
                script.EjecutarImpulso(direccionDash);
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "plataforma estatica" || nombre == "plataforma estática")
        {
            PlataformaEstaticaPower script = GetComponent<PlataformaEstaticaPower>();
            if (script != null && script.cargasRestantes > 0)
            {
                script.EjecutarPlataforma();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "coca agitada")
        {
            if (bloqueadoPorDesplazador) return;
            CocaAgitadaPower script = GetComponent<CocaAgitadaPower>();
            if (script != null)
            {
                script.EjecutarElevacion();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "reseteo local")
        {
            ReseteoLocalPower script = GetComponent<ReseteoLocalPower>();
            if (script != null)
            {
                script.EjecutarReseteo();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "brujula gravitacional" || nombre == "brújula gravitacional")
        {
            BrujulaGravitacionalPower script = GetComponent<BrujulaGravitacionalPower>();
            if (script != null)
            {
                script.EjecutarBrujula();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "cubo repulsor")
        {
            CuboRepulsorPower script = GetComponent<CuboRepulsorPower>();
            if (script != null)
            {
                script.EjecutarPoder();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "gel adherente")
        {
            GelAdherentePower script = GetComponent<GelAdherentePower>();
            if (script != null)
            {
                script.EjecutarDisparo();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "paso sombra")
        {
            PasoSombraPower script = GetComponent<PasoSombraPower>();
            if (script != null)
            {
                if (script.EjecutarPasoSombra()) RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "singularidad")
        {
            SingularidadPower script = GetComponent<SingularidadPower>();
            if (script != null)
            {
                script.EjecutarSingularidad();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "error de codigo" || nombre == "error de código")
        {
            ErrorDeCodigoPower script = GetComponent<ErrorDeCodigoPower>();
            if (script != null)
            {
                script.EjecutarIntercambio();
                RegistrarEventoPoder(rawNombre);
            }
        }
        else if (nombre == "iman" || nombre == "imán" || nombre == "desplazador")
        {
            if (nombre.Contains("desplazador"))
            {
                GetComponent<DesplazadorPower>()?.EjecutarPoder();
                RegistrarEventoPoder(rawNombre);
            }
            if (nombre.Contains("iman"))
            {
                GetComponent<ImanPower>()?.EjecutarIman();
            }
        }
    }

    private void RegistrarEventoPoder(string nombreOriginal)
    {
        miHUD.RegistrarUsoDePoder();
        if (SkillManager.Instance != null)
        {
            SkillManager.Instance.RegistraUsoPoder(nombreOriginal);
        }
    }

    private void ProcesarPoderesContinuos()
    {
        string rawNombre = miHUD.ObtenerNombrePoderActual();
        if (string.IsNullOrEmpty(rawNombre)) return;

        string nombre = rawNombre.Trim().ToLower();
        bool mostrarBarra = false;

        // G-INVERSOR
        GInversorPower scriptGravedad = GetComponent<GInversorPower>();
        if (scriptGravedad != null)
        {
            bool quiereInvertir = (nombre == "g-inversor") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptGravedad.ProcesarInversion(quiereInvertir);

            if (nombre == "g-inversor" && barraSeguimiento != null)
            {
                // Conectado a nivelMedidor y medidorMaximo de GInversorPower
                barraSeguimiento.ActualizarEstado(scriptGravedad.nivelMedidor, scriptGravedad.medidorMaximo, true);
                mostrarBarra = true;
            }
        }

        // MOCHILA DE COCAS
        MochilaCocasPower scriptMochila = GetComponent<MochilaCocasPower>();
        if (scriptMochila != null)
        {
            bool usandoMochila = (nombre == "mochila de cocas") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptMochila.ProcesarVuelo(usandoMochila);

            if (nombre == "mochila de cocas" && barraSeguimiento != null)
            {
                // Conectado a nivelMedidor y medidorMaximo de MochilaCocasPower
                barraSeguimiento.ActualizarEstado(scriptMochila.nivelMedidor, scriptMochila.medidorMaximo, true);
                mostrarBarra = true;
            }
        }

        // SINGULARIDAD
        SingularidadPower scriptSingularidad = GetComponent<SingularidadPower>();
        if (scriptSingularidad != null)
        {
            scriptSingularidad.estaSeleccionado = (nombre == "singularidad");
        }

        // Si el poder actual no es uno de los continuos o no hay HUD, ocultamos la barra
        if (!mostrarBarra && barraSeguimiento != null)
        {
            barraSeguimiento.ActualizarEstado(0, 0, false);
        }
    }
}
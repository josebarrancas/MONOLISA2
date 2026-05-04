using UnityEngine;

public class Controlador_Poderes : MonoBehaviour
{
    private PowerSelector selectorUI;
    public bool bloqueadoPorDesplazador = false;

    [Header("Barra de Combustible (Seguimiento)")]
    public BarraSeguimiento barraSeguimiento; // Arrastra la barra aquí en el Inspector

    void Start()
    {
        selectorUI = FindObjectOfType<PowerSelector>();
        if (selectorUI == null)
        {
            Debug.LogError("¡ALERTA INICIAL!: No se encontró el HUD (PowerSelector) al iniciar el juego.");
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            EjecutarPoderDeToque();
        }

        ProcesarPoderesContinuos();
    }

    private void EjecutarPoderDeToque()
    {
        if (selectorUI == null) return;

        string nombrePoder = selectorUI.ObtenerNombrePoderActual();

        // -- EMBESTIFRESA --
        if (nombrePoder == "Embestifresa")
        {
            if (bloqueadoPorDesplazador) return;
            EmbestifresaPower scriptImpulso = GetComponent<EmbestifresaPower>();
            if (scriptImpulso != null)
            {
                float inputX = Input.GetAxisRaw("Horizontal");
                float inputY = Input.GetAxisRaw("Vertical");
                if (inputX == 0 && inputY == 0) inputX = transform.localScale.x > 0 ? 1 : -1;

                Vector2 direccionDash = new Vector2(inputX, inputY).normalized;
                scriptImpulso.EjecutarImpulso(direccionDash);
                RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- PLATAFORMA ESTÁTICA --
        else if (nombrePoder == "Plataforma Estatica")
        {
            PlataformaEstaticaPower scriptPlataforma = GetComponent<PlataformaEstaticaPower>();
            if (scriptPlataforma != null && scriptPlataforma.cargasRestantes > 0)
            {
                scriptPlataforma.EjecutarPlataforma();
                RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- COCA AGITADA --
        else if (nombrePoder == "Coca Agitada")
        {
            if (bloqueadoPorDesplazador) return;
            CocaAgitadaPower scriptCocas = GetComponent<CocaAgitadaPower>();
            if (scriptCocas != null)
            {
                scriptCocas.EjecutarElevacion();
                RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- RESETEO LOCAL --
        else if (nombrePoder == "Reseteo local")
        {
            ReseteoLocalPower scriptReseteo = GetComponent<ReseteoLocalPower>();
            if (scriptReseteo != null)
            {
                scriptReseteo.EjecutarReseteo();
                RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- BRÚJULA GRAVITACIONAL --
        else if (nombrePoder == "Brujula Gravitacional")
        {
            BrujulaGravitacionalPower scriptBrujula = GetComponent<BrujulaGravitacionalPower>();
            if (scriptBrujula != null)
            {
                scriptBrujula.EjecutarBrujula();
                RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- CUBO REPULSOR --
        else if (nombrePoder == "Cubo Repulsor")
        {
            CuboRepulsorPower scriptCubo = GetComponent<CuboRepulsorPower>();
            if (scriptCubo != null)
            {
                scriptCubo.EjecutarPoder();
                RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- GEL ADHERENTE --
        else if (nombrePoder == "Gel Adherente")
        {
            GelAdherentePower scriptGel = GetComponent<GelAdherentePower>();
            if (scriptGel != null)
            {
                scriptGel.EjecutarDisparo();
                RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- PASO SOMBRA --
        else if (nombrePoder == "Paso Sombra")
        {
            PasoSombraPower scriptSombra = GetComponent<PasoSombraPower>();
            if (scriptSombra != null)
            {
                if (scriptSombra.EjecutarPasoSombra()) RegistrarEventoPoder(nombrePoder);
            }
        }
        // -- DESPLAZADOR, IMÁN, SINGULARIDAD --
        else if (nombrePoder == "Desplazador" || nombrePoder == "Iman" || nombrePoder == "Singularidad")
        {
            if (nombrePoder == "Desplazador") GetComponent<DesplazadorPower>()?.EjecutarPoder();
            if (nombrePoder == "Iman") GetComponent<ImanPower>()?.EjecutarIman();
            if (nombrePoder == "Singularidad") GetComponent<SingularidadPower>()?.EjecutarSingularidad();

            RegistrarEventoPoder(nombrePoder);
        }
        // -- ERROR DE CODIGO --
        else if (nombrePoder == "Error de codigo")
        {
            ErrorDeCodigoPower scriptError = GetComponent<ErrorDeCodigoPower>();
            if (scriptError != null)
            {
                scriptError.EjecutarIntercambio();
                selectorUI.RegistrarUsoDePoder();
            }
        }
    }

    private void RegistrarEventoPoder(string nombre)
    {
        selectorUI.RegistrarUsoDePoder();
        if (SkillManager.Instance != null)
        {
            SkillManager.Instance.RegistraUsoPoder(nombre);
        }
    }

    private void ProcesarPoderesContinuos()
    {
        if (selectorUI == null) return;
        string nombrePoder = selectorUI.ObtenerNombrePoderActual();
        bool mostrarBarra = false;

        // G-INVERSOR
        GInversorPower scriptGravedad = GetComponent<GInversorPower>();
        if (scriptGravedad != null)
        {
            bool quiereInvertir = (nombrePoder == "G-Inversor") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptGravedad.ProcesarInversion(quiereInvertir);

            if (nombrePoder == "G-Inversor" && barraSeguimiento != null)
            {
                barraSeguimiento.ActualizarEstado(scriptGravedad.nivelMedidor, scriptGravedad.medidorMaximo, true);
                mostrarBarra = true;
            }
        }

        // MOCHILA DE COCAS
        MochilaCocasPower scriptMochila = GetComponent<MochilaCocasPower>();
        if (scriptMochila != null)
        {
            bool usandoMochila = (nombrePoder == "Mochila de Cocas") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptMochila.ProcesarVuelo(usandoMochila);

            if (nombrePoder == "Mochila de Cocas" && barraSeguimiento != null)
            {
                barraSeguimiento.ActualizarEstado(scriptMochila.nivelMedidor, scriptMochila.medidorMaximo, true);
                mostrarBarra = true;
            }
        }

        // Apagar la barra si no se está usando un poder continuo
        if (!mostrarBarra && barraSeguimiento != null)
        {
            barraSeguimiento.ActualizarEstado(0, 0, false);
        }
    }
}
using UnityEngine;
using UnityEngine.UI; // Necesario para obtener las referencias de tipo Image

public class Controlador_Poderes : MonoBehaviour
{
    private PowerSelector selectorUI;
    public bool bloqueadoPorDesplazador = false;

    [Header("Barra de Combustible (Seguimiento)")]
    public BarraSeguimiento barraSeguimiento; // Arrastra tu barra continua aquí

    // Variable para rastrear qué poder estaba equipado en el frame anterior
    private string ultimoNombrePoder = "";

    void Start()
    {
        selectorUI = FindObjectOfType<PowerSelector>();
        if (selectorUI == null)
        {
            Debug.LogError("¡ALERTA INICIAL!: No se encontró el HUD (PowerSelector) al iniciar el juego.");
        }
        else
        {
            // Sincroniza automáticamente el icono y las cargas iniciales al arrancar la escena
            Invoke("SincronizarNuevaBarraAlInicio", 0.05f);
        }
    }

    private void SincronizarNuevaBarraAlInicio()
    {
        ActualizarIconoYCargas();
    }

    void Update()
    {
        if (Time.timeScale == 0 || selectorUI == null) return;

        // ==========================================================
        // DETECCIÓN VISUAL: ¿El jugador cambió de poder en este frame?
        // ==========================================================
        string poderActual = selectorUI.ObtenerNombrePoderActual();
        if (poderActual != ultimoNombrePoder)
        {
            ActualizarIconoYCargas();
        }

        // Ejecución por pulsación de botón
        if (Input.GetKeyDown(KeyCode.X))
        {
            EjecutarPoderDeToque();
        }

        ProcesarPoderesContinuos();
    }

    /// <summary>
    /// Sincroniza de golpe tanto el Sprite del icono actual como las cargas en el HUD.
    /// </summary>
    private void ActualizarIconoYCargas()
    {
        if (selectorUI == null || HUDPoderesManager.Instance == null) return;

        // Actualizamos el rastro del nombre
        ultimoNombrePoder = selectorUI.ObtenerNombrePoderActual();

        // 1. OBTENER EL SPRITE DESDE EL RECTTRANSFORM CORRECTO DEL CIRCULO
        if (selectorUI.iconoPoderActual != null)
        {
            Image imgCirculo = selectorUI.iconoPoderActual.GetComponent<Image>();

            if (imgCirculo != null && imgCirculo.sprite != null)
            {
                // Le pasamos ese mismo sprite a tu barra fija del HUD
                HUDPoderesManager.Instance.CambiarIconoPoder(imgCirculo.sprite);
            }
        }

        // 2. FORZAR ACTUALIZACIÓN DE LAS CARGAS DEL NUEVO PODER (Reflexión)
        ActualizarHUDConCargasDelPoderActual();
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

        // Refrescar las cargas visuales inmediatamente en el HUD tras el uso de la tecla X
        ActualizarHUDConCargasDelPoderActual();
    }

    /// <summary>
    /// Escanea mediante Reflexión el script del poder actual y extrae dinámicamente sus cargasRestantes.
    /// </summary>
    private void ActualizarHUDConCargasDelPoderActual()
    {
        if (HUDPoderesManager.Instance == null || selectorUI == null) return;

        string nombrePoder = selectorUI.ObtenerNombrePoderActual();

        // Si es un poder continuo, forzamos a la barra de círculos a ponerse en 0 (oculta o vacía)
        if (nombrePoder == "G-Inversor" || nombrePoder == "Mochila de Cocas")
        {
            HUDPoderesManager.Instance.ActualizarCargasVisuales(0);
            return;
        }

        // Formateamos el string para buscar el componente (Ej: "Plataforma Estatica" -> "PlataformaEstaticaPower")
        string nombreLimpio = nombrePoder.Replace(" ", "");
        string nombreScriptPoder = nombreLimpio + "Power";

        // Buscamos el componente adjunto en este mismo objeto
        Component scriptPoder = GetComponent(nombreScriptPoder);

        if (scriptPoder != null)
        {
            // Buscamos la variable pública entera "cargasRestantes" en la clase
            var campoCargas = scriptPoder.GetType().GetField("cargasRestantes");

            if (campoCargas != null)
            {
                // Extraemos el valor real
                int cargasFieles = (int)campoCargas.GetValue(scriptPoder);

                // Lo mandamos al HUD
                HUDPoderesManager.Instance.ActualizarCargasVisuales(cargasFieles);
                return;
            }
        }

        // Por seguridad, si el script no se encuentra o no tiene cargas, se manda 0
        HUDPoderesManager.Instance.ActualizarCargasVisuales(0);
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

        // Apagar la barra de combustible continua si no está activa
        if (!mostrarBarra && barraSeguimiento != null)
        {
            barraSeguimiento.ActualizarEstado(0, 0, false);
        }
    }
}
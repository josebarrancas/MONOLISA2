using UnityEngine;
using UnityEngine.UI; // Necesario para extraer el Sprite del selector principal
using System;

public class Controlador_Poderes_Principal : MonoBehaviour
{
    private PowerSelectorPrincipal miHUD;
    [HideInInspector] public bool bloqueadoPorDesplazador = false;

    [Header("Barra de Combustible (Seguimiento)")]
    public BarraSeguimiento barraSeguimiento; // Arrastra aquí el objeto con el script BarraSeguimiento

    // Variable para rastrear qué poder estaba equipado en el frame anterior
    private string ultimoNombrePoder = "";


    void Start()
    {
        miHUD = FindObjectOfType<PowerSelectorPrincipal>();

        if (miHUD == null)
        {
            Debug.LogWarning("Controlador_Poderes_Principal: No se encontró 'PowerSelectorPrincipal' en esta escena.");
        }
        else
        {
            // Sincroniza automáticamente el icono y las cargas al arrancar el nivel
            Invoke(nameof(SincronizarNuevaBarraAlInicio), 0.05f);
        }
    }

    private void SincronizarNuevaBarraAlInicio()
    {
        ActualizarIconoYCargas();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // ==========================================================
        // DETECCIÓN VISUAL: ¿El jugador cambió de poder en este frame?
        // ==========================================================
        if (miHUD != null)
        {
            string poderActual = miHUD.ObtenerNombrePoderActual();
            if (poderActual != ultimoNombrePoder)
            {
                ActualizarIconoYCargas();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            EjecutarPoderDeToque();
        }

        ProcesarPoderesContinuos();
    }

    /// <summary>
    /// Sincroniza el Sprite del icono desde el PowerSelectorPrincipal y actualiza las cargas en el HUD.
    /// </summary>
    private void ActualizarIconoYCargas()
    {
        if (miHUD == null || HUDPoderesManager.Instance == null) return;

        // Actualizamos el rastro del nombre original
        ultimoNombrePoder = miHUD.ObtenerNombrePoderActual();

        // Buscamos el componente Image dentro de la referencia 'iconoPoderActual' de tu script principal
        if (miHUD.iconoPoderActual != null)
        {
            Image imgCirculo = miHUD.iconoPoderActual.GetComponent<Image>();

            if (imgCirculo != null && imgCirculo.sprite != null)
            {
                // Enviamos el sprite al HUD de cargas fijas
                HUDPoderesManager.Instance.CambiarIconoPoder(imgCirculo.sprite);
            }
        }

        // Actualizar las cargas de inmediato
        ActualizarHUDConCargasDelPoderActual();
    }


    private void EjecutarPoderDeToque()
    {
        if (miHUD == null) return;

        string rawNombre = miHUD.ObtenerNombrePoderActual();
        if (string.IsNullOrEmpty(rawNombre)) return;

        string nombre = rawNombre.Trim().ToLower();

        // -- EMBESTIFRESA --
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
        // -- PLATAFORMA ESTÁTICA --
        else if (nombre == "plataforma estatica" || nombre == "plataforma estática")
        {
            PlataformaEstaticaPower script = GetComponent<PlataformaEstaticaPower>();
            if (script != null && script.cargasRestantes > 0)
            {
                script.EjecutarPlataforma();
                RegistrarEventoPoder(rawNombre);
            }
        }
        // -- COCA AGITADA --
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
        // -- RESETEO LOCAL --
        else if (nombre == "reseteo local")
        {
            ReseteoLocalPower script = GetComponent<ReseteoLocalPower>();
            if (script != null)
            {
                script.EjecutarReseteo();
                RegistrarEventoPoder(rawNombre);
            }
        }
        // -- BRÚJULA GRAVITACIONAL --
        else if (nombre == "brujula gravitacional" || nombre == "brújula gravitacional")
        {
            BrujulaGravitacionalPower script = GetComponent<BrujulaGravitacionalPower>();
            if (script != null)
            {
                script.EjecutarBrujula();
                RegistrarEventoPoder(rawNombre);
            }
        }
        // -- CUBO REPULSOR --
        else if (nombre == "cubo repulsor")
        {
            CuboRepulsorPower script = GetComponent<CuboRepulsorPower>();
            if (script != null)
            {
                script.EjecutarPoder();
                RegistrarEventoPoder(rawNombre);
            }
        }
        // -- GEL ADHERENTE --
        else if (nombre == "gel adherente")
        {
            GelAdherentePower script = GetComponent<GelAdherentePower>();
            if (script != null)
            {
                script.EjecutarDisparo();
                RegistrarEventoPoder(rawNombre);
            }
        }
        // -- PASO SOMBRA --
        else if (nombre == "paso sombra")
        {
            PasoSombraPower script = GetComponent<PasoSombraPower>();
            if (script != null)
            {
                if (script.EjecutarPasoSombra()) RegistrarEventoPoder(rawNombre);
            }
        }
        // -- SINGULARIDAD --
        else if (nombre == "singularidad")
        {
            SingularidadPower script = GetComponent<SingularidadPower>();
            if (script != null)
            {
                script.EjecutarSingularidad();
                RegistrarEventoPoder(rawNombre);
            }
        }
        // -- ERROR DE CÓDIGO --
        else if (nombre == "error de codigo" || nombre == "error de código")
        {
            ErrorDeCodigoPower script = GetComponent<ErrorDeCodigoPower>();
            if (script != null)
            {
                script.EjecutarIntercambio();
                RegistrarEventoPoder(rawNombre);
            }
        }
        // -- IMÁN / DESPLAZADOR --
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
                RegistrarEventoPoder(rawNombre);
            }
        }

        // Refrescar las cargas visuales inmediatamente tras gastar un uso con la tecla X
        ActualizarHUDConCargasDelPoderActual();
    }

    /// <summary>
    /// Escanea el script del poder actual y extrae dinámicamente sus cargasRestantes.
    /// </summary>
    private void ActualizarHUDConCargasDelPoderActual()
    {
        if (HUDPoderesManager.Instance == null || miHUD == null) return;

        string rawNombre = miHUD.ObtenerNombrePoderActual();
        if (string.IsNullOrEmpty(rawNombre)) return;

        string nombre = rawNombre.Trim().ToLower();

        // Excluimos explícitamente los que no son por cargas discretas de toque
        if (nombre == "g-inversor" || nombre == "mochila de cocas")
        {
            HUDPoderesManager.Instance.ActualizarCargasVisuales(0);
            return;
        }

        // Normalizamos el string para que coincida con los nombres de las clases de C#
        string nombreLimpio = rawNombre.Replace(" ", "");
        nombreLimpio = nombreLimpio.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
        nombreLimpio = nombreLimpio.Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U");

        string nombreScriptPoder = nombreLimpio + "Power";

        // Buscamos el script adjunto en el jugador
        Component scriptPoder = GetComponent(nombreScriptPoder);

        if (scriptPoder != null)
        {
            var campoCargas = scriptPoder.GetType().GetField("cargasRestantes");

            if (campoCargas != null)
            {
                int cargasFieles = (int)campoCargas.GetValue(scriptPoder);
                HUDPoderesManager.Instance.ActualizarCargasVisuales(cargasFieles);
                return;
            }
        }

        HUDPoderesManager.Instance.ActualizarCargasVisuales(0);
    }

    private void RegistrarEventoPoder(string nombreOriginal)
    {
        if (miHUD != null) miHUD.RegistrarUsoDePoder();

        if (SkillManager.Instance != null)
        {
            SkillManager.Instance.RegistraUsoPoder(nombreOriginal);
        }
    }



    private void ProcesarPoderesContinuos()
    {
        if (miHUD == null)
        {
            Debug.LogError("[CONTROLADOR] ¡miHUD es NULO! No se puede leer el poder actual.");
            return;
        }

        string rawNombre = miHUD.ObtenerNombrePoderActual();
        if (string.IsNullOrEmpty(rawNombre)) return;

        string nombre = rawNombre.Trim().ToLower();
        bool mostrarBarra = false;

        // G-INVERSOR
        GInversorPower scriptGravedad = GetComponent<GInversorPower>();
        if (scriptGravedad != null)
        {
            bool esGInversor = (nombre == "g-inversor" || nombre.Contains("inversor"));
            bool quiereInvertir = esGInversor && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;

            scriptGravedad.ProcesarInversion(quiereInvertir);

            if (esGInversor && barraSeguimiento != null)
            {
                barraSeguimiento.ActualizarEstado(scriptGravedad.nivelMedidor, scriptGravedad.medidorMaximo, true);
                mostrarBarra = true;
            }
        }

        // MOCHILA DE COCAS
        MochilaCocasPower scriptMochila = GetComponent<MochilaCocasPower>();
        if (scriptMochila != null)
        {
            bool esMochila = (nombre == "mochila de cocas" || nombre.Contains("mochila") || nombre.Contains("cocas"));
            bool usandoMochila = esMochila && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;

            scriptMochila.ProcesarVuelo(usandoMochila);

            if (esMochila && barraSeguimiento != null)
            {
                barraSeguimiento.ActualizarEstado(scriptMochila.nivelMedidor, scriptMochila.medidorMaximo, true);
                mostrarBarra = true;
            }
        }

        // SINGULARIDAD
        SingularidadPower scriptSingularidad = GetComponent<SingularidadPower>();
        if (scriptSingularidad != null)
        {
            scriptSingularidad.estaSeleccionado = (nombre == "singularidad" || nombre.Contains("singularidad"));
        }

        // Apagar la barra de combustible continua si no se está usando ninguna habilidad de este tipo
        if (!mostrarBarra && barraSeguimiento != null)
        {
            barraSeguimiento.ActualizarEstado(0, 0, false);
        }

    }

}
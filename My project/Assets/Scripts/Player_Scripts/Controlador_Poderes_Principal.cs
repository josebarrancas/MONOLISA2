using UnityEngine;

public class Controlador_Poderes_Principal : MonoBehaviour
{
    // Usaremos solo una variable para evitar confusiones
    private PowerSelectorPrincipal miHUD;
    public bool bloqueadoPorDesplazador = false;

    void Start()
    {
        // Buscamos el HUD en la escena actual
        miHUD = FindObjectOfType<PowerSelectorPrincipal>();

        if (miHUD == null)
        {
            Debug.LogError("¡ALERTA!: No se encontró PowerSelectorPrincipal en esta escena.");
        }
    }

    void Update()
    {
        if (Time.timeScale == 0 || miHUD == null) return;

        // Poderes de un solo toque
        if (Input.GetKeyDown(KeyCode.X))
        {
            EjecutarPoderDeToque();
        }

        // Poderes que requieren mantener la tecla
        ProcesarPoderesContinuos();
    }

    private void EjecutarPoderDeToque()
    {
        // Obtenemos el nombre de forma segura
        string nombrePoder = miHUD.ObtenerNombrePoderActual();
        if (string.IsNullOrEmpty(nombrePoder)) return;

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
            if (nombrePoder == "Error de codigo") GetComponent<ErrorDeCodigoPower>()?.EjecutarIntercambio();
        }
    }

    private void RegistrarEventoPoder(string nombre)
    {
        miHUD.RegistrarUsoDePoder();
        if (SkillManager.Instance != null)
        {
            SkillManager.Instance.RegistraUsoPoder(nombre);
        }
    }

    private void ProcesarPoderesContinuos()
    {
        string nombrePoder = miHUD.ObtenerNombrePoderActual();
        if (string.IsNullOrEmpty(nombrePoder)) return;

        // G-INVERSOR
        GInversorPower scriptGravedad = GetComponent<GInversorPower>();
        if (scriptGravedad != null)
        {
            bool quiereInvertir = (nombrePoder == "G-Inversor") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptGravedad.ProcesarInversion(quiereInvertir);
        }

        // MOCHILA DE COCAS
        MochilaCocasPower scriptMochila = GetComponent<MochilaCocasPower>();
        if (scriptMochila != null)
        {
            bool usandoMochila = (nombrePoder == "Mochila de Cocas") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptMochila.ProcesarVuelo(usandoMochila);
        }
    }
}
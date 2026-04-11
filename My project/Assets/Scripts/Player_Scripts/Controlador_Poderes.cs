using UnityEngine;

public class Controlador_Poderes : MonoBehaviour
{
    private PowerSelector selectorUI;
    public bool bloqueadoPorDesplazador = false;

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

        // 1. PODERES DE UN SOLO TOQUE
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("PASO 1: La tecla X fue detectada correctamente.");
            EjecutarPoderDeToque();
        }

        // 2. PODERES CONTINUOS DE MEDIDOR
        ProcesarPoderesContinuos();
    }

    private void EjecutarPoderDeToque()
    {
        if (selectorUI == null)
        {
            Debug.LogError("PASO 2 FALLIDO: El selectorUI está vacío. El Player no puede ver el HUD.");
            return;
        }

        string nombrePoder = selectorUI.ObtenerNombrePoderActual();
        Debug.Log("PASO 2 ÉXITO: El HUD dice que el poder activo es: [" + nombrePoder + "]");

        // -- EMBESTIFRESA --
        if (nombrePoder == "Embestifresa")
        {
            if (bloqueadoPorDesplazador) return;
            EmbestifresaPower scriptImpulso = GetComponent<EmbestifresaPower>();
            if (scriptImpulso != null)
            {
                Vector2 direccion = new Vector2(transform.localScale.x, 0).normalized;
                scriptImpulso.EjecutarImpulso(direccion);
                selectorUI.RegistrarUsoDePoder();
                Debug.Log("PASO 3: Embestifresa disparada.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script EmbestifresaPower en el Player.");
        }
        // -- PLATAFORMA ESTÁTICA --
        else if (nombrePoder == "Plataforma Estatica")
        {
            PlataformaEstaticaPower scriptPlataforma = GetComponent<PlataformaEstaticaPower>();
            if (scriptPlataforma != null)
            {
                scriptPlataforma.EjecutarPlataforma();
                selectorUI.RegistrarUsoDePoder();
                Debug.Log("PASO 3: Plataforma Estática disparada.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script PlataformaEstaticaPower en el Player.");
        }
        // -- COCA AGITADA (ELEVADOR) --
        else if (nombrePoder == "Coca Agitada")
        {
            if (bloqueadoPorDesplazador) return;
            CocaAgitadaPower scriptCocas = GetComponent<CocaAgitadaPower>();
            if (scriptCocas != null)
            {
                scriptCocas.EjecutarElevacion();
                selectorUI.RegistrarUsoDePoder();
                Debug.Log("PASO 3: Coca Agitada disparada.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script CocaAgitadaPower en el Player.");
        }
        // -- RESETEO LOCAL --
        else if (nombrePoder == "Reseteo local")
        {
            ReseteoLocalPower scriptReseteo = GetComponent<ReseteoLocalPower>();
            if (scriptReseteo != null)
            {
                scriptReseteo.EjecutarReseteo();
                selectorUI.RegistrarUsoDePoder();
                Debug.Log("PASO 3: Reseteo Local disparado.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script ReseteoLocalPower en el Player.");
        }
        // -- BRÚJULA GRAVITACIONAL --
        else if (nombrePoder == "Brujula Gravitacional")
        {
            BrujulaGravitacionalPower scriptBrujula = GetComponent<BrujulaGravitacionalPower>();
            if (scriptBrujula != null)
            {
                scriptBrujula.EjecutarBrujula();
                selectorUI.RegistrarUsoDePoder();
                Debug.Log("PASO 3: Brújula Gravitacional disparada.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script BrujulaGravitacionalPower en el Player.");
        }
        
        // -- CUBO REPULSOR --
        else if (nombrePoder == "Cubo Repulsor")
        {
            CuboRepulsorPower scriptCubo = GetComponent<CuboRepulsorPower>();
            if (scriptCubo != null)
            {
                scriptCubo.EjecutarPoder();
                selectorUI.RegistrarUsoDePoder();
                Debug.Log("PASO 3: Cubo Repulsor disparado.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script CuboRepulsorPower en el Player.");
        }
        // -- GEL ADHERENTE --
        else if (nombrePoder == "Gel Adherente")
        {
            GelAdherentePower scriptGel = GetComponent<GelAdherentePower>();
            if (scriptGel != null)
            {
                scriptGel.EjecutarDisparo();
                selectorUI.RegistrarUsoDePoder();
                Debug.Log("PASO 3: Gel Adherente disparado.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script GelAdherentePower en el Player.");
        }
        // -- PASO SOMBRA --
        else if (nombrePoder == "Paso Sombra")
        {
            PasoSombraPower scriptSombra = GetComponent<PasoSombraPower>();
            if (scriptSombra != null)
            {
                // Solo si la función retorna TRUE (Fase 2 completada), descontamos del HUD
                bool seConsumio = scriptSombra.EjecutarPasoSombra();
                if (seConsumio)
                {
                    selectorUI.RegistrarUsoDePoder();
                }
                Debug.Log("PASO 3: Paso Sombra accionado.");
            }
            else Debug.LogError("PASO 3 FALLIDO: Falta el script PasoSombraPower en el Player.");
        }
        // -- NUEVO: DESPLAZADOR --
        else if (nombrePoder == "Desplazador")
        {
            DesplazadorPower scriptDesp = GetComponent<DesplazadorPower>();
            if (scriptDesp != null)
            {
                scriptDesp.EjecutarPoder();
                selectorUI.RegistrarUsoDePoder();
            }
        }
        // -- IMÁN --
        else if (nombrePoder == "Iman")
        {
            ImanPower scriptIman = GetComponent<ImanPower>();
            if (scriptIman != null)
            {
                scriptIman.EjecutarIman();
            }
        }
        // -- SINGULARIDAD --
        else if (nombrePoder == "Singularidad")
        {
            SingularidadPower scriptSingularidad = GetComponent<SingularidadPower>();
            if (scriptSingularidad != null)
            {
                scriptSingularidad.EjecutarSingularidad();
            }
        }
        else
        {
            Debug.LogWarning("PASO 3: El nombre [" + nombrePoder + "] no coincide con ningún 'if' programado de toque.");
        }
    }

    private void ProcesarPoderesContinuos()
    {
        if (selectorUI == null) return;
        string nombrePoder = selectorUI.ObtenerNombrePoderActual();

        // -- G-INVERSOR --
        GInversorPower scriptGravedad = GetComponent<GInversorPower>();
        if (scriptGravedad != null)
        {
            bool quiereInvertir = (nombrePoder == "G-Inversor") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptGravedad.ProcesarInversion(quiereInvertir);
        }

        // -- MOCHILA DE COCAS --
        MochilaCocasPower scriptMochila = GetComponent<MochilaCocasPower>();
        if (scriptMochila != null)
        {
            bool usandoMochila = (nombrePoder == "Mochila de Cocas") && Input.GetKey(KeyCode.X) && !bloqueadoPorDesplazador;
            scriptMochila.ProcesarVuelo(usandoMochila);
        }
    }
}
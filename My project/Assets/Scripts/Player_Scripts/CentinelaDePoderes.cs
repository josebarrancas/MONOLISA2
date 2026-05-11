using UnityEngine;
using UnityEngine.SceneManagement;

public class CentinelaDePoderes : MonoBehaviour
{
    private bool yaMurio = false;

    [Header("Referencias de Poderes")]
    private EmbestifresaPower embestifresa;
    private PlataformaEstaticaPower plataforma;
    private CocaAgitadaPower coca;
    private GelAdherentePower gel;
    private PasoSombraPower sombra;
    private DesplazadorPower desplaza;
    private ImanPower iman;
    private BrujulaGravitacionalPower brujula;
    private SingularidadPower singu;
    private ErrorDeCodigoPower error;
    private GInversorPower inversor;
    private MochilaCocasPower mochila;

    void Start()
    {
        // Asignamos los componentes que están en el Player
        embestifresa = GetComponent<EmbestifresaPower>();
        plataforma = GetComponent<PlataformaEstaticaPower>();
        coca = GetComponent<CocaAgitadaPower>();
        gel = GetComponent<GelAdherentePower>();
        sombra = GetComponent<PasoSombraPower>();
        desplaza = GetComponent<DesplazadorPower>();
        iman = GetComponent<ImanPower>();
        brujula = GetComponent<BrujulaGravitacionalPower>();
        singu = GetComponent<SingularidadPower>();
        error = GetComponent<ErrorDeCodigoPower>();
        inversor = GetComponent<GInversorPower>();
        mochila = GetComponent<MochilaCocasPower>();
    }

    void Update()
    {
        if (yaMurio) return;

        // Solo vigilamos si el Juez activó "No te acabes los poderes"
        if (CondicionesManager.Instance != null &&
            CondicionesManager.Instance.condicionActual == TipoCondicion.NoTeAcabesLosPoderes)
        {
            VerificarMuertePorAgotamiento();
        }
    }

    // --- ESTA ES LA ÚNICA VERSIÓN QUE DEBE EXISTIR ---
    void VerificarMuertePorAgotamiento()
    {
        // 1. PODERES BASADOS EN CARGAS
        if (embestifresa != null && !embestifresa.esUnico && embestifresa.cargasRestantes <= 0) { Matar(); return; }
        if (plataforma != null && !plataforma.esUnico && plataforma.cargasRestantes <= 0) { Matar(); return; }
        if (coca != null && !coca.esUnico && coca.cargasRestantes <= 0) { Matar(); return; }
        if (gel != null && !gel.esUnico && gel.cargasRestantes <= 0) { Matar(); return; }
        if (sombra != null && !sombra.esUnico && sombra.cargasRestantes <= 0) { Matar(); return; }
        if (desplaza != null && !desplaza.esUnico && desplaza.cargasRestantes <= 0) { Matar(); return; }
        if (iman != null && !iman.esUnico && iman.cargasRestantes <= 0) { Matar(); return; }
        if (singu != null && !singu.esUnico && singu.cargasRestantes <= 0) { Matar(); return; }
        if (error != null && !error.esUnico && error.cargasRestantes <= 0) { Matar(); return; }

        // 2. PODERES CON ESTADO ACTIVO (No matan mientras el efecto dure)
        if (brujula != null && !brujula.esUnico && brujula.cargasRestantes <= 0 && !brujula.estaUsandoBrujula) { Matar(); return; }
        if (inversor != null && !inversor.esUnico && inversor.nivelMedidor <= 0.01f && !inversor.estaInvertido) { Matar(); return; }

        // 3. PODERES DE MEDIDOR SIMPLE
        if (mochila != null && !mochila.esUnico && mochila.nivelMedidor <= 0.01f) { Matar(); return; }
    }

    void Matar()
    {
        yaMurio = true;
        Debug.Log("<color=red>CENTINELA:</color> ¡Sentencia ejecutada! Te quedaste sin recursos.");

        // Regresa a la selección para gastar un intento de los 11 totales
        SceneManager.LoadScene("Pantalla_Seleccion");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReseteoLocalPower : MonoBehaviour
{
    [Header("Configuración")]
    public int cargasRestantes = 1;
    public bool esUnico = false;

    // --- VARIABLES ESTÁTICAS (Sobreviven a la recarga de escena) ---
    public static bool vieneDeReseteoLocal = false;

    // Poderes Originales
    public static int save_Embestifresa;
    public static int save_Plataforma;
    public static int save_CocaAgitada;
    public static float save_GInversor;
    public static float save_MochilaCocas;

    // Poderes Nuevos
    public static int save_GelAdherente;
    public static int save_PasoSombra;
    public static int save_Desplazador;
    public static int save_Iman;
    public static int save_Brujula;
    public static int save_Singularidad;
    public static int save_ErrorCodigo;

    public static int save_Reseteos;

    private void Start()
    {
        // Al iniciar la escena, si la bandera es verdadera, significa que usamos el poder
        if (vieneDeReseteoLocal)
        {
            RestaurarPoderes();
            // Apagamos la bandera para que, si el jugador muere de verdad, el reinicio sea normal
            vieneDeReseteoLocal = false;
        }
    }

    public void EjecutarReseteo()
    {
        if (cargasRestantes > 0)
        {
            cargasRestantes--; // Gastamos este uso
            GuardarPoderesMemorizados(); // Salvamos el estado actual
            vieneDeReseteoLocal = true; // Avisamos que el siguiente reinicio es un "Reseteo Local"

            Debug.Log("¡RESETEO LOCAL! Recargando el layout del nivel...");

            // Recargamos la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("¡No quedan cargas de Reseteo Local!");
        }
    }

    private void GuardarPoderesMemorizados()
    {
        // --- GUARDAR PODERES ORIGINALES ---
        var emb = GetComponent<EmbestifresaPower>();
        if (emb != null) save_Embestifresa = emb.cargasRestantes;

        var plat = GetComponent<PlataformaEstaticaPower>();
        if (plat != null) save_Plataforma = plat.cargasRestantes;

        var coca = GetComponent<CocaAgitadaPower>();
        if (coca != null) save_CocaAgitada = coca.cargasRestantes;

        var inv = GetComponent<GInversorPower>();
        if (inv != null) save_GInversor = inv.nivelMedidor;

        var moch = GetComponent<MochilaCocasPower>();
        if (moch != null) save_MochilaCocas = moch.nivelMedidor;

        var gel = GetComponent<GelAdherentePower>();
        if (gel != null) save_GelAdherente = gel.cargasRestantes;

        var sombra = GetComponent<PasoSombraPower>();
        if (sombra != null) save_PasoSombra = sombra.cargasRestantes;

        var desp = GetComponent<DesplazadorPower>();
        if (desp != null) save_Desplazador = desp.cargasRestantes;

        var iman = GetComponent<ImanPower>();
        if (iman != null) save_Iman = iman.cargasRestantes;

        var brujula = GetComponent<BrujulaGravitacionalPower>();
        if (brujula != null) save_Brujula = brujula.cargasRestantes;

        var sing = GetComponent<SingularidadPower>();
        if (sing != null) save_Singularidad = sing.cargasRestantes;

        save_Reseteos = cargasRestantes;
    }

    private void RestaurarPoderes()
    {
        // --- RESTAURAR PODERES ORIGINALES ---
        var emb = GetComponent<EmbestifresaPower>();
        if (emb != null) emb.cargasRestantes = save_Embestifresa;

        var plat = GetComponent<PlataformaEstaticaPower>();
        if (plat != null) plat.cargasRestantes = save_Plataforma;

        var coca = GetComponent<CocaAgitadaPower>();
        if (coca != null) coca.cargasRestantes = save_CocaAgitada;

        var inv = GetComponent<GInversorPower>();
        if (inv != null) inv.nivelMedidor = save_GInversor;

        var moch = GetComponent<MochilaCocasPower>();
        if (moch != null) moch.nivelMedidor = save_MochilaCocas;

        // --- RESTAURAR PODERES NUEVOS ---
        var gel = GetComponent<GelAdherentePower>();
        if (gel != null) gel.cargasRestantes = save_GelAdherente;

        var sombra = GetComponent<PasoSombraPower>();
        if (sombra != null) sombra.cargasRestantes = save_PasoSombra;

        var desp = GetComponent<DesplazadorPower>();
        if (desp != null) desp.cargasRestantes = save_Desplazador;

        var iman = GetComponent<ImanPower>();
        if (iman != null) iman.cargasRestantes = save_Iman;

        var brujula = GetComponent<BrujulaGravitacionalPower>();
        if (brujula != null) brujula.cargasRestantes = save_Brujula;

        var sing = GetComponent<SingularidadPower>();
        if (sing != null) sing.cargasRestantes = save_Singularidad;

        var error = GetComponent<ErrorDeCodigoPower>();
        if (error != null) error.cargasRestantes = save_ErrorCodigo;

        cargasRestantes = save_Reseteos;

        Debug.Log("Layout reiniciado por Reseteo Local. Todos los poderes se han mantenido en sus valores previos.");
    }

    public void ResetearCargas()
    {
        cargasRestantes = 1;
    }
}
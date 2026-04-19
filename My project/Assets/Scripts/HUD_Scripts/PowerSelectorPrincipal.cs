using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerSelectorPrincipal : MonoBehaviour
{
    [Header("Base de Datos de Poderes")]
    public List<PoderData> todosLosPoderes = new List<PoderData>();
    private List<PoderData> mis9Poderes = new List<PoderData>();
    private int indiceActual;

    [Header("Referencias UI")]
    public RectTransform circuloAzul;
    public CanvasGroup grupoInformacion;
    public RectTransform iconoPoderActual;

    [Header("Textos del Poder")]
    public TextMeshProUGUI txtNombre;
    public TextMeshProUGUI txtEtiquetas;
    public TextMeshProUGUI txtExplicacion;
    public TextMeshProUGUI txtCargas; // <-- Asegúrate de crear este texto en tu HUD

    [Header("Configuración Círculo")]
    public Vector3 escalaExpandida = new Vector3();

    [Header("Configuración Icono")]
    public Vector3 escalaIconoExpandido = new Vector3();
    public float desplazamientoDerecha;

    [Header("Animación")]
    public float velocidadSuavizado = 10f;

    private bool expandido = false;
    private Vector3 posicionOriginalIcono;

    void Start()
    {
        if (iconoPoderActual != null)
            posicionOriginalIcono = iconoPoderActual.anchoredPosition;

        // Intentar cargar los poderes inmediatamente al aparecer en la escena
        PrepararPartida();


    }

    public void PrepararPartida()
    {
        // 1. Buscamos el Logic_Manager para obtener los poderes reales asignados
        if (Logic_Manager.instance == null)
        {
            Debug.LogError("No se encontró el Logic_Manager en la escena.");
            return;
        }

        List<PoderData> poderesAsignados = Logic_Manager.instance.mazoJugador;

        if (poderesAsignados == null || poderesAsignados.Count == 0)
        {
            Debug.LogWarning("El Logic_Manager tiene un mazo vacío. ¿Ya se repartieron los poderes?");
            return;
        }

        // 2. Limpiamos la lista local del HUD
        mis9Poderes.Clear();

        // 3. Clonamos los poderes del mazo para que el HUD tenga sus propias copias 
        // (y así las cargas de uso no afecten al ScriptableObject original)
        foreach (PoderData poder in poderesAsignados)
        {
            mis9Poderes.Add(Instantiate(poder));
        }

        Debug.Log($"HUD configurado con {mis9Poderes.Count} poderes del mazo.");

        // 4. Mostramos el primer poder en el HUD
        indiceActual = 0;
        ActualizarVisualPoder(mis9Poderes[indiceActual]);
    }

    void Update()
    {
        if (this == null || circuloAzul == null) return;

        expandido = Input.GetKey(KeyCode.C);

        if (expandido)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) CambiarPoder(-1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) CambiarPoder(1);
        }

        float delta = Time.unscaledDeltaTime * velocidadSuavizado;

        Vector3 objetivoCirculo = expandido ? escalaExpandida : Vector3.one;
        circuloAzul.localScale = Vector3.Lerp(circuloAzul.localScale, objetivoCirculo, delta);

        Vector3 objetivoEscalaIcono = expandido ? escalaIconoExpandido : Vector3.one;
        iconoPoderActual.localScale = Vector3.Lerp(iconoPoderActual.localScale, objetivoEscalaIcono, delta);

        Vector3 objetivoPosicionIcono = expandido ?
            posicionOriginalIcono + new Vector3(desplazamientoDerecha, 0, 0) :
            posicionOriginalIcono;

        iconoPoderActual.anchoredPosition = Vector3.Lerp(iconoPoderActual.anchoredPosition, objetivoPosicionIcono, delta);

        grupoInformacion.alpha = Mathf.Lerp(grupoInformacion.alpha, expandido ? 1 : 0, delta);
    }

    void CambiarPoder(int direccion)
    {
        //========================================
        //          Borrar los debugs
        //========================================
        // PROTECCIÓN CRÍTICA: Si no hay poderes, no calculamos nada
        if (mis9Poderes == null || mis9Poderes.Count == 0)
        {
            Debug.LogWarning("Lista de poderes vacía en el HUD.");
            return;
        }

        indiceActual = (indiceActual + direccion + mis9Poderes.Count) % mis9Poderes.Count;
        ActualizarVisualPoder(mis9Poderes[indiceActual]);
    }

    public void ActualizarVisualPoder(PoderData datos)
    {
        // SEGURIDAD: Si no hay datos, no intentes actualizar nada
        if (datos == null)
        {
            Debug.LogWarning("Intentando actualizar HUD sin datos de poder.");
            return;
        }

        txtNombre.text = datos.nombre;
        txtEtiquetas.text = string.Join(" / ", datos.etiquetas);
        txtExplicacion.text = datos.explicacion;

        if (datos.tipo == TipoUso.Varios)
            txtCargas.text = "Usos: " + datos.cantidadUsos;
        else
            txtCargas.text = "";

        if (datos.icono != null)
            iconoPoderActual.GetComponent<Image>().sprite = datos.icono;
    }

    // --- NUEVAS FUNCIONES DE COMUNICACIÓN ---

    // Devuelve el nombre del poder actual para que el Player sepa qué disparar
    public string ObtenerNombrePoderActual()
    {
        // SEGURIDAD: Si la lista está vacía o el índice es inválido, devolvemos un string vacío
        if (mis9Poderes == null || mis9Poderes.Count == 0)
        {
            return "";
        }

        return mis9Poderes[indiceActual].nombre;
    }

    // Descuenta una carga y refresca el HUD
    public void RegistrarUsoDePoder()
    {
        PoderData actual = mis9Poderes[indiceActual];
        if (actual.tipo == TipoUso.Varios && actual.cantidadUsos > 0)
        {
            actual.cantidadUsos--;
            ActualizarVisualPoder(actual);
        }
    }
}

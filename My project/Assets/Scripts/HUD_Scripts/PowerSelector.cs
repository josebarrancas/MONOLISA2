using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerSelector : MonoBehaviour
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
    public TextMeshProUGUI txtCargas;

    [Header("Configuración Círculo")]
    public Vector3 escalaExpandida;

    [Header("Configuración Icono")]
    public Vector3 escalaIconoExpandido;
    public float desplazamientoDerecha;

    [Header("Animación")]
    public float velocidadSuavizado = 10f;

    private bool expandido = false;
    private Vector3 posicionOriginalIcono;

    void Start()
    {
        if (iconoPoderActual != null)
            posicionOriginalIcono = iconoPoderActual.anchoredPosition;

        PrepararPartida();
    }

    public void PrepararPartida()
    {
        if (todosLosPoderes == null || todosLosPoderes.Count == 0)
        {
            Debug.LogError("¡No hay poderes en la lista 'Todos Los Poderes'!");
            return;
        }

        mis9Poderes.Clear();

        // Llenamos la lista local con instancias de todos los poderes asignados en el Inspector
        for (int i = 0; i < todosLosPoderes.Count; i++)
        {
            mis9Poderes.Add(Instantiate(todosLosPoderes[i]));
        }

        if (mis9Poderes.Count > 0)
        {
            indiceActual = 0;
            ActualizarVisualPoder(mis9Poderes[indiceActual]);
        }
    }

    void Update()
    {
        if (circuloAzul == null) return;

        // ==========================================================
        // CANDADO DE SEGURIDAD: MENÚ DE PAUSA
        // ==========================================================
        if (ControlarPausaMenu.IsPausado)
        {
            // Forzamos a que el estado sea falso para que el menú se cierre si se quedó abierto
            expandido = false;
        }
        else
        {
            // Si el juego NO está pausado, leemos el teclado normalmente
            expandido = Input.GetKey(KeyCode.C);

            if (expandido)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) CambiarPoder(-1);
                if (Input.GetKeyDown(KeyCode.RightArrow)) CambiarPoder(1);
            }
        }

        // Mantenemos esta sección ejecutándose siempre con 'unscaledDeltaTime'
        // para que si pausas el juego, el semicírculo tenga permitido encogerse fluidamente.
        float delta = Time.unscaledDeltaTime * velocidadSuavizado;

        // Animación del Círculo
        Vector3 objetivoCirculo = expandido ? escalaExpandida : Vector3.one;
        circuloAzul.localScale = Vector3.Lerp(circuloAzul.localScale, objetivoCirculo, delta);

        // Animación del Icono
        Vector3 objetivoEscalaIcono = expandido ? escalaIconoExpandido : Vector3.one;
        iconoPoderActual.localScale = Vector3.Lerp(iconoPoderActual.localScale, objetivoEscalaIcono, delta);

        Vector3 objetivoPosicionIcono = expandido ?
            posicionOriginalIcono + new Vector3(desplazamientoDerecha, 0, 0) :
            posicionOriginalIcono;

        iconoPoderActual.anchoredPosition = Vector3.Lerp(iconoPoderActual.anchoredPosition, objetivoPosicionIcono, delta);

        // Animación del texto (Alpha)
        grupoInformacion.alpha = Mathf.Lerp(grupoInformacion.alpha, expandido ? 1 : 0, delta);
    }

    void CambiarPoder(int direccion)
    {
        if (mis9Poderes.Count == 0) return;

        indiceActual = (indiceActual + direccion + mis9Poderes.Count) % mis9Poderes.Count;
        ActualizarVisualPoder(mis9Poderes[indiceActual]);
    }

    public void ActualizarVisualPoder(PoderData datos)
    {
        if (datos == null) return;

        txtNombre.text = datos.nombre;
        txtEtiquetas.text = string.Join(" / ", datos.etiquetas);
        txtExplicacion.text = datos.explicacion;

        if (datos.tipo == TipoUso.Varios)
            txtCargas.text = "Usos: " + datos.cantidadUsos;
        else
            txtCargas.text = "";

        Image img = iconoPoderActual.GetComponent<Image>();
        if (img != null) img.sprite = datos.icono;
    }

    public string ObtenerNombrePoderActual()
    {
        return mis9Poderes.Count > 0 ? mis9Poderes[indiceActual].nombre : "";
    }

    public void RegistrarUsoDePoder()
    {
        if (mis9Poderes.Count == 0) return;

        PoderData actual = mis9Poderes[indiceActual];
        if (actual.tipo == TipoUso.Varios && actual.cantidadUsos > 0)
        {
            actual.cantidadUsos--;
            ActualizarVisualPoder(actual);
        }
    }
}
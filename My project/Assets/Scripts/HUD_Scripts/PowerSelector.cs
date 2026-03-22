using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerSelector : MonoBehaviour
{
    [Header("Base de Datos de Poderes")]
    public List<PoderData> todosLosPoderes = new List<PoderData>();
    private List<PoderData> mis9Poderes = new List<PoderData>();
    private int indiceActual = 0;

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
    public Vector3 escalaExpandida = new Vector3(2.5f, 2.5f, 1f);

    [Header("Configuración Icono")]
    public Vector3 escalaIconoExpandido = new Vector3(1.5f, 1.5f, 1f);
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

    void PrepararPartida()
    {
        if (todosLosPoderes.Count > 1) return;

        List<PoderData> copia = new List<PoderData>(todosLosPoderes);
        for (int i = 0; i < 9; i++)
        {
            int rnd = Random.Range(0, copia.Count);
            // IMPORTANTE: Instanciamos para no modificar el archivo original del poder
            mis9Poderes.Add(Instantiate(copia[rnd]));
            copia.RemoveAt(rnd);
        }

        ActualizarVisualPoder(mis9Poderes[indiceActual]);
    }

    void Update()
    {
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
        indiceActual = (indiceActual + direccion + mis9Poderes.Count) % mis9Poderes.Count;
        ActualizarVisualPoder(mis9Poderes[indiceActual]);
    }

    public void ActualizarVisualPoder(PoderData datos)
    {
        if (datos == null) return;

        txtNombre.text = datos.nombre;
        txtEtiquetas.text = string.Join(" / ", datos.etiquetas);
        txtExplicacion.text = datos.explicacion;

        // Actualizamos las cargas si el poder es de tipo Varios
        if (datos.tipo == TipoUso.Varios)
            txtCargas.text = "Usos: " + datos.cantidadUsos;
        else
            txtCargas.text = "";

        iconoPoderActual.GetComponent<Image>().sprite = datos.icono;
    }

    // --- NUEVAS FUNCIONES DE COMUNICACIÓN ---

    // Devuelve el nombre del poder actual para que el Player sepa qué disparar
    public string ObtenerNombrePoderActual()
    {
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

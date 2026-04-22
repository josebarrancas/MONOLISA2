using UnityEngine;
using System.Collections.Generic;

public class SingularidadPower : MonoBehaviour
{
    [Header("Configuración del Poder")]
    public int cargasRestantes = 3;
    [Tooltip("Qué tan lejos del jugador aparece la X.")]
    public float distanciaMira = 2f;
    [Tooltip("El ancho y alto del área de borrado.")]
    public Vector2 tamañoArea = new Vector2(2f, 2f);

    [Header("Visuales")]
    [Tooltip("Arrastre el Sprite de la 'X' grisácea aquí.")]
    public GameObject miraVisualX;

    [Header("Filtros de Etiquetas (Tags)")]
    [Tooltip("Escriba aquí los Tags EXACTOS de los objetos que la Singularidad borrará.")]
    public List<string> tagsDestruibles = new List<string> {
        "enganche", "desenganche", "flotante", "Coleccionable",
        "GelAdherente", "CuboRepulsor", "Desplazador", "estatico"
    };

    [Tooltip("Escriba los Tags que NUNCA deben borrarse (Seguridad).")]
    public List<string> tagsProtegidos = new List<string> {
        "Player", "Entrada", "Salida", "Dead"
    };

    [Header("Estado (Controlado desde el Controlador Principal)")]
    public bool estaSeleccionado = false;

    private Vector2 direccionApuntada;

    void Start()
    {
        if (miraVisualX != null) miraVisualX.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // La visualización de la mira ahora depende de lo que dicte el Controlador Principal
        if (estaSeleccionado)
        {
            if (miraVisualX != null) miraVisualX.SetActive(true);
            ActualizarMira();
        }
        else
        {
            if (miraVisualX != null) miraVisualX.SetActive(false);
        }
    }

    private void ActualizarMira()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Sistema de prioridad de 4 direcciones
        if (Mathf.Abs(v) > 0) direccionApuntada = new Vector2(0, Mathf.Sign(v));
        else if (Mathf.Abs(h) > 0) direccionApuntada = new Vector2(Mathf.Sign(h), 0);
        else direccionApuntada = new Vector2(Mathf.Sign(transform.localScale.x), 0);

        if (miraVisualX != null)
        {
            // Posición global de la mira (ignorando el espejo del jugador)
            miraVisualX.transform.position = transform.position + (Vector3)direccionApuntada * distanciaMira;
            // Obligamos a la X a no rotar ni voltearse
            miraVisualX.transform.rotation = Quaternion.identity;
        }
    }

    public void EjecutarSingularidad()
    {
        if (cargasRestantes <= 0) return;

        Vector2 centroExplosion = transform.position + (Vector3)direccionApuntada * distanciaMira;

        // Escaneamos el área de la "X" invisiblemente
        Collider2D[] objetosEnArea = Physics2D.OverlapBoxAll(centroExplosion, tamañoArea, 0f);

        foreach (Collider2D col in objetosEnArea)
        {
            // 1. Regla de oro: Las entradas y salidas NO pueden ser eliminadas
            if (tagsProtegidos.Contains(col.tag)) continue;

            // 2. Si el objeto tiene un Tag de la lista de vulnerables, se borra
            if (tagsDestruibles.Contains(col.tag))
            {
                Destroy(col.gameObject);
            }
        }

        // Consumimos el uso (El registro del evento ya lo hace el Controlador Principal)
        cargasRestantes--;
        Debug.Log("Singularidad: Área purgada. Cargas restantes: " + cargasRestantes);
    }

    // Herramienta visual exclusiva para el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Vector2 centro = Application.isPlaying && miraVisualX != null && miraVisualX.activeSelf ?
                         (Vector2)miraVisualX.transform.position :
                         (Vector2)transform.position + Vector2.right * distanciaMira;
        Gizmos.DrawWireCube(centro, tamañoArea);
    }

    public void ResetearCargas() { cargasRestantes = 3; }
}
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CajaPoder : MonoBehaviour
{
    [Header("Estado del Poder")]
    public PoderData poderContenido;

    private SpriteRenderer spriteBaseCaja; // La imagen de tu caja
    private SpriteRenderer renderIconoHijo; // El objeto dinámico que crearemos

    private void Awake()
    {
        spriteBaseCaja = GetComponent<SpriteRenderer>();

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        // Aseguramos que la caja NO sea un trigger para que tenga hitbox físico
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    public void AsignarPoder(PoderData nuevoPoder)
    {
        poderContenido = nuevoPoder;
        ActualizarVisual();
    }

    private void ActualizarVisual()
    {
        if (poderContenido != null && poderContenido.icono != null)
        {
            // 1. Si no existe un lienzo para el ícono, lo creamos dinámicamente como hijo
            if (renderIconoHijo == null)
            {
                GameObject iconoObj = new GameObject("Icono_Poder_Proyectado");
                iconoObj.transform.SetParent(this.transform);
                iconoObj.transform.localPosition = Vector3.zero; // Al centro de la caja

                renderIconoHijo = iconoObj.AddComponent<SpriteRenderer>();
                // Lo dibujamos una capa por encima de la caja para que no se oculte
                renderIconoHijo.sortingOrder = spriteBaseCaja.sortingOrder + 1;
            }

            renderIconoHijo.sprite = poderContenido.icono;

            // 2. Cálculo matemático para estandarizar el tamaño (70% del tamaño de la caja)
            Vector2 sizeCaja = spriteBaseCaja.bounds.size;
            Vector2 sizeIcono = renderIconoHijo.sprite.bounds.size;

            float scaleX = (sizeCaja.x * 0.7f) / sizeIcono.x;
            float scaleY = (sizeCaja.y * 0.7f) / sizeIcono.y;
            float escalaFinal = Mathf.Min(scaleX, scaleY); // Mantiene proporción perfecta

            // Compensamos la escala del padre para que no distorsione al hijo
            Vector3 escalaPadre = transform.localScale;
            renderIconoHijo.transform.localScale = new Vector3(escalaFinal / escalaPadre.x, escalaFinal / escalaPadre.y, 1f);

            renderIconoHijo.enabled = true;
        }
        else if (renderIconoHijo != null)
        {
            renderIconoHijo.enabled = false;
        }
    }

    // CORRECCIÓN: Método OnCollisionEnter2D para impactos físicos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // CORRECCIÓN: Con Collision2D se usa collision.gameObject.CompareTag
        if (collision.gameObject.CompareTag("Player") && poderContenido != null)
        {
            PowerSelectorPrincipal miHUD = FindObjectOfType<PowerSelectorPrincipal>();

            if (miHUD != null)
            {
                PoderData poderAnteriorJugador = miHUD.IntercambiarPoderActivo(poderContenido);

                if (poderAnteriorJugador != null)
                {
                    AsignarPoder(poderAnteriorJugador);
                    Debug.Log($"[Caja] Trueque exitoso. La caja ahora contiene: {poderAnteriorJugador.nombre}");
                }
            }
        }
    }
}
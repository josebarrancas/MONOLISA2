using UnityEngine;

public class PasoSombraPower : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("El Prefab visual de la sombra que se quedará en el mapa.")]
    public GameObject prefabSombra;
    public int cargasRestantes = 1;

    private GameObject sombraActiva;
    private bool sombraColocada = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Retorna TRUE si el poder se consumió por completo (Fase 2), 
    /// o FALSE si solo se colocó la sombra (Fase 1).
    /// </summary>
    public bool EjecutarPasoSombra()
    {
        // Limpieza de seguridad por si la sombra se destruyó por otra cosa
        if (sombraColocada && sombraActiva == null) sombraColocada = false;

        // FASE 1: Colocar la sombra en el mapa
        if (!sombraColocada && cargasRestantes > 0)
        {
            if (prefabSombra == null)
            {
                Debug.LogError("¡Falta asignar el Prefab de la sombra en el Inspector!");
                return false;
            }

            // Creamos la sombra y copiamos la escala del jugador (para que mire al mismo lado y respete si está de cabeza)
            sombraActiva = Instantiate(prefabSombra, transform.position, Quaternion.identity);
            sombraActiva.transform.localScale = transform.localScale;

            sombraColocada = true;
            Debug.Log("Paso Sombra: Sombra colocada. Presione X para regresar.");

            return false; // Retornamos falso para que el HUD aún no descuente el uso
        }
        // FASE 2: Teletransportarse
        else if (sombraColocada && sombraActiva != null)
        {
            // Nos movemos mágicamente a la posición de la sombra
            transform.position = sombraActiva.transform.position;

            // Frenamos al jugador para que tenga un "aterrizaje" suave
            rb.linearVelocity = Vector2.zero;

            // Destruimos la marca
            Destroy(sombraActiva);
            sombraColocada = false;

            cargasRestantes--;
            Debug.Log("Paso Sombra: Teletransporte exitoso. Cargas restantes: " + cargasRestantes);

            return true; // Retornamos verdadero para avisarle al HUD que descuente el uso
        }

        return false;
    }

    public void ResetearCargas()
    {
        cargasRestantes = 1;

        // Si recargan sus poderes mientras tenía una sombra activa, la borramos para evitar bugs
        if (sombraActiva != null)
        {
            Destroy(sombraActiva);
            sombraColocada = false;
        }
    }
}
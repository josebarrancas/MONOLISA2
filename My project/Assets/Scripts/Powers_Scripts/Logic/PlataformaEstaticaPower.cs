using UnityEngine;
using System.Collections.Generic;

public class PlataformaEstaticaPower : MonoBehaviour
{
    [Header("Configuración de la Plataforma")]
    [Tooltip("El prefab del bloque estático que servirá como plataforma.")]
    public GameObject plataformaPrefab;

    [Tooltip("Tiempo en segundos antes de que desaparezca")]
    public float tiempoDeVida = 0;

    [Tooltip("Distancia hacia abajo desde el centro del jugador para que aparezca en sus pies.")]
    public float offsetVertical = -1f;

    [Header("Límites de Uso")]
    [Tooltip("Número máximo de plataformas que pueden existir al mismo tiempo en el nivel.")]
    public int limiteMaximoEnPantalla = 3;

    // --- NUEVA LÓGICA DE CARGAS ESTILO EMBESTIFRESA ---
    public int cargasRestantes = 3;

    // Lista para llevar el conteo de las plataformas vivas
    private List<GameObject> plataformasCreadas = new List<GameObject>();

    /// <summary>
    /// Se ejecuta desde el PowerManager cuando es el poder activo.
    /// </summary>
    public void EjecutarPlataforma()
    {
        // Solo ejecutamos si nos quedan cargas disponibles
        if (cargasRestantes > 0)
        {
            if (plataformaPrefab == null)
            {
                Debug.LogError("¡ERROR!: No se ha asignado el prefab de la plataforma estática en el inspector.");
                return;
            }

            // 1. Limpieza de seguridad de las plataformas destruidas por tiempo
            plataformasCreadas.RemoveAll(plat => plat == null);

            // 2. Control de plataformas máximas en pantalla
            if (plataformasCreadas.Count >= limiteMaximoEnPantalla)
            {
                GameObject masVieja = plataformasCreadas[0];
                if (masVieja != null)
                {
                    Destroy(masVieja);
                }
                plataformasCreadas.RemoveAt(0);
            }

            // 3. Instanciamos la NUEVA plataforma
            Vector3 posicionSpawn = transform.position;
            posicionSpawn.y += offsetVertical;
            GameObject plataformaInstanciada = Instantiate(plataformaPrefab, posicionSpawn, Quaternion.identity);

            // Destrucción temporal
            if (tiempoDeVida > 0f)
            {
                Destroy(plataformaInstanciada, tiempoDeVida);
            }

            // Agregamos esta nueva plataforma a nuestra lista de control
            plataformasCreadas.Add(plataformaInstanciada);

            // 4. RESTAMOS LA CARGA Y REPORTAMOS
            cargasRestantes--;
            Debug.Log("Plataforma creada. Cargas restantes: " + cargasRestantes);
        }
        else
        {
            Debug.Log("¡No quedan cargas para Plataforma Estática!");
        }
    }

    /// <summary>
    /// Llama a esta función para devolverle al jugador sus 3 usos.
    /// </summary>
    public void ResetearCargas()
    {
        cargasRestantes = 3;
        Debug.Log("Cargas de Plataforma Estática reseteadas a 3.");
    }
}
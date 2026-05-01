using System.Collections;
using UnityEngine;

public class ImanLogic : MonoBehaviour
{
    private Vector2 direccion;
    private float velocidadVuelo = 15f;
    [Tooltip("Velocidad a la que se arrastrará el bloque hacia el origen.")]
    private float velocidadArraste = 12f;
    private float rangoMaximo = 6f;
    private Vector3 posicionInicial;
    private GameObject dueño;
    private LineRenderer soga;
    private bool impactado = false;

    public void Configurar(Vector2 dir, GameObject player)
    {
        direccion = dir;
        dueño = player;
        posicionInicial = player.transform.position;
        soga = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (dueño == null) { Destroy(gameObject); return; }

        // Dibujar la soga constantemente
        soga.SetPosition(0, dueño.transform.position);
        soga.SetPosition(1, transform.position);

        // Si aún no choca con nada, viaja hacia adelante
        if (!impactado)
        {
            transform.Translate(direccion * velocidadVuelo * Time.deltaTime);
            if (Vector3.Distance(posicionInicial, transform.position) >= rangoMaximo)
            {
                dueño.GetComponent<ImanPower>().GastarCarga();
                Destroy(gameObject); // Superó el rango sin tocar nada
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impactado) return; // Evita doble colisión

        string etiqueta = collision.tag;

        if (etiqueta == "estatico" || etiqueta == "Player" || etiqueta == "Dead")
        {
            if (etiqueta == "estatico")
            {
                impactado = true;
                Debug.Log("Imán chocó contra pared estática: Se rompe.");
                Destroy(gameObject);
            }
            return;
        }

        impactado = true;
        dueño.GetComponent<ImanPower>().GastarCarga();

        // 1. Lo convertimos en flotante (Etiqueta)
        collision.tag = "flotante";

        // 2. Le inyectamos el script Bloque_Flotante si no lo tiene
        Bloque_Flotante scriptBloque = collision.gameObject.GetComponent<Bloque_Flotante>();
        if (scriptBloque == null)
        {
            scriptBloque = collision.gameObject.AddComponent<Bloque_Flotante>();
            Debug.Log("<color=green>Transformación: Se inyectó Bloque_Flotante al objeto.</color>");
        }

        // 3. Nos aseguramos de que tenga Rigidbody2D
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = collision.gameObject.AddComponent<Rigidbody2D>();
        }

        // --- EL CAMBIO: FORZAR ESTADO DINÁMICO ---
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Iniciamos el viaje de regreso enviándole el Rigidbody para usar físicas limpias
        StartCoroutine(RutinaMoverPlataforma(collision.transform, rb));
    }

    private IEnumerator RutinaMoverPlataforma(Transform plataforma, Rigidbody2D rbPlataforma)
    {
        Vector3 destinoFinal = posicionInicial;
        float distanciaMinimaLlegada = 0.5f;

        while (plataforma != null && Vector3.Distance(plataforma.position, destinoFinal) > distanciaMinimaLlegada)
        {
            // Calculamos el nuevo paso usando fixedDeltaTime porque alteraremos físicas
            Vector3 nuevaPosicion = Vector3.MoveTowards(plataforma.position, destinoFinal, velocidadArraste * Time.fixedDeltaTime);

            // Movemos el objeto usando el motor de físicas de Unity para que no ignore paredes
            if (rbPlataforma != null)
            {
                rbPlataforma.MovePosition(nuevaPosicion);
            }

            // Pegamos el imán al bloque
            transform.position = plataforma.position;

            // IMPORTANTE: Al usar MovePosition, debemos esperar al FixedUpdate, no al Update normal
            yield return new WaitForFixedUpdate();
        }

        if (rbPlataforma != null)
        {
            rbPlataforma.linearVelocity = Vector2.zero; // Frenado total en seco
        }

        Destroy(gameObject);
    }
}
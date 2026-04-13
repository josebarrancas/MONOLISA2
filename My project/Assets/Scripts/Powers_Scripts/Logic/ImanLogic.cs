using System.Collections;
using UnityEngine;

public class ImanLogic : MonoBehaviour
{
    private Vector2 direccion;
    private float velocidadVuelo = 15f;
    [Tooltip("Velocidad a la que se arrastrará el bloque.")]
    private float velocidadArraste = 8f;
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
                Destroy(gameObject); // Superó los 6 bloques sin tocar nada
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impactado) return; // Evita doble colisión

        string etiqueta = collision.tag;

        if (etiqueta == "estatico")
        {
            impactado = true;
            Debug.Log("Imán tocó estático: Ignorado.");
            Destroy(gameObject); // Se rompe sin gastar uso
        }
        else if (etiqueta == "enganche" || etiqueta == "desenganche" || etiqueta == "flotante")
        {
            impactado = true;
            dueño.GetComponent<ImanPower>().GastarCarga(); // Cobra el uso

            if (etiqueta == "enganche")
            {
                collision.tag = "flotante";
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
            }

            // Inicia el temporizador de 1 segundo
            StartCoroutine(RutinaMoverPlataforma(collision.transform));
        }
    }

    private IEnumerator RutinaMoverPlataforma(Transform plataforma)
    {
        float tiempo = 0f;

        // Calculamos la dirección para ATRAERLO hacia el jugador.
        // Si literalmente quería EMPUJARLO lejos en la misma dirección del disparo, 
        // cambie esto a: Vector3 dirMovimiento = direccion;
        Vector3 dirMovimiento = (dueño.transform.position - plataforma.position).normalized;

        while (tiempo < 1f) // Bucle estricto de 1 segundo
        {
            if (plataforma != null)
            {
                // Movemos la plataforma
                plataforma.Translate(dirMovimiento * velocidadArraste * Time.deltaTime, Space.World);
                // Pegamos el imán a la plataforma para que la soga la siga visualmente
                transform.position = plataforma.position;
            }

            tiempo += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        // Se cumplió 1 segundo exacto: se destruye el imán y se corta la soga
        Destroy(gameObject);
    }
}
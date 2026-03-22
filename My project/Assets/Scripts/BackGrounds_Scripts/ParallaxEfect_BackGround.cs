using UnityEngine;

public class ParallaxEfecto : MonoBehaviour
{
    private float longitud, posicionInicial;
    public GameObject camara;

    [Tooltip("0 = Sigue a la cámara perfectamente, 1 = No se mueve (parece muy lejano)")]
    public float efectoParallax;

    void Start()
    {
        // Guardamos la posición X inicial de la capa
        posicionInicial = transform.position.x;

        // Buscamos el SpriteRenderer en los objetos de ADENTRO (los sprites reales)
        // para saber cuánto mide el dibujo y poder hacer el bucle infinito
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
        {
            longitud = sprite.bounds.size.x;
        }
        else
        {
            Debug.LogError("¡Error! No hay ningún SpriteRenderer dentro de: " + gameObject.name);
        }
    }

    void FixedUpdate()
    {
        // Calculamos la distancia que debe recorrer esta capa basándonos en la cámara
        float distancia = (camara.transform.position.x * efectoParallax);

        // Movemos la capa (el objeto "Hijo") a su nueva posición
        transform.position = new Vector3(posicionInicial + distancia, transform.position.y, transform.position.z);

        // Lógica de Repetición Infinita (Bucle)
        // Esto detecta si la cámara ya pasó el dibujo y lo teletransporta para que nunca se acabe
        float temp = (camara.transform.position.x * (1 - efectoParallax));

        if (temp > posicionInicial + longitud)
        {
            posicionInicial += longitud;
        }
        else if (temp < posicionInicial - longitud)
        {
            posicionInicial -= longitud;
        }
    }
}

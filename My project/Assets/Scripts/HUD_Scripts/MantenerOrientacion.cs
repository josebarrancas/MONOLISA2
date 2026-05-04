using UnityEngine;

public class MantenerOrientacion : MonoBehaviour
{
    private Quaternion rotacionInicial;
    private Vector3 escalaInicial;

    void Awake()
    {
        // Guardamos cómo debe verse originalmente
        rotacionInicial = transform.rotation;
        escalaInicial = transform.localScale;
    }

    void LateUpdate()
    {
        // Forzamos a que la rotación sea siempre la misma del mundo
        transform.rotation = rotacionInicial;

        // Evitamos que la escala se vuelva negativa si el padre escala -1
        Vector3 escalaPadre = transform.parent.localScale;
        transform.localScale = new Vector3(
            escalaInicial.x / Mathf.Sign(escalaPadre.x),
            escalaInicial.y,
            escalaInicial.z
        );
    }
}
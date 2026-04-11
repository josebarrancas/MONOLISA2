using UnityEngine;

public class ImanPower : MonoBehaviour
{
    public GameObject prefabIman;
    public int cargasRestantes = 3;

    public void EjecutarIman()
    {
        if (cargasRestantes <= 0) return;

        // Detectar 8 direcciones (Horizontal y Vertical)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dirDisparo = new Vector2(h, v).normalized;

        // Si no se presiona dirección, dispara hacia donde mira el personaje
        if (dirDisparo == Vector2.zero)
            dirDisparo = new Vector2(transform.localScale.x, 0).normalized;

        GameObject iman = Instantiate(prefabIman, transform.position, Quaternion.identity);
        iman.GetComponent<ImanLogic>().Configurar(dirDisparo, gameObject);
    }

    public void GastarCarga()
    {
        cargasRestantes--;
        FindObjectOfType<PowerSelector>().RegistrarUsoDePoder();
    }

    public void ResetearCargas() { cargasRestantes = 3; }
}
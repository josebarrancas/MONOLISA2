using UnityEngine;
using System.Collections.Generic;

public class PowerManager : MonoBehaviour
{
    public List<PoderData> poderesEquipados = new List<PoderData>();
    private int indiceActual = 0;
    private bool mostrandoExplicacion = false;

    void Update()
    {
        // Mantener C: Mostrar explicación
        if (Input.GetKey(KeyCode.C))
        {
            mostrandoExplicacion = true;
            Debug.Log("Explicación: " + poderesEquipados[indiceActual].explicacion);

            // Cambiar poder mientras mantienes C con flechas
            if (Input.GetKeyDown(KeyCode.LeftArrow)) CambiarPoder(-1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) CambiarPoder(1);
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            mostrandoExplicacion = false;
        }

        // Tap C: Cambiar poder rápido
        if (Input.GetKeyDown(KeyCode.C) && !Input.GetKey(KeyCode.LeftArrow))
        {
            CambiarPoder(1);
        }

        // Tecla X: Usar Poder
        if (Input.GetKeyDown(KeyCode.X) && !mostrandoExplicacion)
        {
            UsarPoderActual();
        }
    }

    void CambiarPoder(int direccion)
    {
        indiceActual = (indiceActual + direccion + poderesEquipados.Count) % poderesEquipados.Count;
        Debug.Log("Poder seleccionado: " + poderesEquipados[indiceActual].nombre);
    }

    void UsarPoderActual()
    {
        PoderData p = poderesEquipados[indiceActual];
        // Aqui conectaremos la logica especifica de cada uno de los 15 poderes
        Debug.Log("Usando: " + p.nombre);
    }
}

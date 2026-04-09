using UnityEngine;

public class MetaNivel : MonoBehaviour  
{

 private bool nivelFinalizado = false;
 private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nivelFinalizado = true;
            animator.SetTrigger("Abrir");

            Debug.Log("Nivel Completado! Calculando Skill...");

            // 1. Le pedimos al SkillManager que haga sus cuentas
            SkillManager.Instance.CalcularResultados();

            // 2. Llamamos a la función que cargará el siguiente nivel después de 2 segundos
            // (Asegúrate de que el nombre sea exacto y sin espacios)
            Invoke("CargarSiguienteNivel", 2f);


        }


    }
}

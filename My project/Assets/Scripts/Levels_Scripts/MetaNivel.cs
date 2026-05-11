using UnityEngine;

public class MetaNivel : MonoBehaviour
{
    private bool yaSeActivo = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !yaSeActivo)
        {
            if (!MonedaColeccionable.TodasLasMonedasRecogidas()) return;

            yaSeActivo = true;

            if (LevelLoader.Instance != null)
            {
                // AVANZAMOS EL PROGRESO AQUÍ
                LevelLoader.Instance.nivelGlobal++;
                LevelLoader.Instance.nivelActualIndice++;

                string dif = SkillManager.Instance != null ? SkillManager.Instance.estadoActual : "Based";
                LevelLoader.Instance.CargarSiguienteNivel(dif);
            }
        }
    }
}
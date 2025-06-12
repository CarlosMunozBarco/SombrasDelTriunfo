using TMPro;
using UnityEngine;

public class Vaccine : MonoBehaviour
{
    // Referencia al spawner de zombis
    public ZombieSpawner spawner;
    // Referencia al texto que muestra la localización de los ingredientes de la vacuna
    public TextMeshProUGUI localizacion;

    /// <summary>
    /// Método que se llama cuando el jugador consigue un ingrediente de la vacuna. Se encarga de actualizar el contador de vacunas tomadas, actualizar la UI y destruir el objeto de la vacuna.
    /// </summary>
    public void takeVaccine()
    {
        // Aumentamos el contador de vacunas tomadas
        GlobalReferences.Instance.vaccinesTaken++;
        // Actualizamos el texto de vacunas recogidas
        HUDManager.Instance.updateVaccines();
        // Generamos una oleada de zombis como respuesta a haber recogido la vacuna
        spawner.SpawnWave();
        // Ponemos el texto de localización en verde, indicando que el jugador ha recogido el ingrediente de la vacuna
        localizacion.color = Color.green;
        // Destruimos el objeto de la vacuna
        Destroy(gameObject);
    }
}

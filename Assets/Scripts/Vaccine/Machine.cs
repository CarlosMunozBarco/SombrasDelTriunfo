using System.Collections;
using UnityEngine;

public class Machine : MonoBehaviour
{
    // Animator del sintetizador
    private Animator animator;
    // Referencia al spawner de zombis
    public ZombieSpawner spawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicializamos el spawner de zombis
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Método público que permite abrir el sintetizador. Se llama cuando el jugador apunta al sintetizador estando cerca de el, siempre y cuando no se esté produciendo la vacuna.
    /// </summary>
    public void OpenMachine()
    {
        if (!GlobalReferences.Instance.vaccineIsReady)
        {
            animator.SetBool("isOpen", true);
        }
        
    }
    /// <summary>
    /// Método público que permite cerrar el sintetizador. Se llama cuando el jugador deja de apuntar al sintetizador o cuando se empieza a producir la vacuna.
    /// </summary>
    /// <param name="zombiesSpawning"></param>
    public void CloseMachine()
    {
        animator.SetBool("isOpen", false);
    }

    /// <summary>
    /// Método que fabrica la vacuna. Se llama cuando el jugador ha tomado los 3 ingredientes necesarios para fabricar la vacuna y pulsa el botón de 'Interactuar' mientras señala al
    /// sintetizador
    /// </summary>
    public IEnumerator makeVaccine()
    {
        // Si el jugador ha recogido los 3 ingredientes necesarios para fabricar la vacuna, se inicia el proceso de fabricación
        if (GlobalReferences.Instance.vaccinesTaken == 3)
        {
            GlobalReferences.Instance.vaccineIsReady = true;
            CloseMachine();
            // Actualizamos el texto de vacunas recogidas para indicar que se está fabricando la vacuna
            HUDManager.Instance.updateVaccines("Vaccine is being made!");
            // Generamos una oleada de zombis para que el jugador tenga que defender el sintetizador mientras se fabrica la vacuna
            spawner.SpawnWave();
            // Esperamos 30 segundos para que se fabrique la vacuna, que será el tiempo que tenga que aguantar el jugador enfrentando la oleada de zombis
            yield return new WaitForSeconds(30f);
            // Una vez transcurridos los 30 segundos, actualizamos el texto de vacunas recogidas para indicar que la vacuna está lista y terminamos el juego
            HUDManager.Instance.updateVaccines("Vaccine is ready!");
            HistoryManager.Instance.StartCoroutine(HistoryManager.Instance.finishGame());
            
        }
    }
}

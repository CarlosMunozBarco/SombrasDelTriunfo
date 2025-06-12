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
    /// M�todo p�blico que permite abrir el sintetizador. Se llama cuando el jugador apunta al sintetizador estando cerca de el, siempre y cuando no se est� produciendo la vacuna.
    /// </summary>
    public void OpenMachine()
    {
        if (!GlobalReferences.Instance.vaccineIsReady)
        {
            animator.SetBool("isOpen", true);
        }
        
    }
    /// <summary>
    /// M�todo p�blico que permite cerrar el sintetizador. Se llama cuando el jugador deja de apuntar al sintetizador o cuando se empieza a producir la vacuna.
    /// </summary>
    /// <param name="zombiesSpawning"></param>
    public void CloseMachine()
    {
        animator.SetBool("isOpen", false);
    }

    /// <summary>
    /// M�todo que fabrica la vacuna. Se llama cuando el jugador ha tomado los 3 ingredientes necesarios para fabricar la vacuna y pulsa el bot�n de 'Interactuar' mientras se�ala al
    /// sintetizador
    /// </summary>
    public IEnumerator makeVaccine()
    {
        // Si el jugador ha recogido los 3 ingredientes necesarios para fabricar la vacuna, se inicia el proceso de fabricaci�n
        if (GlobalReferences.Instance.vaccinesTaken == 3)
        {
            GlobalReferences.Instance.vaccineIsReady = true;
            CloseMachine();
            // Actualizamos el texto de vacunas recogidas para indicar que se est� fabricando la vacuna
            HUDManager.Instance.updateVaccines("Vaccine is being made!");
            // Generamos una oleada de zombis para que el jugador tenga que defender el sintetizador mientras se fabrica la vacuna
            spawner.SpawnWave();
            // Esperamos 30 segundos para que se fabrique la vacuna, que ser� el tiempo que tenga que aguantar el jugador enfrentando la oleada de zombis
            yield return new WaitForSeconds(30f);
            // Una vez transcurridos los 30 segundos, actualizamos el texto de vacunas recogidas para indicar que la vacuna est� lista y terminamos el juego
            HUDManager.Instance.updateVaccines("Vaccine is ready!");
            HistoryManager.Instance.StartCoroutine(HistoryManager.Instance.finishGame());
            
        }
    }
}

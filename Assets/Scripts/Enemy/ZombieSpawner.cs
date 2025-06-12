using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // Cantidad de zombis que van a aparecer cuando se active el spawn
    public int totalZombiesSpawning = 5;
    // Cantidad de zombis que van a aparecer cuando el jugador se acerque al ingrediente o sintetizador
    public int initialZombiesSpawning = 5;
    // Tiempo de espera entre el spawn de cada zombi
    public float spawnDelay = 0.1f;
    // Prefab del zombi que se va a instanciar
    public GameObject zombiePrefab;

    // Referencia al jugador
    private GameObject player;
    // Variable para controlar si la oleada inicial ya ha sido activada
    private bool initialWaveSpawned = true;

    public void Start()
    {
        // Inicialización de variables
        player = GameObject.FindGameObjectWithTag("Player");

        // Permitimos la generación de zombis tras 5 segundos desde que se inicia el juego. Esperamos este tiempo
        // para que, en caso de que el jugador haya iniciado el juego demasiado cerca de alguna de las localizacíones clave, le de tiempo
        // a las herramientas del Geospatial a situar los elementos de la escena correctamente en el mundo real
        StartCoroutine(enableInitialWave());
    }

    public void Update()
    {
        // Si el jugador se acerca a mas de 100 metros del spawner, se activa la oleada inicial de zombis 
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer < 100 && !initialWaveSpawned && HistoryManager.Instance.gameplayStarted)
        {
            initialWaveSpawned = true;
            StartCoroutine(startWave(initialZombiesSpawning));
        }
    }

    /// <summary>
    /// Método que se llama cuando el jugador toma un ingrediente de la vacuna o activa el sintetizador. Sirve para generar una oleada de zombis.
    /// </summary>
    public void SpawnWave()
    {
        // Start the coroutine to spawn zombies
        StartCoroutine(startWave(totalZombiesSpawning));
    }

    /// <summary>
    /// Método privado que permite generar una oleada de zombis en el lugar donde se encuentre el spawner.
    /// </summary>
    /// <param name="zombiesSpawning"></param>
    private IEnumerator startWave(int zombiesSpawning)
    {
        for (int i = 0; i < zombiesSpawning; i++)
        {
            // Generamos un offset aleatorio para que los zombis no aparezcan todos en el mismo lugar
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Instanciamos el zombi
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    /// <summary>
    /// Método que permite la generación de la oleada inicial
    /// </summary>
    /// <returns></returns>
    private IEnumerator enableInitialWave()
    {
        yield return new WaitForSeconds(5f);
        initialWaveSpawned = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HistoryManager : MonoBehaviour
{
    // Instancia para el patrón Singleton
    public static HistoryManager Instance { get; set; }

    // Referencia a la interfaz normal del juego
    public GameObject normalUI;
    // Referecnia a la interfaz explicativa del juego
    public GameObject firstScreen;

    // Variable que controla si el jugador ha iniciado el juego (pulsando el botón empezar)
    public bool gameplayStarted = false;

    // Referencia al soundManager. En este caso el objetivo de esta referencia es poder desactivarlo mientras el jugador lee la pantalla inicial,
    // por lo que no nos sirve acceder a su instancia de Singleton
    public GameObject soundManager;

    // Método awake para inicializar la instancia del Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Desactivamos la interfaz normal del juego y el sound manager al inicio
        normalUI.SetActive(false);
        soundManager.SetActive(false);
    }

    /// <summary>
    /// Método que se llama cuando el jugador pulsa el botón "Empezar" en la pantalla inicial del juego. Cierra la pantalla inicial y muestra la interfaz normal del juego.
    /// </summary>
    public void closeFirstScreen()
    {
        // Cerramos la panalla inicial
        firstScreen.SetActive(false);
        // Mostramos la interfaz normal
        normalUI.SetActive(true);
        // Activamos el sonido del juego
        soundManager.SetActive(true); 
        // Iniciamos el juego
        gameplayStarted = true; 
    }

    /// <summary>
    /// Método que se llama cuando el jugador ha completado el juego. Muestra una pantalla de finalización y vuelve al menú principal tras un fundido a negro.
    /// </summary>
    public IEnumerator finishGame()
    {
        GetComponent<ScreenFader>().StartFade();

        yield return new WaitForSeconds(5f); // Wait for the fade duration

        SceneManager.LoadScene("MainMenu");
    }
}

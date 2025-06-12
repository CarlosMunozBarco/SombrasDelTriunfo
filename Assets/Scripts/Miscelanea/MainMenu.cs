using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nombre de la escena donde llevaremos al jugador tras pulsar el botón 'Empezar'
    public string newGameScene = "ZombieKiller";

    // AudioClip y canal de audio relacionados con el sonido de fondo
    public AudioClip bg_music;
    public AudioSource main_channel;

    // Start is called before the first frame update
    void Start()
    {
        // Reproducimos la música de fondo al iniciar el juego
        main_channel.clip = bg_music;
        main_channel.loop = true;
        main_channel.Play();
    }

    /// <summary>
    /// Método que se llama cuando el jugador pulsa el botón "Empezar" en el menú principal. Carga la escena del juego y detiene la música de fondo.
    /// </summary>
    public void StartNewGame()
    {
        main_channel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    /// <summary>
    /// Método que se llama cuando el jugador pulsa el botón "Salir" en el menú principal. Cierra la aplicación.
    /// </summary>
    /// <param name="zombiesSpawning"></param>
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

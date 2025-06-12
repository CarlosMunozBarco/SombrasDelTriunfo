using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Vida total del jugador, si llega a 0, el jugador muere
    public int HP = 100;

    // Referencias a los elementos de la UI.

    // Pantalla de sangre que aparece cuando el jugador recibe daño
    public GameObject bloodyScreen;
    // Referencia a la UI de salud del jugador
    public TextMeshProUGUI playerHealthUI;
    // Referencia a la UI de Game Over que aparece cuando el jugador muere
    public GameObject gameOverUI;

    // Booleana que controla si el jugador ha muerto o no
    public bool isDead = false;

    // Tiempo de invulnerabilidad después de recibir daño.
    public float invulnerableTime = 1f;
    private float lastTimeHit;


    private void Start()
    {
        // Inicializamos la UI de salud del jugador
        playerHealthUI.text = $"Health: {HP}";
    }

    /// <summary>
    /// Método que permite al jugador recibir daño. 
    /// </summary>
    /// <param name="damageAmount">Cantidad de daño que va a recibir el jugador.</param>
    public void TakeDamage(int damageAmount)
    {
        // Si el jugador está en tiempo de invulnerabilidad, no recibe daño
        if (Time.time - lastTimeHit < invulnerableTime)
        {
            return;
        }

        lastTimeHit = Time.time;

        // Se reduce la vida del jugador en función del daño recibido
        HP -= damageAmount;

        // Si la vida del jugador es menor o igual a 0, el jugador muere
        if (HP <= 0)
        {
            playerDead();
            isDead = true;
        }
        else
        {
            playerHurt();
        }
    }

    /// <summary>
    /// Método que encapsula la lógica de lo que ocurre cuando el jugador recibe daño.
    /// </summary>
    private void playerHurt()
    {
        // Se muestra la pantalla de sangre y se actualiza la UI de salud del jugador
        StartCoroutine(BloodyScreenEffect());
        playerHealthUI.text = $"Health: {HP}";
        // Se reproduce el sonido de daño del jugador
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
    }

    /// <summary>
    /// Método que encapsula la lógica de lo que ocurre cuando el jugador muere.
    /// </summary>
    private void playerDead()
    {
        // Se reproduce el sonido de muerte del jugador
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

        // Se reproduce la animación de muerte
        GetComponentInChildren<Animator>().enabled = true;
        // Se desactiva la interfaz de salud del jugador
        playerHealthUI.gameObject.SetActive(false);

        // Se produce un fundido a negro mientras se muestra la pantalla de Game Over
        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    /// <summary>
    /// Método que muestra la pantalla de Game Over
    /// </summary>
    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);

        // Se inicia la coroutina para volver al menú principal
        StartCoroutine(ReturnToMainMenu());
    }

    /// <summary>
    /// Método que devuelve al jugador al menú principal
    /// </summary>
    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Método que muestra pantalla de haber recibido daño
    /// </summary>

    private IEnumerator BloodyScreenEffect()
    {
        // Se activa la pantalla de sangre si no lo está ya
        if(bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        // Se reduce la opacidad de la pantalla de sangre de forma gradual
        var image = bloodyScreen.GetComponentInChildren<Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }


        // Se desactiva la pantalla de sangre al finalizar el efecto
        if (bloodyScreen.activeInHierarchy == true)
        {
            bloodyScreen.SetActive(false);
        }
    }

    /// <summary>
    /// Método que gestiona las colisiones del jugador con los objetos del juego. En este caso, se encarga de detectar cuando el jugador recibe daño de un ZombieHand.
    /// Este método es llamado automáticamente por Unity cuando el jugador entra en contacto con un objeto que tiene un Collider y está marcado como Trigger.
    /// </summary>
    /// <param name="other"> El objeto con el que colisiona el jugador. </param>
    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto con el que colisiona el jugador es un ZombieHand, se le aplica daño al jugador.
        if (other.CompareTag("ZombieHand"))
        {
            if (!isDead)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
            
        }
    }
}

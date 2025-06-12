using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Clase Enemy que representa un enemigo en el juego, en este caso un zombi.
/// </summary>
public class Enemy : MonoBehaviour
{
    // Vida del zombi. Cuando esta llegue a 0, el zombi morirá
    [SerializeField] public int HP = 100;
    private Animator animator;

    // NavMeshAgent para gestionar el movimiento del zombi
    private NavMeshAgent navAgent;

    // Booleana que controla si el zombi ha muerto o no
    public bool isDead;
    // El GameObject que usará el zombi para hacer daño al jugador
    public GameObject damageDealer;

    // Cantidad de puntos que otorgará el zombi al morir
    public int pointsGivenForKill = 10;

    /// <summary>
    /// Inicializa las variables de animator y navAgent.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Método que permite al zombi recibir daño.
    /// </summary>
    /// <param name="damageAmount">Cantidad de daño que va a recibir el zombi.</param>
    public void TakeDamage(int damageAmount)
    {
        // Se reducie la vida del zombi en función del daño recibido
        HP -= damageAmount;

        // Si la vida del zombi es menor o igual a 0, el zombi muere
        if (HP <= 0)
        {
            // Se reproduce aleatoriamente una de las dos animaciones de muerte del zombi
            int randomValue = UnityEngine.Random.Range(0, 2); // 0 o 1

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");

            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            isDead = true;

            // Se reproduce el sonido de muerte del zombi
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);

            // Se desactiva el damageDealer del zombi para que no pueda seguir haciendo daño
            damageDealer.gameObject.SetActive(false);

            // Se otorgan al jugador los zombis correspondientes a haber matado al zombi
            GlobalReferences.Instance.playerPoints += pointsGivenForKill;

        }
        else // El zombi sigue vivo
        {
            // Se reproduce la animación de daño del zombi
            animator.SetTrigger("DAMAGE");

            // Se reproduce aleatoriamente uno de los dos sonidos de daño del zombi
            int randomValue = UnityEngine.Random.Range(0, 2); // 0 o 1

            if (randomValue == 0)
            {
                SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt2);
            }
            else
            {
                SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
            }

        }
    }

    // Método que dibuja en la escena las esferas de detección y ataque del zombi
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 2.5f); // Attacking // Stop Attacking

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, 20f); // Detection (Start Chasing)

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, 25f); // Stop Chasing
    //}

    /// <summary>
    /// Método que destruye el zombi. Se llama al final de la animación de muerte.
    /// </summary>
    public void destroyEnemy()
    {
        Destroy(gameObject);
    }
}



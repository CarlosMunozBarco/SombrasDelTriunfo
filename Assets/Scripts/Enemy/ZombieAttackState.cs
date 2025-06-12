using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    // Transform del player
    Transform player;
    // NavMeshAgent del zombi
    NavMeshAgent agent;
    // Variables para controlar el sonido del ataque
    private float lastSoundTime = 0f;
    public float stopAttackingDistance = 2.5f;
    public float soundCooldown = 3f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // --- Inicialización --- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Si ha psado el tiempo suficiente desde el último sonido, reproducimos un sonido de ataque
        if (Time.time - lastSoundTime >= soundCooldown && SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            AudioClip currentSound;
            int randomValue = UnityEngine.Random.Range(0, 2); // 0 or 1
            if (randomValue == 0)
            {
                currentSound = SoundManager.Instance.zombieAttack;
            }
            else
            {
                currentSound = SoundManager.Instance.zombieAttack2;
            }

            SoundManager.Instance.zombieChannel.PlayOneShot(currentSound);
            lastSoundTime = Time.time;
        }

        // El zombi mira al jugador para atacarle
        LookAtPlayer();

        // Comprobamos que el jugador no haya salido del radio de ataque
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    
    /// <summary>
    /// Método que permite al zombi mirar en la dirección donde se encuentre el player
    /// </summary>
    ///
    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

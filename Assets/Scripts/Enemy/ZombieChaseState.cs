using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{

    // Variables para el NavMeshAgent y el Transform del jugador
    NavMeshAgent agent;
    Transform player;

    // Velocidad de persecución y distancias para detenerse y atacar
    public float chaseSpeed = 6f;

    public float stopChasingDistance = 21;
    public float attackingDistance = 2.5f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Inicialización

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Hacemos que el zombi persiga al jugador y mire hacia él
        agent.SetDestination(player.position);
        
        animator.transform.LookAt(player);

        // Comprobamos que el jugador no haya salido del radio de persecución

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        // Comprobamos si el zombi esta lo sucifientemente cerca del jugador para atacar

        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }

        // Reproducimos el sonido de persecución si el zombi no lo está reproduciendo y está a una distancia suficiente del jugador
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
            SoundManager.Instance.zombieChannel.PlayDelayed(15f);

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Frenamos al zombi al salir del estado de persecución

        agent.SetDestination(animator.transform.position);

        //SoundManager.Instance.zombieChannel.Stop();
    }
}

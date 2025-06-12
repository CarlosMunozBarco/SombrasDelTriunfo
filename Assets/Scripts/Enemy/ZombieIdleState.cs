using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    // Variables para almacenar cuanto tiempo lleva el zombi en estado 'IdleState'
    float timer;
    public float idleTime = 0f;

    // Transform del player
    Transform player;

    // Radio del área de detección
    public float detectionArea = 18f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Incialización de variables
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // El zombi pasara a estado 'PatrolingState' tras un tiempo determinado en estado 'IdleState'
        timer += Time.deltaTime;
        if(timer >= idleTime)
        {
            animator.SetBool("isPatroling", true);
        }

        // Comprobamos si el jugador está lo suficientemente cerca como para entrar en el estado 'ChaseState'
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if(distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

}

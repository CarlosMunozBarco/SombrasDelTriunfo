using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{

    // Variables para almacenar cuanto tiempo lleva el zombi en estado 'PatrolingState'
    float timer;
    public float patrolingTime = 0f;

    // Transform del player y NavMeshAgent del zombi
    Transform player;
    NavMeshAgent agent;

    // Radio del área de detección y velocidad de patrullaje
    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    // Lista de los Waypoints que definirán a donde se dirigira el zombi durante el patrullaje
    List<Transform> waypointsList = new List<Transform>();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Inicialización de variables
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();


        agent.speed = patrolSpeed;
        timer = 0;

        // Buscamos el waypointCluster más cercano al zombi
        List<GameObject> waypointClusters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Waypoints"));
        waypointClusters.Sort((a, b) => Vector3.Distance(a.transform.position, animator.transform.position).CompareTo(Vector3.Distance(b.transform.position, animator.transform.position)));
        GameObject waypointCluster = waypointClusters[0];

        foreach (Transform child in waypointCluster.transform)
        {
            waypointsList.Add(child);
        }

        // Mandamos al zombi a un waypoint aleatorio de la lista de waypoints
        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].transform.position;
        agent.SetDestination(nextPosition);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Si no se esta reproduciendo ya algún otro sonido, reproducimos uno aleatorio
        if(SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            int randomValue = UnityEngine.Random.Range(0, 2); // 0 or 1

            if (randomValue%2 == 0)
            {
                SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            }
            else
            {
                SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking2;
            }

            SoundManager.Instance.zombieChannel.PlayDelayed(5f);

        }

        // Comprobamos si el zombi ha llegado al waypoint al que se dirigía y le asignamos uno nuevo como destino

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].transform.position;
            agent.SetDestination(nextPosition);
            //animator.transform.LookAt(nextPosition);
        }

        // El zombi pasará a IdleState trás pasar un tiempo patrullando

        timer += Time.deltaTime;
        if(timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        // Comprobamos si el jugador está lo suficientemente cerca como para entrar en el estado 'ChaseState'
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Frenamos al zombi al salir del estado de patrullaje

        agent.SetDestination(animator.transform.position);

        // Detenemos el sonido de patrullaje del zombi
        SoundManager.Instance.zombieChannel.Stop();
    }
}

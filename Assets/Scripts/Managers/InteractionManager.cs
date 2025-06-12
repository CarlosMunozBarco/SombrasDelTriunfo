using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // Instancia para el patrón Singleton
    public static InteractionManager Instance { get; set; }

    // Variables que almacenan cuál es el objeto actualmente seleccionado por el jugador para interactuar con él
    public WeaponScript hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public GameObject hoveredVaccine = null;
    public Machine hoveredMachine = null;

    // Referencia al botón de recoger que se muestra cuando el jugador puede interactuar con un objeto
    public GameObject pickButton;

    // Referencia al player
    public GameObject player;

    // Variable que comprueba si el jugador está seleccionando algo
    private bool somethingIsHovered = false;

    // Método Awake para inicializar la instancia del Singleton.
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

    private void Start()
    {
        // Inicialización de variables
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Raycast que emerge del centro de la pantalla
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Comprobamos si ha impactado con algo
        
        if (Physics.Raycast(ray, out hit))
        {
            // En caso afirmativo, almacenamos dicho objeto en una variable para poder interactuar con él
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // Si el jugador está lo suficientemente cerca del objeto, le permitimos interactuar con el
            float distanceFromPlayer = Vector3.Distance(player.transform.position, objectHitByRaycast.transform.position);
            if(distanceFromPlayer > 20f)
            {
                pickButton.SetActive(false);
                return;
            }


            // Si el objeto con el que ha impactado el raycast es un ingrediente, podemos recogerlo
            if (objectHitByRaycast.CompareTag("Vaccine"))
            {
                somethingIsHovered = true;

                // Hacemos visible el botón de 'Interactuar'
                pickButton.SetActive(true);

                // Desactivamos el outline del ingrediente anteriormente seleccionado
                if (hoveredVaccine)
                {
                    hoveredVaccine.GetComponent<Outline>().enabled = false;
                }

                // Activamos el outline del nuevo ingrediente seleccionado
                hoveredVaccine = objectHitByRaycast;
                hoveredVaccine.GetComponent<Outline>().enabled = true;

                // En caso de que el jugador pulse el botón de 'Interactuar', recogemos el ingrediente
                if (SimpleInput.GetButtonDown("Pick"))
                {
                    hoveredVaccine.GetComponent<Vaccine>().takeVaccine();
                }
            }
            else 
            {
                // En caso de haber seleccionado otro objeto que no sea un ingrediente, desactivamos el outline del ingrediente anteriormente seleccionado
                if (hoveredVaccine)
                {
                    hoveredVaccine.GetComponent<Outline>().enabled = false;
                }
            }

            // Si le objeto con el que ha impactado el raycast es una caja de munición, podemos activarla
            if (objectHitByRaycast.GetComponent<Machine>())
            {
                somethingIsHovered = true;

                // Hacemos visible el botón de 'Interactuar'
                pickButton.SetActive(true);

                // Desactivamos el outline del sintetizador anteriormente seleccionada
                if (hoveredMachine)
                {
                    hoveredMachine.GetComponent<Outline>().enabled = false;
                }

                // Activamos el outline de la nueva máquina seleccionada
                hoveredMachine = objectHitByRaycast.GetComponent<Machine>();
                hoveredMachine.GetComponent<Outline>().enabled = true;
                hoveredMachine.OpenMachine();

                // En caso de que el jugador pulse el botón de 'Interactuar', activamos el sintetizador
                if (SimpleInput.GetButtonDown("Pick"))
                {
                    StartCoroutine(hoveredMachine.makeVaccine());
                }
            }
            else
            {
                // En caso de haber seleccionado otro objeto que no sea el sintetizador, desactivamos el outline del sintetizador
                if (hoveredMachine)
                {
                    hoveredMachine.GetComponent<Outline>().enabled = false;
                    hoveredMachine.CloseMachine();
                }
            }

            // Si no estamos seleccionando nada interactuable, desactivamos el botón de 'Interactuar'
            if (!objectHitByRaycast.CompareTag("Vaccine") && !objectHitByRaycast.GetComponent<Machine>())
            {
                somethingIsHovered = false;
            }
        }

        if(somethingIsHovered == false)
        {
            pickButton.SetActive(false);
        }

    }
}

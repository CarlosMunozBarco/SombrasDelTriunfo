using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    // Instancia para el patr�n Singleton
    public static GlobalReferences Instance { get; set; }

    // Referencia al efecto especial de salpicadura de sangre
    public GameObject bloodSprayEffect;

    // Puntos disponibles para gastar en la tienda
    public double playerPoints = 1000;
    // N�mero de ingredientes de la vacuna recogidos
    public int vaccinesTaken = 0;
    // Variable que controla si ya se ha fabricado la vacuna
    public bool vaccineIsReady = false;

    // M�todo awake para inicializar la instancia del Singleton
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }
}

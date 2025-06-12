using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    // Tiempo que tardar� el objeto en autodestruirse
    public float timeForDestruction;
    
    // Start is called before the first frame update
    void Start()
    {
        // Iniciamos una corrutina que destruir� el objeto tras un tiempo determinado
        StartCoroutine(DestroySelf(timeForDestruction));
    }

    /// <summary>
    /// M�todo privado que permite destruir el objeto tras un tiempo determinado.
    /// </summary>
    /// <param name="timeForDestruction"></param> Tiempo que tardar� el objeto en autodestruirse
    private IEnumerator DestroySelf(float timeForDestruction)
    {
        yield return new WaitForSeconds(timeForDestruction);
        Destroy(gameObject);
    }

}

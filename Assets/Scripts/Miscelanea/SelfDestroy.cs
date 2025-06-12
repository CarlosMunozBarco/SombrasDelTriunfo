using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    // Tiempo que tardará el objeto en autodestruirse
    public float timeForDestruction;
    
    // Start is called before the first frame update
    void Start()
    {
        // Iniciamos una corrutina que destruirá el objeto tras un tiempo determinado
        StartCoroutine(DestroySelf(timeForDestruction));
    }

    /// <summary>
    /// Método privado que permite destruir el objeto tras un tiempo determinado.
    /// </summary>
    /// <param name="timeForDestruction"></param> Tiempo que tardará el objeto en autodestruirse
    private IEnumerator DestroySelf(float timeForDestruction)
    {
        yield return new WaitForSeconds(timeForDestruction);
        Destroy(gameObject);
    }

}

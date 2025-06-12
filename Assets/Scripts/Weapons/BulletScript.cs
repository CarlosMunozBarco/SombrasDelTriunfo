using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Da�o de la bala. Cuando es disparada, este valor se actualiza con el valor del da�o del arma.
    public int bulletDamage = 10;

    /// <summary>
    /// M�todo que gestiona las colisiones de la bala con otros objetos. En este caso, provoca que los zombis reciban da�o cuando
    /// colisiona contra ellos
    /// </summary>
    /// <param name="objectWeHit"></param> Objeto con el que se colisiona
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            // Si esta vivo, recibe da�o
            if(objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
            {
                objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }

            // Generamos un efecto de salpicadura de sangre al impactar con el zombi
            CreateBloodSprayEffect(objectWeHit);

            // Destruimos la bala
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// M�todo que genera un efecto de salpicadura de sangre
    /// </summary>
    /// <param name="objectWeHit"></param> Objeto con el que se colisiona. En este caso, un zombi.
    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        // Obtenemos el primer punto de contacto de la bala
        ContactPoint contact = objectWeHit.contacts[0];

        // Instanciamos el prefab de la salpicadura de sangre en el punto de contacto y con la rotaci�n adecuada
        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal) //* Quaternion.Euler(0, 180, 0)
            );

        // Establecemos el prefab de la salpicadura de sangre como hijo del objeto con el que hemos colisionado
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }
}

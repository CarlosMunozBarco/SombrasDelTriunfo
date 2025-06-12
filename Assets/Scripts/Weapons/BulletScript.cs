using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Daño de la bala. Cuando es disparada, este valor se actualiza con el valor del daño del arma.
    public int bulletDamage = 10;

    /// <summary>
    /// Método que gestiona las colisiones de la bala con otros objetos. En este caso, provoca que los zombis reciban daño cuando
    /// colisiona contra ellos
    /// </summary>
    /// <param name="objectWeHit"></param> Objeto con el que se colisiona
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            // Si esta vivo, recibe daño
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
    /// Método que genera un efecto de salpicadura de sangre
    /// </summary>
    /// <param name="objectWeHit"></param> Objeto con el que se colisiona. En este caso, un zombi.
    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        // Obtenemos el primer punto de contacto de la bala
        ContactPoint contact = objectWeHit.contacts[0];

        // Instanciamos el prefab de la salpicadura de sangre en el punto de contacto y con la rotación adecuada
        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal) //* Quaternion.Euler(0, 180, 0)
            );

        // Establecemos el prefab de la salpicadura de sangre como hijo del objeto con el que hemos colisionado
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }
}

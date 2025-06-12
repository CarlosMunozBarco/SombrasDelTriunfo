using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    // Cantidad de munici�n que contiene la caja de munici�n
    public int ammoAmount = 200;
    public AmmoType ammoType;

    // Tipo de munici�n que contiene la caja de munici�n
    public enum AmmoType
    {
        PistolAmmo,
        RifleAmmo
    }

}

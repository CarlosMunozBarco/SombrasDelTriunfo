using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    // Cantidad de munición que contiene la caja de munición
    public int ammoAmount = 200;
    public AmmoType ammoType;

    // Tipo de munición que contiene la caja de munición
    public enum AmmoType
    {
        PistolAmmo,
        RifleAmmo
    }

}

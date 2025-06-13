using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static WeaponScript;

public class WeaponManager : MonoBehaviour
{
    // Instancia para el patrón Singleton
    public static WeaponManager Instance { get; set; }

    // Varaibles para almacenar los slots de armas y cual es el arma 'equipada'
    [Header("Weapon Slots")]
    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;
    public int activeWeaponSlotIndex;

    // Variables para almacenar la munición total del jugador
    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;

    // Variables para almacenar el botón 'Intercambiar arma'
    [Header("GUI")]
    public GameObject switchButton;

    // Variables para el arma y munción iniciales
    [Header("Initial")]
    public GameObject initialWeapon;
    public AmmoBox initialAmmo;

    // Método awake para inicializar la instancia del Singleton
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
        activeWeaponSlot = weaponSlots[0];
        activeWeaponSlotIndex = 0;

        // Ocultamos el botón de intercambio de armas al inicio
        switchButton.SetActive(false);

        // Otorgamos al jugador el arma y munición iniciales
        PickupWeapon(initialWeapon);
        PickupAmmo(initialAmmo);
    }

    private void Update()
    {

        // Comprobamos cual es el arma que estamos usando
        foreach(GameObject weaponSlot in weaponSlots)
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        // Si poseemos ambas armas, mostramos el botón de intercambio de armas
        if (weaponSlots[0].transform.childCount > 0 && weaponSlots[1].transform.childCount > 0)
        {
            switchButton.SetActive(true);
        }
        else
        {
            switchButton.SetActive(false);
        }
        

    }




    #region || ---- Weapon ---- ||
    /// <summary>
    /// Método que permite recoger un arma y añadirla al inventario del jugador. Si el jugador ya tiene un arma equipada, se intercambia por la nueva.
    /// </summary>
    /// <param name="pickedUpWeapon"> El arma recogida</param>
    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        if (weaponSlots[activeWeaponSlotIndex].transform.childCount > 0)
        {
            SwitchActiveSlot();
        }
        AddWeaponIntoActiveSlot(pickedUpWeapon);
        
    }

    /// <summary>
    /// Método auxiliar que añade un arma al slot activo del jugador.
    /// </summary>
    /// <param name="pickedUpWeapon"> El arma recogida.</param>
    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        // Establecemos el arma recogida como hijo del slot activo del jugador
        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        WeaponScript weapon = pickedUpWeapon.GetComponent<WeaponScript>();

        // Colocamos el arma en la posición y rotación correctas dentro del slot
        pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        // Activamos el arma y el animator del arma
        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    /// <summary>
    /// Método que nos permite cambiar el arma activa del jugador. Si el jugador tiene un arma equipada, se desactiva y se activa la nueva arma.
    /// </summary>
    public void SwitchActiveSlot()
    {
        // Desactivamos el arma actual
        if (activeWeaponSlot.transform.childCount > 0)
        {
            WeaponScript currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
            currentWeapon.isActiveWeapon = false;
        }

        // Activamos el otro arma. De esta forma, convertimos el arma 'equipada' en la 'guardada' y viceversa
        activeWeaponSlotIndex = (activeWeaponSlotIndex + 1) % 2;
        activeWeaponSlot = weaponSlots[activeWeaponSlotIndex];

        if(activeWeaponSlot.transform.childCount > 0)
        {
            WeaponScript newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
            newWeapon.isActiveWeapon = true;
        }
    }
    #endregion

    #region || ---- Ammo ---- ||

    /// <summary>
    /// Método que nos permite recoger munición y añadirla al inventario del jugador. Dependiendo del tipo de munición, se añade a la cantidad total de ese tipo de munición del jugador.
    /// </summary>
    /// <param name="ammo"> Tipo de munición recogida. </param>
    internal void PickupAmmo(AmmoBox ammo)
    {
        switch(ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    }

    /// <summary>
    /// Método que permite reducir la cantidad de balas de la reserva del jugador.
    /// </summary>
    /// <param name="bulletsToDrecrease"> El número de balas que va a perder el jugador</param>
    /// <param name="thisWeaponModel"> El modelo del arma al que pertenecen dichas balas </param>
    internal void DecreaseTotalAmmo(int bulletsToDrecrease, WeaponScript.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponScript.WeaponModel.Pistol1911:
                totalPistolAmmo -= bulletsToDrecrease;
                break;
            case WeaponScript.WeaponModel.AK74:
                totalRifleAmmo -= bulletsToDrecrease;
                break;
        }
    }

    /// <summary>
    /// Método que permite comprobar cuántas balas le quedan al jugador de un tipo de arma concreto
    /// </summary>
    /// <param name="thisWeaponModel"> El modelo del arma al que pertenecen las balas</param>
    public int CheckAmmoLeftFor(WeaponScript.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponModel.Pistol1911:
                return totalPistolAmmo;

            case WeaponModel.AK74:
                return totalRifleAmmo;

            default:
                return 0;
        }
    }

    #endregion
}
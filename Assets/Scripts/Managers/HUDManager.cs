using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    // Instancia para el patrón Singleton
    public static HUDManager Instance { get; set; }

    // Elementos de la interfaz de usuario relacionados con la munición
    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    // Elementos de la interfaz de usuario relacionados con las armas
    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    // Sprite para los slots vacíos (es decir, para cuando no hay arma equipada)
    public Sprite emptySlot;

    // Cruceta colocada en el centro de la pantalla para indicar al jugador dónde está apuntando
    public GameObject middleDot;

    // Puntos del jugador
    [Header("Points")]
    public TextMeshProUGUI pointsUI;

    // Texto de la interfaz que indica al jugador el número de vacunas que ha recogido
    [Header("Vaccines")]
    public TextMeshProUGUI vaccinesUI;


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

    public void Update()
    {
        // Comprobamos cuales son las armas 'equipada' y 'guardada' del jugador y las mostramos en la interfaz.
        WeaponScript activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<WeaponScript>();
        WeaponScript unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<WeaponScript>();

        // Si tenemos al menos un arma, actualizamos toda la interfaz relacionada con ella
        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft}";
            totalAmmoUI.text = WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel).ToString();

            WeaponScript.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            // Si tenemos arma 'guardada', mostramos su sprite en el slot correspondiente
            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            // En caso contrario mostramos una interfaz vacía, sin armas
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        // Actualizamos el jugador
        pointsUI.text = $"{GlobalReferences.Instance.playerPoints.ToString("F0")} $";
    }

    /// <summary>
    /// Método que permite actualizar el texto de las vacunas recogidas por el jugador. Si se le pasa algún mensaje,
    /// el texto se actualizará con ese mensaje, en caso contrario se mostrará el número de vacunas recogidas hasta el momento.
    /// </summary>
    /// <param name="msg"> </param> Mensaje a mostrar en el slot de vacunas recogidas
    public void updateVaccines(String msg = "")
    {
        if(msg == "")
        {
            vaccinesUI.text = $"Vaccines Taken: {GlobalReferences.Instance.vaccinesTaken} / 3";
        }
        else
        {
            vaccinesUI.text = msg;
        }
        
    }

    /// <summary>
    /// Método que devuelve el sprite de un arma en función de su modelo. Este sprite se usará para mostrar el arma en la interfaz del jugador.
    /// </summary>
    /// <param name="model"> </param> Modelo del arma cuyo sprite se quiere mostrar.
    /// <returns> El sprite del arma cuyo modelo se ha pasasdo por argumento. </returns>
    private Sprite GetWeaponSprite(WeaponScript.WeaponModel model)
    {
        switch (model)
        {
            case WeaponScript.WeaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;

            case WeaponScript.WeaponModel.AK74:
                return Resources.Load<GameObject>("AK74_Weapon").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    /// <summary>
    /// Método que devuelve el sprite de la munición en función del modelo del arma. Este sprite se usará para mostrar la munición en la interfaz del jugador.
    /// </summary>
    /// <param name="model"> </param> Modelo del arma cuya munición se quiere mostrar.
    /// <returns> El sprite de la munición del arma cuyo modelo se ha pasado por argumento. </returns>
    private Sprite GetAmmoSprite(WeaponScript.WeaponModel model)
    {
        switch (model)
        {
            case WeaponScript.WeaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case WeaponScript.WeaponModel.AK74:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    /// <summary>
    /// Método que comprueba cual es el arma 'guardada' del jugador y devuelve una referencia hacia el slot que la contiene.
    /// </summary>
    /// <returns>Descripción del valor devuelto.</returns>
    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }

        // This will never happen, but we need to return something
        return null;
    }
}

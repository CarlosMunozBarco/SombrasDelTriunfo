using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Instancia para el patr�n Singleton
    public static Shop Instance { get; set; }

    // Variables para almacenar la interfaz normal del juego y la de la tienda
    [Header("UIs")]
    public GameObject normalUI;
    public GameObject shopUI;

    // Variable que indica si la tienda est� abierta o no
    public bool isOpen = false;

    // Posicion del spawn del rifle. Es indiferente, pues el arma es posicionada autom�ticamente 
    // en el slot de armas activo del jugador, pero es necesario tenerlo controlado para asegurarnos de que no se instancie dentro de otro objeto, provocando errores
    [Header("SpawnPositions")]
    public Transform rifleSpawnPosition;

    // Prefabs de los armas
    [Header("Weapons")]
    public GameObject riflePrefab;
    public GameObject pistolPrefab;

    // Precios del rifle
    [Header("WeaponsInfo")]
    public double riflePrize = 20;
    public double pistolDamageUpgradePrize = 10;
    public double pistolAmmoUpgradePrize = 10;

    public double rifleDamageUpgradePrize = 30;
    public double rifleAmmoUpgradePrize = 70;

    // Las interfaces del rifle: la de compra, activa desde el principio, y la de mejora, que se activa cuando el jugador compra el rifle
    public GameObject rifleUpgrade;
    public GameObject rifleBuy;

    // Precios de la pistola
    [Header("Ammo")]
    public double pistolAmmoPrize = 100;
    public double rifleAmmoPrize = 400;

    public AmmoBox pistolAmmoPrefab;
    public AmmoBox rifleAmmoPrefab;

    [Header("Texts")]

    // Pistola
    
    public TextMeshProUGUI pistolDamageUpgradePrizeText;
    public TextMeshProUGUI pistolAmmoUpgradePrizeText;
    public TextMeshProUGUI pistolDamageText;
    public TextMeshProUGUI pistolAmmoText;

    // Rifle
    public TextMeshProUGUI riflePrizeText;
    public TextMeshProUGUI rifleDamageUpgradePrizeText;
    public TextMeshProUGUI rifleAmmoUpgradePrizeText;
    public TextMeshProUGUI rifleDamageText;
    public TextMeshProUGUI rifleAmmoText;

    // Munici�n
    public TextMeshProUGUI pistolAmmoBoughtText;
    public TextMeshProUGUI rifleAmmoBoughtText;

    public TextMeshProUGUI pistolAmmoPrizeText;
    public TextMeshProUGUI rifleAmmoPrizeText;

    // Puntos del jugador
    public TextMeshProUGUI playerPointsText;

    // Las distintas interfaces de la tienda.
    [Header("Pages")]
    public GameObject weaponsPage;
    public GameObject ammoPage;

    // El rifle y la pistola del jugador, para poder aplicar las mejores
    private WeaponScript playerRifle;
    private WeaponScript playerPistol;

    // M�todo Awake para inicializar la instancia del Singleton
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

    // Start is called before the first frame update
    void Start()
    {
        // Desactivamos la interfaz de la tienda
        shopUI.SetActive(false);
        // Obtenemos la referencia a la pistola inicial del jugador
        playerPistol = WeaponManager.Instance.activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();

        // Establecemos los valores iniciales de los textos de la tienda
        pistolDamageText.text = pistolPrefab.GetComponent<WeaponScript>().WeaponDamage.ToString();
        pistolAmmoText.text = pistolPrefab.GetComponent<WeaponScript>().magazineSize.ToString();

        rifleDamageText.text = riflePrefab.GetComponent<WeaponScript>().WeaponDamage.ToString();
        rifleAmmoText.text = riflePrefab.GetComponent<WeaponScript>().magazineSize.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            // Actualizamos los textos de la tienda con los valores actuales
            riflePrizeText.text = $"{riflePrize.ToString("F0")} $";
            pistolDamageUpgradePrizeText.text = $"{pistolDamageUpgradePrize.ToString("F0")} $";
            pistolAmmoUpgradePrizeText.text = $"{pistolAmmoUpgradePrize.ToString("F0")} $";

            rifleDamageUpgradePrizeText.text = $"{rifleDamageUpgradePrize.ToString("F0")} $";
            rifleAmmoUpgradePrizeText.text = $"{rifleAmmoUpgradePrize.ToString("F0")} $";

            pistolAmmoPrizeText.text = $"{pistolAmmoPrize.ToString("F0")} $";
            rifleAmmoPrizeText.text = $"{rifleAmmoPrize.ToString("F0")} $";


            // Actualizamos el texto de los puntos del jugador
            playerPointsText.text = $"{GlobalReferences.Instance.playerPoints.ToString("F0")}$";

            // Actualizamos los valores de la munici�n poseida por el jugador
            pistolAmmoBoughtText.text = WeaponManager.Instance.totalPistolAmmo.ToString();
            rifleAmmoBoughtText.text = WeaponManager.Instance.totalRifleAmmo.ToString();

            // Comprobamos si el jugador ha pulsado el bot�n 'CerrarTienda'
            if (SimpleInput.GetButtonDown("Close"))
            {
                CloseShop();
            }

            // Comprobamos si el jugador ha pulsado el bot�n 'ComprarRifle�
            if (SimpleInput.GetButtonDown("BuyRifle"))
            {
                // Si el jugador tiene suficientes puntos, compramos el rifle
                if (GlobalReferences.Instance.playerPoints >= riflePrize)
                {
                    // Reducimos el n�mero de puntos del jugador en base al precio del rifle
                    GlobalReferences.Instance.playerPoints -= riflePrize;
                    // Intercambiamos la interfaz de compra del rifle por la de mejora
                    rifleBuy.SetActive(false);
                    rifleUpgrade.SetActive(true);
                    // Otorgamos el rifle al jugador
                    WeaponManager.Instance.PickupWeapon(Instantiate(riflePrefab, rifleSpawnPosition.position, Quaternion.Euler(0, 180, -90)));
                    playerRifle = WeaponManager.Instance.activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
                }
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a comprar mejoras para el da�o de la pistola
            if (SimpleInput.GetButtonDown("BuyPistolDamageUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= pistolDamageUpgradePrize)
                {
                    // Reducimos el n�mero de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= pistolDamageUpgradePrize;

                    // Aumentamos el da�o del arma
                    playerPistol.WeaponDamage += 5;
                    pistolDamageText.text = playerPistol.WeaponDamage.ToString();

                    // Incrementamos el precio de la mejora
                    pistolDamageUpgradePrize = pistolDamageUpgradePrize*1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a comprar mejoras para la munici�n de la pistola
            if (SimpleInput.GetButtonDown("BuyPistolAmmoUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= pistolAmmoUpgradePrize)
                {
                    // Reducimos el n�mero de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= pistolAmmoUpgradePrize;

                    // Aumentamos el tama�o del cargador de la pistola
                    playerPistol.magazineSize += 2;
                    pistolAmmoText.text = playerPistol.magazineSize.ToString();

                    // Incrementamos el precio
                    pistolAmmoUpgradePrize = pistolAmmoUpgradePrize * 1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a comprar el rifle
            if (SimpleInput.GetButtonDown("BuyRifleDamageUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= rifleDamageUpgradePrize)
                {
                    // Reducimos el n�mero de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= rifleDamageUpgradePrize;

                    // Aumentamos el da�o del rifle
                    playerRifle.WeaponDamage += 3;
                    rifleDamageText.text = playerRifle.WeaponDamage.ToString();

                    // Incrementamos el precio
                    rifleDamageUpgradePrize = rifleDamageUpgradePrize * 1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a comprar mejoras para la munici�n del rifle
            if (SimpleInput.GetButtonDown("BuyRifleAmmoUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= rifleAmmoUpgradePrize)
                {
                    // Reducimos el n�mero de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= rifleAmmoUpgradePrize;
                    // Aumentamos el tama�o del cargador del rifle
                    playerRifle.magazineSize += 5;
                    rifleAmmoText.text = playerRifle.magazineSize.ToString();

                    // Incrementamos el precio
                    rifleAmmoUpgradePrize = rifleAmmoUpgradePrize * 1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a comprar munici�n de la pistola
            if (SimpleInput.GetButtonDown("BuyPistolAmmo"))
            {
                // Si el jugador tiene suficientes puntos, compramos la munici�n
                if (GlobalReferences.Instance.playerPoints >= pistolAmmoPrize)
                {
                    // Reducimos el n�mero de puntos del jugador en base al precio de la munici�n
                    GlobalReferences.Instance.playerPoints -= pistolAmmoPrize;
                    // A�adimos munici�n de pistola al inventario del jugador
                    WeaponManager.Instance.PickupAmmo(pistolAmmoPrefab);
                    pistolAmmoBoughtText.text = (int.Parse(pistolAmmoBoughtText.text) + 50).ToString();
                }
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a comprar munici�n del rifle
            if (SimpleInput.GetButtonDown("BuyRifleAmmo"))
            {
                // Si el jugador tiene suficientes puntos, compramos la munici�n
                if (GlobalReferences.Instance.playerPoints >= rifleAmmoPrize)
                {
                    // Reducimos el n�mero de puntos del jugador en base al precio de la munici�n
                    GlobalReferences.Instance.playerPoints -= rifleAmmoPrize;
                    // A�adimos munici�n de rifle al inventario del jugador
                    WeaponManager.Instance.PickupAmmo(rifleAmmoPrefab);
                    rifleAmmoBoughtText.text = (int.Parse(rifleAmmoBoughtText.text) + 200).ToString();
                }
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a ir a la p�gina de armas. En caso afirmativo, 
            // desactivamos la interfaz de la tienda de armas y activamos la de munici�n
            if (SimpleInput.GetButtonDown("GoWeapons"))
            {
                weaponsPage.SetActive(true);
                ammoPage.SetActive(false);
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a ir a la p�gina de munici�n. En caso afirmativo,
            // desactivamos la interfaz de la tienda de munici�n y activamos la de armas
            if (SimpleInput.GetButtonDown("GoAmmo"))
            {
                weaponsPage.SetActive(false);
                ammoPage.SetActive(true);
            }

            // Comprobamos si el jugador ha pulsado el bot�n asignado a cerrar la tienda. En caso afirmativo, cerramos la tienda
            if (SimpleInput.GetButtonDown("Close"))
            {
                CloseShop();
            }
        }
        else
        {
            // Si la tienda no est� abierta, comprobamos si el jugador ha pulsado el bot�n asignado a abrir la tienda. En caso afirmativo, la abrimos
            if (SimpleInput.GetButtonDown("Shop"))
            {
                OpenShop();
            }
        }

    }

    /// <summary>
    /// M�todo privado que permite abrir la tienda. Esto consiste en activar la interfaz de la tienda y desactivar la interfaz normal del juego.
    /// </summary>
    private void OpenShop()
    {
        isOpen = true;
        shopUI.SetActive(true);
        normalUI.SetActive(false);
    }

    /// <summary>
    /// M�todo privado que permite cerrar la tienda. Esto consiste en desactivar la interfaz de la tienda y activar la interfaz normal del juego.
    /// </summary>
    private void CloseShop()
    {
        if(isOpen)
        {
            isOpen = false;
            shopUI.SetActive(false);
            normalUI.SetActive(true);

        }
    }
}

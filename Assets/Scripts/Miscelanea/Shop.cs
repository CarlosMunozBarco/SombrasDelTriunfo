using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Instancia para el patrón Singleton
    public static Shop Instance { get; set; }

    // Variables para almacenar la interfaz normal del juego y la de la tienda
    [Header("UIs")]
    public GameObject normalUI;
    public GameObject shopUI;

    // Variable que indica si la tienda está abierta o no
    public bool isOpen = false;

    // Posicion del spawn del rifle. Es indiferente, pues el arma es posicionada automáticamente 
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

    // Munición
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

    // Método Awake para inicializar la instancia del Singleton
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

            // Actualizamos los valores de la munición poseida por el jugador
            pistolAmmoBoughtText.text = WeaponManager.Instance.totalPistolAmmo.ToString();
            rifleAmmoBoughtText.text = WeaponManager.Instance.totalRifleAmmo.ToString();

            // Comprobamos si el jugador ha pulsado el botón 'CerrarTienda'
            if (SimpleInput.GetButtonDown("Close"))
            {
                CloseShop();
            }

            // Comprobamos si el jugador ha pulsado el botón 'ComprarRifle¡
            if (SimpleInput.GetButtonDown("BuyRifle"))
            {
                // Si el jugador tiene suficientes puntos, compramos el rifle
                if (GlobalReferences.Instance.playerPoints >= riflePrize)
                {
                    // Reducimos el número de puntos del jugador en base al precio del rifle
                    GlobalReferences.Instance.playerPoints -= riflePrize;
                    // Intercambiamos la interfaz de compra del rifle por la de mejora
                    rifleBuy.SetActive(false);
                    rifleUpgrade.SetActive(true);
                    // Otorgamos el rifle al jugador
                    WeaponManager.Instance.PickupWeapon(Instantiate(riflePrefab, rifleSpawnPosition.position, Quaternion.Euler(0, 180, -90)));
                    playerRifle = WeaponManager.Instance.activeWeaponSlot.transform.GetChild(0).GetComponent<WeaponScript>();
                }
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a comprar mejoras para el daño de la pistola
            if (SimpleInput.GetButtonDown("BuyPistolDamageUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= pistolDamageUpgradePrize)
                {
                    // Reducimos el número de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= pistolDamageUpgradePrize;

                    // Aumentamos el daño del arma
                    playerPistol.WeaponDamage += 5;
                    pistolDamageText.text = playerPistol.WeaponDamage.ToString();

                    // Incrementamos el precio de la mejora
                    pistolDamageUpgradePrize = pistolDamageUpgradePrize*1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a comprar mejoras para la munición de la pistola
            if (SimpleInput.GetButtonDown("BuyPistolAmmoUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= pistolAmmoUpgradePrize)
                {
                    // Reducimos el número de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= pistolAmmoUpgradePrize;

                    // Aumentamos el tamaño del cargador de la pistola
                    playerPistol.magazineSize += 2;
                    pistolAmmoText.text = playerPistol.magazineSize.ToString();

                    // Incrementamos el precio
                    pistolAmmoUpgradePrize = pistolAmmoUpgradePrize * 1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a comprar el rifle
            if (SimpleInput.GetButtonDown("BuyRifleDamageUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= rifleDamageUpgradePrize)
                {
                    // Reducimos el número de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= rifleDamageUpgradePrize;

                    // Aumentamos el daño del rifle
                    playerRifle.WeaponDamage += 3;
                    rifleDamageText.text = playerRifle.WeaponDamage.ToString();

                    // Incrementamos el precio
                    rifleDamageUpgradePrize = rifleDamageUpgradePrize * 1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a comprar mejoras para la munición del rifle
            if (SimpleInput.GetButtonDown("BuyRifleAmmoUpgrade"))
            {
                // Si el jugador tiene suficientes puntos, compramos la mejora
                if (GlobalReferences.Instance.playerPoints >= rifleAmmoUpgradePrize)
                {
                    // Reducimos el número de puntos del jugador en base al precio de la mejora
                    GlobalReferences.Instance.playerPoints -= rifleAmmoUpgradePrize;
                    // Aumentamos el tamaño del cargador del rifle
                    playerRifle.magazineSize += 5;
                    rifleAmmoText.text = playerRifle.magazineSize.ToString();

                    // Incrementamos el precio
                    rifleAmmoUpgradePrize = rifleAmmoUpgradePrize * 1.2;
                }
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a comprar munición de la pistola
            if (SimpleInput.GetButtonDown("BuyPistolAmmo"))
            {
                // Si el jugador tiene suficientes puntos, compramos la munición
                if (GlobalReferences.Instance.playerPoints >= pistolAmmoPrize)
                {
                    // Reducimos el número de puntos del jugador en base al precio de la munición
                    GlobalReferences.Instance.playerPoints -= pistolAmmoPrize;
                    // Añadimos munición de pistola al inventario del jugador
                    WeaponManager.Instance.PickupAmmo(pistolAmmoPrefab);
                    pistolAmmoBoughtText.text = (int.Parse(pistolAmmoBoughtText.text) + 50).ToString();
                }
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a comprar munición del rifle
            if (SimpleInput.GetButtonDown("BuyRifleAmmo"))
            {
                // Si el jugador tiene suficientes puntos, compramos la munición
                if (GlobalReferences.Instance.playerPoints >= rifleAmmoPrize)
                {
                    // Reducimos el número de puntos del jugador en base al precio de la munición
                    GlobalReferences.Instance.playerPoints -= rifleAmmoPrize;
                    // Añadimos munición de rifle al inventario del jugador
                    WeaponManager.Instance.PickupAmmo(rifleAmmoPrefab);
                    rifleAmmoBoughtText.text = (int.Parse(rifleAmmoBoughtText.text) + 200).ToString();
                }
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a ir a la página de armas. En caso afirmativo, 
            // desactivamos la interfaz de la tienda de armas y activamos la de munición
            if (SimpleInput.GetButtonDown("GoWeapons"))
            {
                weaponsPage.SetActive(true);
                ammoPage.SetActive(false);
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a ir a la página de munición. En caso afirmativo,
            // desactivamos la interfaz de la tienda de munición y activamos la de armas
            if (SimpleInput.GetButtonDown("GoAmmo"))
            {
                weaponsPage.SetActive(false);
                ammoPage.SetActive(true);
            }

            // Comprobamos si el jugador ha pulsado el botón asignado a cerrar la tienda. En caso afirmativo, cerramos la tienda
            if (SimpleInput.GetButtonDown("Close"))
            {
                CloseShop();
            }
        }
        else
        {
            // Si la tienda no está abierta, comprobamos si el jugador ha pulsado el botón asignado a abrir la tienda. En caso afirmativo, la abrimos
            if (SimpleInput.GetButtonDown("Shop"))
            {
                OpenShop();
            }
        }

    }

    /// <summary>
    /// Método privado que permite abrir la tienda. Esto consiste en activar la interfaz de la tienda y desactivar la interfaz normal del juego.
    /// </summary>
    private void OpenShop()
    {
        isOpen = true;
        shopUI.SetActive(true);
        normalUI.SetActive(false);
    }

    /// <summary>
    /// Método privado que permite cerrar la tienda. Esto consiste en desactivar la interfaz de la tienda y activar la interfaz normal del juego.
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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    // Variables que almacenan si el arma está activa y su daño
    public bool isActiveWeapon = false;
    public int WeaponDamage;
    // Daño inicial, antes de conseguir mejoras en la tienda
    public int baseWeaponDamage;

    [Header("Shooting")]
    // Variables relacionadas con el disparo
    public bool isShooting;
    public bool readyToShoot;
    public float shootingDelay = 2f;

    // Variables relacionaads con la dispersión del disparo.
    [Header("Spread")]
    public float spreadIntensity;
    // Dispersión del disparo al apuntar desde la cadera
    public float hipSpreadIntensity;
    // Dispersión del disparo al apuntar con la mira
    public float adsSpreadIntensity;

    // Variables relacionadas con las balas disparadas
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    // Efecto de fuego de disparo
    [Header("Muzzle effect")]
    public GameObject muzzleEfect;

    // Animator del arma
    [Header("Animator")]
    public Animator animator;

    // Variables relacionadas con la recarga del arma
    [Header("Reloading")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public int baseMagazineSize;
    public bool isReloading;

    // Posición inicial del arma cuando se obtiene
    [Header("Weapon position")]
    // Weapons position
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    // Variable que almacena si el jugador está apuntando con la mira (ADS - Aim Down Sights)
    public bool isADS = false;

    // Enumerado para almacenar los distintos tipos de modelos de arma disponibles
    public enum WeaponModel
    {
        Pistol1911,
        AK74
    }

    // Modelo de arma actual
    public WeaponModel thisWeaponModel;

    // Enumerado que almacena los distintos tipos de modo de disparo disponibles
    public enum ShootingMode
    {
        Single,
        Auto
    }

    // Modo de disparo actual
    public ShootingMode currentShootingMode;

    // Metodo Awake para inicializar las variables del arma cuando es instanciada
    private void Awake()
    {
        readyToShoot = true;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;

        WeaponDamage = baseWeaponDamage;
        magazineSize = baseMagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Si el arma esta activa
        if(isActiveWeapon)
        {
           
            
            // Comprobamos si el jugador esta apretando el botón 'Apuntar Arma'
            if (SimpleInput.GetButtonUp("ADS"))
            {
                if(isADS == false)
                {
                    EnterADS();
                }
                else
                {
                    ExitADS();
                }
            }


            // Se desactiva el outline del arma mientras la estemos usando
            GetComponent<Outline>().enabled = false;

            // Comprobamos si el jugador esta apretando el botón de 'Disparar Arma'
            if (currentShootingMode == ShootingMode.Auto)
            {

                isShooting = SimpleInput.GetButton("Shoot");

            }
            else if (currentShootingMode == ShootingMode.Single)
            {
                isShooting = SimpleInput.GetButtonDown("Shoot");

            }

            // En caso de que el arma este lista para disparar, el jugador haya apretado (o este apretando) el botón de disparo, tengamos balas suficientes y no estemos recargando, disparamos el arma
            if (readyToShoot && isShooting && bulletsLeft > 0 && !isReloading)
            {
                FireWeapon();
            }

            // En caso de que el jugador haya apretado el botón de recargar, nos falten balas en el cargador, no estemos recargando y tengamos balas en la reserva, recargamos el arma
            if (SimpleInput.GetButtonDown("Reload") && bulletsLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            // Si el arma ha terminado de disparar, no estamos disparando, no estamos recargando y no nos quedan balas en el cargador, pero tenemos balas en la reserva, recargamos el arma
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0 && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

        }
    }

    /// <summary>
    /// Método que permite al jugador apuntar con el arma
    /// </summary>
    private void EnterADS()
    {
        // Reproducimos la animación de apuntar con el arma
        animator.SetBool("ADS", true);
        isADS = true;
        // Desactivamos el punto central de la pantalla
        HUDManager.Instance.middleDot.SetActive(false);
        // Reducimos la dispersión de disparo
        spreadIntensity = adsSpreadIntensity;
    }

    /// <summary>
    /// Método que permite al jugador dejar de apuntar con el arma
    /// </summary>
    private void ExitADS()
    {
        // Reproducimos la animación de dejar de apuntar con el arma
        animator.SetBool("ADS", false);
        isADS = false;
        // Activamos el punto central de la pantalla
        HUDManager.Instance.middleDot.SetActive(true);
        // Aumentamos la dispersión de disparo
        spreadIntensity = hipSpreadIntensity;
    }

    /// <summary>
    /// Método que permite al jugador disparar con el arma
    /// </summary>
    private void FireWeapon()
    {
        // Reduce el número de balas en el cargador
        bulletsLeft--;

        // Reproduce el efecto de fuego de disparo
        muzzleEfect.GetComponent<ParticleSystem>().Play();

        // Animación de retroceso del arma
        if (isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            animator.SetTrigger("RECOIL");
        }


        // Reproducimos el sonido de disparo del arma
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        // Evitamos que el jugador vuelva a disparar antes de que termine de procesarse este disparo
        readyToShoot = false;
        // Calculamos la dirección de disparo
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // Instanciamos la bala en el bulletSpawn del arma
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // Establecemos el daño de la bala
        BulletScript bul = bullet.GetComponent<BulletScript>();
        bul.bulletDamage = WeaponDamage;

        // Apuntamos la bala hacia la dirección de disparo
        bullet.transform.forward = shootingDirection;

        // Disparamos la bala aplicandole una fuerza a modo de impuslo en la dirección de disparo
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Destruimos la bala después de un tiempo si no ha impactado con nada
        Destroy(bullet, bulletPrefabLifeTime);

        // Iniciamos el proceso para permitir disparar de nuevo despues de un cierto tiempo, que definirá la cadencia del arma
        Invoke("ResetShot", shootingDelay);

    }


    /// <summary>
    /// Método que permite al jugador recargar con el arma
    /// </summary>
    private void Reload()
    {
        // Si el jugador está apuntando con la mira, le obligamos a dejar de apuntar
        if (isADS)
        {
            ExitADS();
        }

        // Reproducimos el sonido de recarga del arma
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        // Reproducimos la animación de recarga del arma
        animator.SetTrigger("RELOAD");
        // Indicamos que el arma esta recargando
        isReloading = true;
        // Iniciamos el proceso para terminar de recargar el arma despues de un cierto tiempo. Este tiempo definirá la velocidad de recarga del arma
        Invoke("ReloadCompleted", reloadTime);
    }

    /// <summary>
    /// Método que permite al arma terminar de recargar
    /// </summary>
    private void ReloadCompleted()
    {
        // Se obtiene el número de balas que quedan en el cargador y se calcula cuantas balas necesitamos para llenarlo
        int bulletsNeeded = magazineSize - bulletsLeft;
        // Recargamos el cargador tanto como sea posible, teniendo en cuenta las balas que nos quedan en la reserva
        if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsNeeded, thisWeaponModel);
        }
        else
        {
            int bulletsLeftInReserve = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            bulletsLeft += bulletsLeftInReserve;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeftInReserve, thisWeaponModel);
        }
        
        // Indicamos que ya no estamos recargando
        isReloading = false;
    }

    /// <summary>
    /// Método que reinicia el estado de disparo del arma, permitiendo al jugador volver a disparar
    /// </summary>
    private void ResetShot()
    {
        readyToShoot = true;
    }

    /// <summary>
    /// Método que calcula la dirección de la bala tras ser disparada, teniendo en cuenta la posición del jugador y la dispersión del disparo.
    /// </summary>
    public Vector3 CalculateDirectionAndSpread()
    {
        // Disparamos un rayo desde el centro de la pantalla para saber a donde esta mirando el jugador
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        // Comprobamos si el rayo ha impactado con algo
        if (Physics.Raycast(ray, out hit))
        {
            // Almacenamos el punto de impacto del rayo como el punto al que estamos disparando
            targetPoint = hit.point;
        }
        else
        {
            // Si estamos disparando al aire, establecemos un punto de destino a 100 unidades en la dirección del rayo
            targetPoint = ray.GetPoint(100);
        }

        // Calculamos la dirección del disparo teniendo en cuenta la posición de la bala y el punto de impacto
        Vector3 direction = targetPoint - bulletSpawn.position;
        // Aplicamos la dispersión del arma modificando la dirección de la bala una cantidad aleatoria pequeña
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Devolvemos la dirección de disparo modificada por la dispersión
        return direction + new Vector3(0, y, z);
    }
}

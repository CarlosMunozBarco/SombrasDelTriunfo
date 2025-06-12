using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponScript;

public class SoundManager : MonoBehaviour
{
    // Instancia para el patrón Singleton
    public static SoundManager Instance { get; set; }

    // Canales y sonidos relacionados con las armas
    [Header("Weapons")]
    public AudioSource ShootingChannel;
    
    public AudioClip AK74Shot;
    public AudioClip P1911Shot;

    public AudioSource reloadSoundAK74;
    public AudioSource reloadSound1911;

    // Canales y sonidos relacionados con los zombis
    [Header("Zombie")]

    public AudioClip zombieWalking;
    public AudioClip zombieWalking2;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieAttack2;
    public AudioClip zombieHurt;
    public AudioClip zombieHurt2;
    public AudioClip zombieDeath;

    // Canal asociado a la máquina de estados del zombi
    public AudioSource zombieChannel;

    // Canal asociado al resto de sonidos
    public AudioSource zombieChannel2;

    // Canales y sonidos relacionados con el jugador
    [Header("Player")]
    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDeath;

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

    /// <summary>
    /// Método que permite reproducir un sonido de disparo según el arma que se le pase como argumento.
    /// </summary>
    /// <param name="weapon"> </param> Modelo del arma que se ha disparado. Puede ser Pistol1911 o AK74.
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shot);
                break;
            case WeaponModel.AK74:
                ShootingChannel.PlayOneShot(AK74Shot);
                break;
        }
    }

    /// <summary>
    /// Método que permite reproducir un sonido de recarga según el arma que se le pase como argumento.
    /// </summary>
    /// <param name="weapon"> </param> Modelo del arma que se ha recargado. Puede ser Pistol1911 o AK74.
    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadSound1911.Play();
                break;
            case WeaponModel.AK74:
                reloadSoundAK74.Play();
                break;
        }
    }


}

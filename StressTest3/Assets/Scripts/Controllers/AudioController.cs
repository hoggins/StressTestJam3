using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
  public class AudioController : MonoBehaviour
  {
    public AudioClip Shoot;
    public AudioClip Reload;
    public AudioClip Freeze;
    public AudioClip Heal;
    public AudioClip MultiShot;
    public AudioClip GreenOrb;
    public AudioClip RedOrb;
    public AudioClip BlueOrb;
    public AudioClip OrbPickup;
    public AudioClip NoBullets;
    public AudioClip Melee;
    public AudioClip DeathBig;
    public AudioClip DeathSmall;
    public AudioClip DeathHero;
    public AudioClip HeroHit;
    
    public AudioSource Common;
    public AudioSource Music;
    public AudioSource MusicMenu;
    
    public static AudioController Instance;
    
    private void Awake()
    {
      if (Instance != null)
      {
        Destroy(gameObject); 
        return;
      }

      Instance = this;
      DontDestroyOnLoad(this);
    }

    public void PlayShoot()
    {
      Common.PlayOneShot(Shoot);
    }
    
    public void PlayDeathSmall()
    {
      Common.PlayOneShot(DeathSmall);
    }
    
    public void PlayDeathBig()
    {
      Common.PlayOneShot(DeathBig);
    }
     
    public void PlayDeathHero()
    {
      Common.PlayOneShot(DeathHero);
    }
    public void PlayHeroHit()
    {
      Common.PlayOneShot(HeroHit);
    }
         public void PlayHeroMelee()
    {
      Common.PlayOneShot(Melee);
    }  
    public void PlayMusic()
    {
      Music.Play();
      StartCoroutine(ChangeVolumeTo(Music, 1f));
    }
    public void PlayFreeze()
    {
      Common.PlayOneShot(Freeze);
    }
    
    
    public void PlayHeal()
    {
      Common.PlayOneShot(Heal);
    }
    
    
    public void PlayMultishot()
    {
      Common.PlayOneShot(MultiShot);
    }
    
    
    public void PlayOrbRed()
    {
      Common.PlayOneShot(RedOrb);
    }
     
    public void PlayOrbGreen()
    {
      Common.PlayOneShot(GreenOrb);
    }
     
    public void PlayOrbBlue()
    {
      Common.PlayOneShot(BlueOrb);
    }
    
    
    public void PlayOrbPickup()
    {
      Common.PlayOneShot(OrbPickup);
    }
    

    public void StopMusic()
    {
      StartCoroutine(ChangeVolumeTo(Music, 0f));
    }

    private IEnumerator ChangeVolumeTo(AudioSource source, float to)
    {
      var from = source.volume;
      var t = 0f;
      while (t <= 1f)
      {
        t += Time.deltaTime;
        source.volume = Mathf.Lerp(from, to, t);
        yield return null;
      }
      
      if(source.volume == 0)
        source.Stop();
    }
  }
}
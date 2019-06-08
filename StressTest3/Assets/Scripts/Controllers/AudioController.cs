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

         private Coroutine _musicCoroutine;
    public void PlayMusic()
    {
      Music.Play();
      Music.volume = 0f;
      
      if(_musicCoroutine != null)
        StopCoroutine(_musicCoroutine);
      
      _musicCoroutine = StartCoroutine(ChangeVolumeTo(Music, 1f, 3f));
    }
    public void StopMusic()
    {
      if(_musicCoroutine != null)
        StopCoroutine(_musicCoroutine);
      _musicCoroutine = StartCoroutine(ChangeVolumeTo(Music, 0f));
    }
         private Coroutine _musicMenuCoroutine;
     public void PlayMusicMenu()
    {
      MusicMenu.Play();
      MusicMenu.volume = 0f;
      if(_musicMenuCoroutine != null)
        StopCoroutine(_musicMenuCoroutine);
      _musicMenuCoroutine = StartCoroutine(ChangeVolumeTo(MusicMenu, 1f));
    }
    public void StopMusicMenu()
    {
      if(_musicMenuCoroutine != null)
        StopCoroutine(_musicMenuCoroutine);
      _musicMenuCoroutine = StartCoroutine(ChangeVolumeTo(MusicMenu, 0f, 0.3f));
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
    



    private IEnumerator ChangeVolumeTo(AudioSource source, float to, float duration = 1f)
    {
      var from = source.volume;
      var t = 0f;
      while (t/duration <= 1f)
      {
        t += Time.deltaTime;
        source.volume = Mathf.Lerp(from, to, t/duration);
        yield return null;
      }
      
      if(source.volume == 0)
        source.Stop();
    }
  }
}
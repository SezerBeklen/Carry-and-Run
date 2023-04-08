using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    public AudioClip ButtonClick, Crash, Collect,horn;
    public static SoundManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
  
    }

    public void ButtonSound()
    {
        source.PlayOneShot(ButtonClick,1f);
    }
    public void CrashSound()
    {
        source.PlayOneShot(Crash, 0.5f);
    }
    public void CollectSound()
    {
        source.PlayOneShot(Collect, 0.5f);
    }
    public void WinSound()
    {
        source.Play();
    }
    public void FailSound()
    {
        UIManager.instance.failsound.Play();
    }
    public void HornSound()
    {
        source.PlayOneShot(horn, 0.5f);
    } 
    
    public void WheelSound()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        
    }

     
}

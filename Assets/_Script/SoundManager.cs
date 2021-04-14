using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private List<AudioClip> clipsFx;
    [SerializeField]List<AudioClip> clipsBG;
    [SerializeField]AudioSource SourceBgMusic;
    [SerializeField]AudioSource SourceSoundfx;

    private void Awake()
    {
       if(instance == null)
       {
            instance = this;
       }
       else
       {
           Destroy(this.gameObject);
       }
         
    }

    public void TocarFx(int index)
    {
        SourceSoundfx.clip = clipsFx[index];
        SourceSoundfx.Play();
          
    }
    public AudioClip GetClipFx(int index)
    {
        return clipsFx[index];
    }
    public void TocarBG(int index)
    {
        SourceBgMusic.clip = clipsBG[index];
        SourceBgMusic.Play();
        
    }

    public void StopSoundBG()
    {   if(SourceBgMusic.isPlaying)
            SourceBgMusic.Stop();
    }
    public void PauseSoundBG()
    {   
        SourceBgMusic.Pause();
    }
    public void ChangeMusicSpeed(float velocity)
    {
        SourceBgMusic.pitch = velocity;
    }
}

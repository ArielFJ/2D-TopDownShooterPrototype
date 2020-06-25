using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /*public AudioClip inGameAudio;
    public AudioClip inMenuAudio;
    public AudioClip startScreenAudio;*/
    public AudioClip generalAudio;
    
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.instance.OnStateChange += PlayAudio;
    }



    void PlayAudio()
    {
        if(GameManager.instance.state == GameState.InGame)
        {
            //audioSource.clip = inGameAudio;
            audioSource.volume = .3f;
        }
        else
        {
            //audioSource.clip = inMenuAudio;
            audioSource.volume = 1f;
        }
        //audioSource.Play();
                
    }
}

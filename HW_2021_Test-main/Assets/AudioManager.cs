using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource bgAudio,soundSource;
    public AudioClip successAudio, failAudio;
    // Start is called before the first frame update
    void Start()
    {
        print(PlayerPrefs.GetInt("Music") + " is music on");
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            bgAudio.Play();
        }
        else
        {
            bgAudio.Pause();
        }

    }
    public void PlaySuccess()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            soundSource.PlayOneShot(successAudio);
        }
    }
    public void PlayFailure()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            soundSource.PlayOneShot(failAudio);
        }
    }
}

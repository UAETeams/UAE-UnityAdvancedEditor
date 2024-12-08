using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource UniversalSource;
    public AudioSource MusicSource;
    void Update()
    {
        if(UniversalSource.isPlaying)
        {
            MusicSource.Pause();
        }
        else
        {
            MusicSource.UnPause();
        }
    }
    public void removeAudio()
    {
        UniversalSource.clip = null;
    }
}

using JetBrains.Annotations;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Master;
    public AudioSource Music;
    public AudioSource SFX;
    public AudioClip Click;
    
    public void ClickSound()
    {
        SFX.PlayOneShot(Click);
    }
}

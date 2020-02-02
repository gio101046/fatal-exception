using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip jumpSound, screamSound, wrongButtonSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("Audio/Jump");
        jumpSound = Resources.Load<AudioClip>("Audio/GirlyScream");
        jumpSound = Resources.Load<AudioClip>("Audio/WrongButton");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "scream":
                audioSrc.PlayOneShot(screamSound);
                break;
            case "wrong":
                audioSrc.PlayOneShot(wrongButtonSound);
                break;
        }
    }
}

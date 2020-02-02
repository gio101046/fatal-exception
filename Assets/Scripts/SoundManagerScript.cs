using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip jumpSound, screamSound, wrongButtonSound, 
        bugLaughSound, drinkSound, eatSound, punchSound, hardPunchSound, 
        slapSound, hardSlapSound, winSound;
    public static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("Audio/Jump");
        screamSound = Resources.Load<AudioClip>("Audio/GirlyScream");
        wrongButtonSound = Resources.Load<AudioClip>("Audio/WrongButton");
        bugLaughSound = Resources.Load<AudioClip>("Audio/BugLaugh");
        drinkSound = Resources.Load<AudioClip>("Audio/Drink");
        eatSound = Resources.Load<AudioClip>("Audio/Eat");
        punchSound = Resources.Load<AudioClip>("Audio/Punch");
        hardPunchSound = Resources.Load<AudioClip>("Audio/HardPunch");
        slapSound = Resources.Load<AudioClip>("Audio/Slap");
        hardSlapSound = Resources.Load<AudioClip>("Audio/HardSlap");
        winSound = Resources.Load<AudioClip>("Audio/Win");

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
            case "bug laugh":
                audioSrc.PlayOneShot(bugLaughSound);
                break;
            case "drink":
                audioSrc.PlayOneShot(drinkSound);
                break;
            case "eat":
                audioSrc.PlayOneShot(eatSound);
                break;
            case "punch":
                audioSrc.PlayOneShot(punchSound);
                break;
            case "hard punch":
                audioSrc.PlayOneShot(hardPunchSound);
                break;
            case "slap":
                audioSrc.PlayOneShot(slapSound);
                break;
            case "hard slap":
                audioSrc.PlayOneShot(hardSlapSound);
                break;
            case "win":
                audioSrc.clip = winSound;
                audioSrc.PlayOneShot(winSound);
                break;

        }
    }
}

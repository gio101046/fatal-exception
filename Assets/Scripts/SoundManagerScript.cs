using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip jumpSound, screamSound, wrongButtonSound, 
        bugLaughSound, drinkSound, eatSound, punchSound, hardPunchSound, 
        slapSound, hardSlapSound, winSound, slamSound;
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
        slamSound = Resources.Load<AudioClip>("Audio/Slam");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        AudioClip sound = null;
        switch (clip)
        {
            case "jump":
                sound = jumpSound;                
                break;
            case "scream":
                sound = screamSound;
                break;
            case "wrong":
                sound = wrongButtonSound;
                break;
            case "bug laugh":
                sound = bugLaughSound;
                break;
            case "drink":
                sound = drinkSound;
                break;
            case "eat":
                sound = eatSound;
                break;
            case "punch":
                sound = punchSound;
                break;
            case "hard punch":
                sound = hardPunchSound;
                break;
            case "slap":
                sound = slapSound;
                break;
            case "hard slap":
                sound = hardSlapSound;
                break;
            case "win":
                sound = winSound;
                break;
            case "slam":
                sound = slamSound;
                break;

        }

        audioSrc.clip = sound;
        audioSrc.PlayOneShot(sound);
    }
}

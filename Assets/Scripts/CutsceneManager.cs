using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    Cutscene cutscene = null;

    void Start()
    {
        if (cutscene != null)
            StartCoroutine(cutscene.Play());
    }
}

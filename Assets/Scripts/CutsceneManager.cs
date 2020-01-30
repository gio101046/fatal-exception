using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    Cutscene cutscene;

    void Start()
    {
        if (cutscene != null)
            StartCoroutine(cutscene.Play());
    }
}

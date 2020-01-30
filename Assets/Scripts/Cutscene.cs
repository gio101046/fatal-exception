using System.Collections;
using UnityEngine;

public abstract class Cutscene : MonoBehaviour
{
    public IEnumerator Play()
    {
        OnSceneStart();
        yield return OnScenePlay();
        OnSceneEnd();
    }

    protected abstract IEnumerator OnScenePlay();

    protected virtual void OnSceneStart() { }

    protected virtual void OnSceneEnd() { }
}

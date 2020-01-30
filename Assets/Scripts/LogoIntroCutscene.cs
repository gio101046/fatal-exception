using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoIntroCutscene : Cutscene
{
    [SerializeField]
    GameObject logoCubic;
    [SerializeField]
    GameObject logoFull;
    [SerializeField]
    SpriteRenderer logoFullRenderer;

    protected override void OnSceneStart()
    {
        logoFull.SetActive(false);
        logoCubic.SetActive(true);
        logoCubic.transform.eulerAngles = Vector3.up * 90f;
    }

    protected override IEnumerator OnScenePlay()
    {
        yield return RotateCube();
        yield return new WaitForSeconds(.2f);
        yield return FadeInLogo();
        yield return new WaitForSeconds(.5f);
    }

    protected override void OnSceneEnd()
    {
        // TODO: Go to the next scene.
    }

    IEnumerator RotateCube()
    {
        const int maxTicks = 80;
        var angle = logoCubic.transform.eulerAngles;

        for (int i = 0; i <= maxTicks; i++)
        {
            var percent = Easing.SineOut(1f * i / maxTicks);
            var angleLerp = new Vector3(
                Mathf.Lerp(angle.x, angle.x + 360f, percent),
                Mathf.Lerp(angle.y, angle.y + 360f, percent),
                Mathf.Lerp(angle.z, angle.z + 360f, percent));
            logoCubic.transform.eulerAngles = angleLerp;
            yield return null;
        }
    }

    IEnumerator FadeInLogo()
    {
        const int maxTicks = 45;
        var clearWhite = new Color(1f, 1f, 1f, 0f);

        logoFull.SetActive(true);

        for (int i = 0; i <= maxTicks; i++)
        {
            var percent = Easing.SineIn(1f * i / maxTicks);
            logoFullRenderer.color = Color.Lerp(clearWhite, Color.white, percent);
            yield return null;
        }
    }
}

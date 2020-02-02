using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIntroCutscene : Cutscene
{
    public SpriteRenderer sceneA;
    public SpriteRenderer sceneB;
    public SpriteRenderer sceneC;
    public Sprite[] sprites;

    public Camera gameCamera;
    private Vector3 basePosition;
    private int shakeTime = 0;

    private void Start()
    {
        basePosition = gameCamera.transform.position;
    }


    private void Update()
    {
        if (shakeTime > 0)
        {
            gameCamera.transform.position = basePosition + new Vector3(
                Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), basePosition.z);
            shakeTime--;

            if (shakeTime == 0)
                gameCamera.transform.position = basePosition;
        }
    }

    void Shake(int time)
    {
        shakeTime = time;
    }

    protected override IEnumerator OnScenePlay()
    {
        yield return TestA();
        yield return TestB();
        yield return TestC();
    }

    protected override void OnSceneEnd()
    {
        var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings > nextIndex)
            SceneManager.LoadScene(nextIndex);
    }

    IEnumerator TestA()
    {
        sceneA.sprite = sprites[1];
        sceneB.sprite = sprites[0];
        sceneC.sprite = sprites[2];

        for (int i = 0; i <= 150; i++)
        {
            var percent = 1f * i / 150;
            sceneA.transform.position = Vector3.Lerp(
                new Vector3(-0.16f, -0.04f, 10f), 
                new Vector3(3.08f, -0.04f, 10f), 
                percent);
            sceneB.transform.position = Vector3.Lerp(
                new Vector3(2.02f, -1.22f, 0f), 
                new Vector3(.4f, -1.22f, 0f), 
                percent);
            sceneC.transform.position = Vector3.Lerp(
                new Vector3(0.38f, -1.08f, 9f),
                new Vector3(3.62f, -1.08f, 9f), 
                percent);
            yield return null;
        }

        yield return new WaitForSeconds(.8f);

        sceneC.sprite = sprites[3];
        Shake(15);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator TestB()
    {
        sceneA.sprite = sprites[5];
        sceneB.sprite = sprites[4];
        sceneC.gameObject.SetActive(false);

        sceneA.transform.position = new Vector3(0f, 0f, 1f);
        sceneB.transform.position = new Vector3(0f, 0f, 0f);

        Shake(30);

        for (int i = 0; i <= 90; i++)
        {
            var test = i % 4 >= 2;
            sceneA.transform.localScale = new Vector3(test ? -1f : 1f, 1f, 1f);
            yield return null;
        }

        sceneA.transform.localScale = Vector3.one;
    }

    IEnumerator TestC()
    {
        sceneA.sprite = sprites[1];
        sceneB.sprite = sprites[0];
        sceneC.sprite = sprites[3];
        sceneC.gameObject.SetActive(true);

        sceneA.transform.position = new Vector3(3.08f, -0.04f, 10f);
        sceneB.transform.position = new Vector3(.4f, -1.22f, 0f);
        sceneC.transform.position = new Vector3(3.62f, -1.08f, 9f);

        yield return new WaitForSeconds(.8f);

        sceneC.sprite = sprites[2];
        sceneB.sprite = sprites[6];
        Shake(20);

        yield return new WaitForSeconds(1.5f);
    }
}

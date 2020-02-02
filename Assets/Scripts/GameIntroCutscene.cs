using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntroCutscene : Cutscene
{
    public SpriteRenderer sceneA;
    public SpriteRenderer sceneB;
    public SpriteRenderer sceneC;
    public Sprite[] sprites;

    public Camera camera;
    private Vector3 basePosition;
    private int shakeTime = 0;

    private void Start()
    {
        basePosition = camera.transform.position;
    }


    private void Update()
    {
        if (shakeTime > 0)
        {
            camera.transform.position = basePosition + new Vector3(
                Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), basePosition.z);
            shakeTime--;

            if (shakeTime == 0)
                camera.transform.position = basePosition;
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
    }

    IEnumerator TestA()
    {
        sceneA.sprite = sprites[0];
        sceneB.sprite = sprites[1];
        sceneC.sprite = sprites[2];

        for (int i = 0; i <= 150; i++)
        {
            var percent = 1f * i / 150;
            sceneA.transform.position = Vector3.Lerp(
                new Vector3(-0.16f, -0.04f, 0f), 
                new Vector3(3.08f, -0.04f, 0f), 
                percent);
            sceneB.transform.position = Vector3.Lerp(
                new Vector3(1.5f, -1.22f, -2f), 
                new Vector3(-2.2f, -1.22f, -2f), 
                percent);
            sceneC.transform.position = Vector3.Lerp(
                new Vector3(0.38f, -1.08f, -1f),
                new Vector3(3.62f, -1.08f, -1f), 
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
        sceneA.sprite = sprites[4];
        sceneB.sprite = sprites[5];
        sceneC.gameObject.SetActive(false);

        sceneA.transform.position = new Vector3(0f, 0f, 0f);
        sceneB.transform.position = new Vector3(0f, 0f, -10f);

        Shake(60);

        for (int i = 0; i <= 60; i++)
        {
            sceneA.transform.localScale.Set(i % 4 >= 2 ? -1f : 1f, 1f, 1f);
            yield return null;
        }
    }
}

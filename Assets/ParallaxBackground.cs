using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Camera camera;
    public SpriteRenderer renderer;
    public Vector2 scrollRate = Vector2.one * .4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //var currentPos = 

        //var offset = new Vector3(8f, -7.5f, 0f);

        //renderer.transform.position = camera.transform.position + ((camera.transform.position * new Vector3(scrollRate.x, scrollRate.y, 0f)) - offset);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Camera baseCamera;
    [SerializeField] SpriteRenderer baseRenderer;
    public Vector2 scrollRate = Vector2.one * .4f;

    Vector2 tileSize;
    Vector2 offset = new Vector2(8f, 7.5f);

    // Start is called before the first frame update
    void Start()
    {
        baseRenderer.transform.position = new Vector3(
            baseCamera.transform.position.x - offset.x,
            baseCamera.transform.position.y - offset.y, 
            baseRenderer.transform.position.z);

        tileSize = baseRenderer.size;

        baseRenderer.size = new Vector2(
            (Mathf.Ceil(tileSize.x / (offset.x * 2f)) + 2f) * tileSize.x,
            (Mathf.Ceil(tileSize.y / (offset.y * 2f)) + 2f) * tileSize.y);
    }

    // Update is called once per frame
    void Update()
    {
        var depth = baseRenderer.transform.position.z;
        var basePosition = (Vector2)baseCamera.transform.position - (offset * new Vector2(1f, -1f));
        var scrollOffset = baseCamera.transform.position * scrollRate;
        scrollOffset = new Vector2(scrollOffset.x % tileSize.x, scrollOffset.y % tileSize.y);

        var position = basePosition - scrollOffset - tileSize * new Vector2(1, -1);
        baseRenderer.transform.position = new Vector3(position.x, position.y, depth);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventControlsDisplay : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Tile tile;
    [SerializeField] private float framesPerControlTile = 15;
    [SerializeField] private int nextControlYOffset = 1;
    [SerializeField] private int nextControlXOffset = -2;

    private Tilemap tilemap;
    private int nextControlAccumalator = 0;

    private int framePerSecond => 60;

    private void Start()
    { 
        tilemap = GetComponent<Tilemap>();
        
    }

    private void Update()
    {

        if (nextControlAccumalator >= framesPerControlTile && nextControlXOffset < 1)
        {
            tilemap.SetTile(GetCameraPosition() + new Vector3Int(nextControlXOffset, nextControlYOffset, 0), tile);
            nextControlAccumalator = 0;
            nextControlXOffset++;
        }
        else
        {
            //transform.position = Vector2.Lerp(transform.position, player.transform.position + new Vector3(0, offsetY, 0), .1f);
        }

        nextControlAccumalator++;
    }

    private Vector3Int GetCameraPosition()
    {
        return new Vector3Int((int) camera.transform.position.x, 
                              (int) camera.transform.position.y, 
                              (int) camera.transform.position.z);
    }
}

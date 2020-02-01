using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventControlsDisplay : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Tile tile;
    [SerializeField] private float framesPerControlTile = 15;
    [SerializeField] private float nextControlYOffset = 0;
    [SerializeField] private float nextControlXOffset = 0;

    private Tilemap tilemap;
    private int maxNumberOfControlTiles = 3;
    private int numberOfControlTilesSet = 0;
    private Vector3Int? initialCameraPosition;
    private int nextControlAccumalator = 0;

    private int framePerSecond => 60;

    private void Start()
    { 
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        SetTile();
        SetTileMapPosition();
        
        nextControlAccumalator++;
    }

    private void SetTile()
    {
        if (nextControlAccumalator >= framesPerControlTile && numberOfControlTilesSet < maxNumberOfControlTiles)
        {
            if (initialCameraPosition == null)
                initialCameraPosition = GetCameraPosition();

            tilemap.SetTile(GetNextTilePosition(), tile);

            nextControlAccumalator = 0;
            numberOfControlTilesSet++;
        }
        else if (numberOfControlTilesSet == maxNumberOfControlTiles)
        {
            initialCameraPosition = null;
        }
    }

    private Vector3Int GetCameraPosition()
    {
        return new Vector3Int((int) camera.transform.position.x, 
                              (int) camera.transform.position.y, 
                              (int) camera.transform.position.z);
    }

    private Vector3Int GetNextTilePosition()
    {
        return (initialCameraPosition + new Vector3Int(numberOfControlTilesSet, 0, 0))
               .GetValueOrDefault();
    }

    private void SetTileMapPosition()
    {
        transform.position = camera.transform.position + 
                             new Vector3(nextControlXOffset, nextControlYOffset, 0); // offset
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventControlsDisplay : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Tile[] controlTiles;
    [SerializeField] private Tile plusTile;

    private Tilemap tilemap;

    void Start()
    { 
        tilemap = GetComponent<Tilemap>();        
    }

    void Update()
    {
        tilemap.SetTile(GetCameraTilePosition(), plusTile);
    }

    Vector3Int GetCameraTilePosition()
    {
        return new Vector3Int((int)camera.transform.position.x,
                              (int)camera.transform.position.y,
                              (int)camera.transform.position.z);
    }
}

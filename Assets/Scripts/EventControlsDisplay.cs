using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventControlsDisplay : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private Tilemap tilemap;
    [SerializeField] private Tile tile;

    void Start()
    { 
        tilemap = GetComponent<Tilemap>();        
    }

    void Update()
    {
        tilemap.SetTile(new Vector3Int((int)camera.transform.position.x, (int)camera.transform.position.y, (int)camera.transform.position.z), tile);
    }
}

using Assets.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventControls : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Tile tile;
    [SerializeField] private Tile successTile;
    [SerializeField] private float framesPerControlTile = 15;
    [SerializeField] private float nextControlYOffset = 0;
    [SerializeField] private float nextControlXOffset = 0;
    [SerializeField] private float tileMapClearDelayInSeconds = 0.5f;

    private Tilemap tilemap;

    /* For drawing event control tiles */
    private Vector3Int? initialCameraPosition;
    private int maxNumberOfControlTiles = 3;
    private int numberOfControlTilesSet = 0;
    private int nextControlAccumalator = 0;

    private bool eventDrawn = false;
    private bool eventTriggered = false;
    private int eventCycleInSeconds = 4;
    private int eventCycleAccumalator = 0;
    private int tileMapClearDelayAccumalator = 0;
    private EventControlTile currentEventControl;

    private Queue<EventControlTile> eventControlTilesInCycle;

    private int framesPerSecond => 60;

    private void Start()
    { 
        tilemap = GetComponent<Tilemap>();
        eventControlTilesInCycle = new Queue<EventControlTile>();
        eventTriggered = true; // TODO: TEST
    }

    private void Update()
    {
        if (eventTriggered)
        {
            SetTile();
            SetTileMapPosition();
        }
        else
        {
            nextControlAccumalator = 0;
        }

        if (eventDrawn && eventTriggered)
        {
            if (currentEventControl == null)
                currentEventControl = eventControlTilesInCycle.Dequeue();

            if (Input.GetKeyDown(currentEventControl.keyCode))
            {
                tilemap.SetTile(currentEventControl.position, currentEventControl.successTile);

                currentEventControl = null;
                if (eventControlTilesInCycle.Count == 0)
                {
                    eventTriggered = false;
                }
            }
        }
        
        if (eventDrawn && !eventTriggered)
        {
            if (tileMapClearDelayAccumalator >= tileMapClearDelayInSeconds * framesPerSecond)
            {
                tilemap.ClearAllTiles();
                tileMapClearDelayAccumalator = 0;
                eventDrawn = false;
            }
            else
            {
                tileMapClearDelayAccumalator++;
            }
        }
    }

    private void SetTile()
    {
        if (nextControlAccumalator >= framesPerControlTile && numberOfControlTilesSet < maxNumberOfControlTiles)
        {
            if (initialCameraPosition == null)
                initialCameraPosition = GetCameraPosition();

            // Store tiles for event cycle
            var eventControlTile = new EventControlTile(KeyCode.W, tile, successTile, GetNextTilePosition()); /* TODO: Hard code */
            eventControlTilesInCycle.Enqueue(eventControlTile);

            tilemap.SetTile(eventControlTile.position, tile);

            nextControlAccumalator = 0;
            numberOfControlTilesSet++;
        }
        else if (numberOfControlTilesSet == maxNumberOfControlTiles)
        {
            initialCameraPosition = null;
            eventDrawn = true;
        }

        nextControlAccumalator++;
    }

    private Vector3Int GetCameraPosition()
    {
        return new Vector3Int((int)camera.transform.position.x, 
                              (int)camera.transform.position.y, 
                              (int)camera.transform.position.z);
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

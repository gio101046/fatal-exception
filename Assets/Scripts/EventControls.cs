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
    [SerializeField] private int nextControlYOffset = 0;
    [SerializeField] private int nextControlXOffset = 0;
    [SerializeField] private float tileMapClearDelayInSeconds = 0.5f;

    private Tilemap tilemap;

    /* For drawing event control tiles */
    private Vector3Int? initialCameraPosition;
    private int maxNumberOfControlTiles = 3;
    private int numberOfControlTilesSet = 0;
    private int nextControlAccumalator = 0;

    private bool eventDrawn = false;
    private bool eventTriggered = false;
    private int eventCycleInSeconds = 2;
    private int eventCycleAccumalator = 0;
    private int tileMapClearDelayAccumalator = 0;
    private EventControlTile currentEventControl;

    private Collider2D currentPlayerCollider;
    private Collider2D currentEnemyCollider;

    private Queue<EventControlTile> eventControlTilesInCycle;

    private int framesPerSecond => 60;
    private Vector3Int nextControlOffSetAsVector => new Vector3Int(nextControlXOffset, nextControlYOffset, 0);

    public void TriggerEvent(Collider2D playerCollider, Collider2D enemyCollider)
    {
        currentPlayerCollider = playerCollider;
        currentEnemyCollider = enemyCollider;
        Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);

        eventTriggered = true;
    }

    public bool IsEventTriggered()
    {

        return eventTriggered;
    }

    private void Start()
    { 
        tilemap = GetComponent<Tilemap>();
        eventControlTilesInCycle = new Queue<EventControlTile>();
    }

    private void Update()
    {
        if (eventTriggered)
        {
            SetTile();
            //SetTileMapPosition();
        }

        if (eventDrawn && eventTriggered)
        {
            PerformControlEvent();
        }
        
        if (eventDrawn && !eventTriggered)
        {
            Reset();
        }
    }

    private void SetTile()
    {
        if (nextControlAccumalator >= framesPerControlTile && numberOfControlTilesSet < maxNumberOfControlTiles)
        {
            if (initialCameraPosition == null)
                initialCameraPosition = GetCameraPosition();

            // Store tiles for event cycle
            var eventControlTile = new EventControlTile(KeyCode.W, tile, successTile, GetNextTilePosition() + nextControlOffSetAsVector); /* TODO: Hard code */
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
        else
        {
            nextControlAccumalator++;
        }
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

    private void PerformControlEvent()
    {
        nextControlAccumalator = 0;

        if (currentEventControl == null)
            currentEventControl = eventControlTilesInCycle.Dequeue();

        if (Input.GetKeyDown(currentEventControl.keyCode))
        {
            tilemap.SetTile(currentEventControl.position, currentEventControl.successTile);

            currentEventControl = null;
            if (eventControlTilesInCycle.Count == 0)
            {
                eventTriggered = false;
                eventCycleAccumalator = 0;
            }
        }
        else if (eventCycleAccumalator >= eventCycleInSeconds * framesPerSecond)
        {

        }
        else
        {
            eventCycleAccumalator++;
        }
    }

    private void Reset()
    {
        if (tileMapClearDelayAccumalator >= tileMapClearDelayInSeconds * framesPerSecond)
        {
            tilemap.ClearAllTiles();
            tileMapClearDelayAccumalator = 0;
            eventDrawn = false;
            numberOfControlTilesSet = 0;

            Physics2D.IgnoreCollision(currentPlayerCollider, currentEnemyCollider, false);
            currentPlayerCollider = null;
            currentEnemyCollider = null;
        }
        else
        {
            tileMapClearDelayAccumalator++;
        }
    }

    private void SetTileMapPosition()
    {
        transform.position = camera.transform.position + 
                             new Vector3Int(nextControlXOffset, nextControlYOffset, 0); // offset
    }
}

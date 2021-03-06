using Assets.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventControls : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject music;
    [SerializeField] private List<Tile> tiles;
    [SerializeField] private List<Tile> pressedTiles;
    [SerializeField] private List<Tile> successTiles;
    [SerializeField] private List<Tile> failTiles;
    [SerializeField] private Tile plusTile;

    [SerializeField] private float framesPerControlTile = 15;
    [SerializeField] private int nextControlYOffset = 0;
    private int nextControlXOffset => 0 - maxNumberOfControlTiles + 1;
    [SerializeField] private float tileMapClearDelayInSeconds = 0.5f;
    private int numberOfEventsCount = 0;

    private int maxNumberOfEvents => currentEnemyCollider.gameObject
                                                         .GetComponent<BugMovement>()
                                                         .eventTriggerCount;
    private int maxNumberOfControlTiles => currentEnemyCollider.gameObject
                                                               .GetComponent<BugMovement>()
                                                               .controlTriggerCounterEvent;

    private Tilemap tilemap;

    /* For drawing event control tiles */
    private Vector3Int? initialCameraPosition;
    private int numberOfControlTilesSet = 0;
    private int nextControlAccumalator = 0;

    private bool eventDrawn = false;
    private bool eventTriggered = false;
    private float eventCycleInSeconds = 2.8f;
    private int eventCycleAccumalator = 0;
    private int tileMapClearDelayAccumalator = 0;
    private EventControlTile currentEventControl;
    private bool isInBattle = false;
    private bool hasFailed = false;
    private int buttonPressAnimationAccumalator = 0;

    private Collider2D currentPlayerCollider;
    private Collider2D currentEnemyCollider;

    private List<EventControlTile> eventControlTilesInCycle;
    private Queue<EventControlTile> eventControlTilesInCycleQueue;
    private bool? isControlTile = null;

    private int framesPerSecond => 60;
    private Vector3Int nextControlOffSetAsVector => new Vector3Int(nextControlXOffset, nextControlYOffset, 0);

    public void TriggerEvent(Collider2D playerCollider, Collider2D enemyCollider, Player player)
    {
        if (isInBattle)
            return;

        currentPlayerCollider = playerCollider;
        currentEnemyCollider = enemyCollider;

        enemyCollider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        enemyCollider.gameObject.GetComponent<BugMovement>().DisableMovement();

        player.StartEncounter();
        Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);

        eventTriggered = true;
        isInBattle = true;
    }

    public bool IsEventTriggered()
    {
        return eventTriggered;
    }

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        eventControlTilesInCycle = new List<EventControlTile>();
    }

    private void Update()
    {
        if (eventTriggered)
        {
            SetTile();
            // SetTileMapPosition();
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
            EventControlTile eventControlTile = null;
            if (isControlTile == null || isControlTile.GetValueOrDefault())
            {
                eventControlTile = GenerateEventControlTile(GetNextTilePosition() + nextControlOffSetAsVector);
                isControlTile = false;
                numberOfControlTilesSet++;
            }
            else
            {
                eventControlTile = GenerateEventControlPlusTile(GetNextPlusTilePosition() + nextControlOffSetAsVector);
                isControlTile = true;
            }

            eventControlTilesInCycle.Add(eventControlTile);

            tilemap.SetTile(eventControlTile.position, eventControlTile.tile);

            nextControlAccumalator = 0;
        }
        else if (numberOfControlTilesSet == maxNumberOfControlTiles)
        {
            initialCameraPosition = null;
            isControlTile = null;
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
        return (initialCameraPosition + new Vector3Int(numberOfControlTilesSet * 2, 0, 0))
               .GetValueOrDefault();
    }

    private Vector3Int GetNextPlusTilePosition()
    {
        return (initialCameraPosition + new Vector3Int(numberOfControlTilesSet * 2 - 1, 0, 0))
               .GetValueOrDefault();
    }

    IEnumerator PlayVictoryMusic()
    {
        music = GameObject.Find("Music");
        var audioSrc = music.GetComponent<AudioSource>();
        audioSrc.Pause();
        SoundManagerScript.PlaySound("win");
        var clipSeconds = SoundManagerScript.audioSrc.clip.length;
        yield return new WaitForSeconds(clipSeconds);
        audioSrc.Play();
    }

    private void PerformControlEvent()
    {
        nextControlAccumalator = 0;

        if (currentEventControl == null)
            currentEventControl = eventControlTilesInCycle.First(x => !x.isPlusTile && !x.isPerformed);
        else
        {
            if (buttonPressAnimationAccumalator >= framesPerControlTile)
            {
                tilemap.SetTile(currentEventControl.position, currentEventControl.isPressed ? currentEventControl.pressedTile :
                                                                              currentEventControl.tile);
                currentEventControl.isPressed = !currentEventControl.isPressed;
                buttonPressAnimationAccumalator = 0;
            }
            else
                buttonPressAnimationAccumalator++;
        }

        if (Input.GetKeyDown(currentEventControl.keyCode))
        {
            currentEventControl.isPerformed = true;
            tilemap.SetTile(currentEventControl.position, currentEventControl.successTile);

            currentEventControl = null;
            if (eventControlTilesInCycle.Where(x => !x.isPlusTile && !x.isPerformed).Count() == 0)
            {
                numberOfEventsCount++;
                eventTriggered = false;
                eventCycleAccumalator = 0;

                if (maxNumberOfEvents == numberOfEventsCount)
                {
                    player.EndEncounter();
                    isInBattle = false;
                    // Destroy(currentEnemyCollider.gameObject);
                    // currentEnemyCollider = null;
                    hasFailed = false;
                    StartCoroutine(PlayVictoryMusic());
                }
            }

            return;
        }
        else if (eventCycleAccumalator >= eventCycleInSeconds * framesPerSecond * player.GetStaminaDifficultyFactor())
        {
            eventTriggered = false;
            isInBattle = false;
            eventCycleAccumalator = 0;
            currentEventControl = null;
            numberOfEventsCount = 0;
            hasFailed = true;

            player.ThrowUserInTheAirHurt();
            SoundManagerScript.PlaySound("bug laugh");

            currentEnemyCollider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            currentEnemyCollider.gameObject.GetComponent<BugMovement>().EnableMovement();
            player.EndEncounter();

            return;
        }
        else if (FailedToClickCorrect(currentEventControl.keyCode))
        {
            SoundManagerScript.PlaySound("wrong");
            tilemap.SetTile(currentEventControl.position, currentEventControl.failTile);
            eventTriggered = false;
            isInBattle = false;
            eventCycleAccumalator = 0;
            currentEventControl = null;
            numberOfEventsCount = 0;
            hasFailed = true;

            player.ThrowUserInTheAirHurt();

            currentEnemyCollider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            currentEnemyCollider.gameObject.GetComponent<BugMovement>().EnableMovement();
            player.EndEncounter();

            SoundManagerScript.PlaySound("bug laugh");
            return;
        }

        eventCycleAccumalator++;
    }

    private void Reset()
    {
        if (tileMapClearDelayAccumalator >= tileMapClearDelayInSeconds * framesPerSecond)
        {
            eventControlTilesInCycle.Clear();
            tilemap.ClearAllTiles();
            tileMapClearDelayAccumalator = 0;
            eventDrawn = false;
            numberOfControlTilesSet = 0;

            if (maxNumberOfEvents == numberOfEventsCount)
            {
                if (currentEnemyCollider != null && hasFailed)
                    Physics2D.IgnoreCollision(currentPlayerCollider, currentEnemyCollider, false);

                numberOfEventsCount = 0;
                currentPlayerCollider = null;
                currentEnemyCollider = null;
            }
            else
            {
                eventTriggered = true && !hasFailed;
                isInBattle = true && !hasFailed;

                if (hasFailed)
                {
                    if (currentEnemyCollider != null)
                        Physics2D.IgnoreCollision(currentPlayerCollider, currentEnemyCollider, false);
                }
            }
        }
        else
        {
            tileMapClearDelayAccumalator++;
        }
    }

    //private void SetTileMapPosition()
    //{
    //    var offset = camera.transform.position.x - (int)camera.transform.position.x;
    //    transform.position = new Vector3(transform.position.x + offset, transform.position.y, 0);
    //}
    
    private EventControlTile GenerateEventControlTile(Vector3Int position)
    {
        var randomNumber = Random.Range(0, TileToKeyMappings.TileToKey.Count);

        return new EventControlTile(TileToKeyMappings.TileToKey[randomNumber],
                                    tiles[randomNumber],
                                    pressedTiles[randomNumber],
                                    successTiles[randomNumber],
                                    failTiles[randomNumber],
                                    position);
    }

    private EventControlTile GenerateEventControlPlusTile(Vector3Int position)
    {
        return new EventControlTile(KeyCode.Escape,
                                    plusTile,
                                    null,
                                    null,
                                    null,
                                    position,
                                    true);
    }

    private bool FailedToClickCorrect(KeyCode correctKey)
    {
        var allKeysButCorrect = TileToKeyMappings.TileToKey.Where(x => x != correctKey);

        return allKeysButCorrect.Any(x => Input.GetKeyDown(x));
    }
}

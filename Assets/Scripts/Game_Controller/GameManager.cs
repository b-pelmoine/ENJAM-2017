using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    [Header("Game Parameters")]
    [Tooltip("rate at which the zone shrinks in units per second")]
    public float shrinkRate;
    public float asteroidCircleStart; // how far do we start from the center
    public float asteroidCircleEnd; // how close from the center the cirlce ends
    public int astreoidCount;
    public int asteroidMaxCount;

    [Header("Game manager related variables")]
    private List<GameObject> players;

    void Start()
    {
        players = new List<GameObject>();
    }

    void OnEnable()
    {
        EventManager.StartListening("start", GameStart);
    }

    void OnDisable()
    {
        EventManager.StopListening("start", GameStart);
    }

    void GameStart()
    {
        //spawn players

        //enable splitscreen
        SplitScreen.switchState();
        EventManager.TriggerEvent("splitscreen");
        //show screen and start the game for real
    }
}

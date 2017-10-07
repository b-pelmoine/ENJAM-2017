using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    //self
    public static GameManager instance;

    [Header("Game Parameters")]
    public float shrinkDuration; //in seconds
    public float asteroidCircleStart; // how far do we start from the center
    public float asteroidCircleEnd; // how close from the center the cirlce ends
    public int astreoidCount;
    public int asteroidMaxCount;
    public int beltAsteroidCount;

    [Header("Players")]
    public float moveSpeed;
    public int maxHP;
    public Color colorJ1;
    public Color colorJ2;

    [Header("Dash")]
    public float dashDistance;
    public float dashSpeed;
    public float dashCooldown;

    [Header("Shield")]
    public float shieldCooldown;
    public float shieldDuration;

    [Header("Attack")]
    public float maxWidth;
    public float maxLenght;
    public int damagesToEnnemy;
    public float percentageHealthCost;
    public float attackCooldown;

    [Header("Asteroids")]
    public int damagesOnCollision;
    public float speedMultiplierOnCollision;
    public float speedMultiplierOnSpawn;

    [Header("Game manager related variables")]
    public List<GameObject> players;
    private AreaManager areaGenerator;

    public Vector3 getPlayerPosition(int player)
    {
        return players[player].transform.position;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
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
        EventManager.TriggerEvent("generateArea");
        //spawn players

        //enable splitscreen
        SplitScreen.switchState();
        EventManager.TriggerEvent("splitscreen");
        //show screen and start the game for real
        EventManager.TriggerEvent("gameStart");
    }
}

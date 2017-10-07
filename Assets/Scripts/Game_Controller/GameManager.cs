using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
	
	void Update () {
        if (Input.GetKeyDown("x"))
        {
            EventManager.TriggerEvent("splitscreen");
        }
        if (Input.GetKeyDown("c"))
        {
            SplitScreen.switchState();
        }
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
        Debug.Log("Game starting !");
    }
}

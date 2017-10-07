using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

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
        SplitScreen.switchState();
        EventManager.TriggerEvent("splitscreen");
    }
}

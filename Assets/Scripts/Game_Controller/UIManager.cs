using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Button startGameButton;
    [SerializeField]
    private GameObject MainMenuRoot;
    [SerializeField]
    private GameObject LoadingRoot;

    void Start () {
        startGameButton.onClick.AddListener(StartGame);
	}
	
	void StartGame()
    {
        EventManager.TriggerEvent("start");
        MainMenuRoot.SetActive(false);
    }
}

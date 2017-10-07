using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Button startGameButton;
    [SerializeField]
    private GameObject MainMenuRoot;

	void Start () {
        startGameButton.onClick.AddListener(startGame);
	}
	
	void startGame()
    {
        EventManager.TriggerEvent("start");
        MainMenuRoot.SetActive(false);
    }
}

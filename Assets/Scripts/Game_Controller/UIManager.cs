using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Button startGameButton;
    private Button creditsButton;
    [SerializeField]
    private GameObject MainMenuRoot;
    [SerializeField]
    private GameObject LoadingRoot;
    [SerializeField]
    private GameObject GameUIRoot;

    void OnEnable()
    {
        EventManager.StartListening("gameStart", () => { StartCoroutine(closeLoadingScreen()); });
    }

    void OnDisable()
    {
        EventManager.StopListening("gameStart", () => { StartCoroutine(closeLoadingScreen()); });
    }

    void Start () {
        startGameButton.onClick.AddListener(StartGame);

        LoadingRoot.SetActive(false);
        GameUIRoot.SetActive(false);
        MainMenuRoot.SetActive(true);
    }
	
	void StartGame()
    {
        LoadingRoot.SetActive(true);
        MainMenuRoot.SetActive(false);
        EventManager.TriggerEvent("loading");
        StartCoroutine(startGameDelayed());
    }

    IEnumerator closeLoadingScreen()
    {
        CanvasGroup group = LoadingRoot.GetComponent<CanvasGroup>();
        float k = 0.0f;
        while( k < .3f)
        {
            k += .3f / 30;
            yield return new WaitForSeconds(.3f/30);
            group.alpha = 1 - k / .3f;
        }
        LoadingRoot.SetActive(false);
        group.alpha = 1.0f;
    }

    IEnumerator startGameDelayed()
    {
        yield return new WaitForSeconds(.5f);
        EventManager.TriggerEvent("start");
    }
}

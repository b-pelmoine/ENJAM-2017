using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIUpdater : MonoBehaviour {

    public GameObject GameUiRoot;

    [Header("Health Bar")]
    public Color Healhy;
    public Color InDanger;
    public Image lifeBarJ1;
    public Image lifeBarJ2;
    public GameObject DangerstatusJ1;
    public GameObject DangerstatusJ2;

    [Header("Icons")]
    public Image dashJ1;
    public Image shieldJ1;
    public Image attackJ1;
    public Image dashJ2;
    public Image shieldJ2;
    public Image attackJ2;

    bool InGame = false;

    void Update()
    {
        if(InGame)
        {
            //set health & change status etc
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("gameStart", gameStart);
        EventManager.StartListening("DashJ1", () => { StartCoroutine(Fill(dashJ1, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StartListening("DashJ2", () => { StartCoroutine(Fill(dashJ2, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StartListening("ShieldJ1", () => { StartCoroutine(Fill(shieldJ1, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StartListening("ShieldJ2", () => { StartCoroutine(Fill(shieldJ2, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StartListening("AttackJ1", () => { StartCoroutine(Fill(attackJ1, .1f, GameManager.instance.attackCooldown)); });
        EventManager.StartListening("AttackJ2", () => { StartCoroutine(Fill(attackJ2, .1f, GameManager.instance.attackCooldown)); });
    }

    void OnDisable()
    {
        EventManager.StopListening("gameStart", gameStart);
        EventManager.StopListening("DashJ1",    () => { StartCoroutine(Fill(dashJ1, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StopListening("DashJ2",    () => { StartCoroutine(Fill(dashJ2, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StopListening("ShieldJ1",  () => { StartCoroutine(Fill(shieldJ1, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StopListening("ShieldJ2",  () => { StartCoroutine(Fill(shieldJ2, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StopListening("AttackJ1",  () => { StartCoroutine(Fill(attackJ1, .1f, GameManager.instance.attackCooldown)); });
        EventManager.StopListening("AttackJ2",  () => { StartCoroutine(Fill(attackJ2, .1f, GameManager.instance.attackCooldown)); });
    }

    void gameStart()
    {
        GameUiRoot.SetActive(true);
        InGame = true;
    }

    IEnumerator Fill(Image img, float rate, float fillDuration)
    {
        float startTime = Time.time;
        float elapsedTime = 0;
        img.fillAmount = 0.0f;
        //do stuff pls machine
        while (startTime + elapsedTime < startTime + fillDuration)
        {
            float ratio = elapsedTime / fillDuration;
            elapsedTime += rate;
            yield return new WaitForSeconds(rate);
            img.fillAmount = (ratio > 1.0f) ? 1.0f : ratio;
        }
        img.fillAmount = 1.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIUpdater : MonoBehaviour {

    public GameObject GameUiRoot;

    public PlayerController[] players;

    [Header("Health Bar")]
    public Image lifeBarJ1;
    public Image lifeBarJ2;
    public GameObject DangerstatusJ1;
    public GameObject DangerstatusJ2;

    [Header("Icons")]
    public Image dashJ1;
    public Image shieldJ1;
    public Image attackJ1;
    public Image attackJ1Txt;
    public Text vieJ1;
    public Image dashJ2;
    public Image shieldJ2;
    public Image attackJ2;
    public Image attackJ2Txt;
    public Text vieJ2;

    bool InGame = false;

    void Update()
    {
        if(InGame)
        {
            if(players.Length > 0)
            {
                //set health & change status etc
                //J1
                float ratio = players[1].GetHealth();
                lifeBarJ1.fillAmount = ratio;
                vieJ1.text = ((int)100 * ratio).ToString() + "%";
                if (!DangerstatusJ1.activeInHierarchy)
                {
                    if (ratio <= .5f)
                        DangerstatusJ1.SetActive(true);
                }
                else
                {
                    if (ratio > .5f)
                        DangerstatusJ1.SetActive(false);
                }
                //J2
                ratio = players[0].GetHealth();
                lifeBarJ2.fillAmount = ratio;
                vieJ2.text = ((int)100 * ratio).ToString() + "%";
                if (!DangerstatusJ2.activeInHierarchy)
                {
                    if (ratio <= .5f)
                        DangerstatusJ2.SetActive(true);
                }
                else
                {
                    if (ratio > .5f)
                        DangerstatusJ2.SetActive(false);
                }
            }
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("gameStart", gameStart);
        EventManager.StartListening("DashJ1", () => { StartCoroutine(Fill(dashJ1, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StartListening("DashJ2", () => { StartCoroutine(Fill(dashJ2, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StartListening("ShieldJ1", () => { StartCoroutine(Fill(shieldJ1, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StartListening("ShieldJ2", () => { StartCoroutine(Fill(shieldJ2, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StartListening("AttackJ1", () => { if (attackJ1Txt) attackJ1Txt.enabled = false;
            StartCoroutine(Fill(attackJ1, .1f, GameManager.instance.attackCooldown, () => { if (attackJ1Txt) attackJ1Txt.enabled = true; })); });
        EventManager.StartListening("AttackJ2", () => { if (attackJ2Txt) attackJ2Txt.enabled = false;
            StartCoroutine(Fill(attackJ2, .1f, GameManager.instance.attackCooldown, () => { if (attackJ2Txt) attackJ2Txt.enabled = true; })); });
    }

    void OnDisable()
    {
        EventManager.StopListening("gameStart", gameStart);
        EventManager.StopListening("DashJ1",    () => { StartCoroutine(Fill(dashJ1, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StopListening("DashJ2",    () => { StartCoroutine(Fill(dashJ2, .05f, GameManager.instance.dashCooldown)); });
        EventManager.StopListening("ShieldJ1",  () => { StartCoroutine(Fill(shieldJ1, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StopListening("ShieldJ2",  () => { StartCoroutine(Fill(shieldJ2, .1f, GameManager.instance.shieldCooldown)); });
        EventManager.StopListening("AttackJ1",  () => { if(attackJ1Txt)attackJ1Txt.enabled = false;
            StartCoroutine(Fill(attackJ1, .1f, GameManager.instance.attackCooldown, () => { if (attackJ1Txt) attackJ1Txt.enabled = true; })); });
        EventManager.StopListening("AttackJ2",  () => { if (attackJ2Txt) attackJ2Txt.enabled = false;
            StartCoroutine(Fill(attackJ2, .1f, GameManager.instance.attackCooldown, () => { if (attackJ2Txt) attackJ2Txt.enabled = true; })); });
    }

    void gameStart()
    {
        GameUiRoot.SetActive(true);
        players = GameObject.FindObjectsOfType<PlayerController>();
        if (players.Length == 0)
        {
            Debug.LogError("No players found");
        }
        InGame = true;
    }

    IEnumerator Fill(Image img, float rate, float fillDuration, System.Action callback = null)
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

        if(callback != null) callback();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {

    private PlayerState playerState;

    private float speed;
    public Boundary boundary;
    public GameObject shield;

    private GameManager gameManager;
    private int health;
    private Rigidbody2D rig;
    private Vector3 _origPos;
    private Vector2 savedVelocity;

    //Dash variables
    private DashState dashStateOne;
    private DashState dashStateTwo;
    private float dashSpeed;
    private float dashDistance;
    private float dashCooldown;
    private float dashTime;
    private float dashCD;
    private bool dashing;
    
    //Shield variables
    private ShieldState shieldStateOne;
    private ShieldState shieldStateTwo;
    private float shieldDuration;
    private float shieldTime;
    private float shieldCooldown;
    private float shieldCD;
    private bool isShielding;

    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    public enum ShieldState
    {
        Ready,
        Shielding,
        Cooldown
    }

    public enum PlayerState
    {
        PlayerOne,
        PlayerTwo
    }

    private void Start()
    {
        if(gameObject.name == "Player1")
        {
            playerState = PlayerState.PlayerOne;
        }
        else
        {
            playerState = PlayerState.PlayerTwo;
        }

        rig = GetComponent<Rigidbody2D>();
        _origPos = transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        health = gameManager.maxHP;
        speed = gameManager.moveSpeed;

        dashDistance = gameManager.dashDistance;
        dashSpeed = gameManager.dashSpeed;
        dashCooldown = gameManager.dashCooldown;
        dashStateOne = DashState.Ready;
        dashStateTwo = DashState.Ready;
        dashTime = 0.0f;
        dashCD = dashCooldown;
        dashing = false;

        shieldStateOne = ShieldState.Ready;
        shieldStateTwo = ShieldState.Ready;
        shieldDuration = gameManager.shieldDuration;
        shieldCooldown = gameManager.shieldCooldown;
        shieldCD = shieldCooldown;
        shieldTime = 0.0f;
        isShielding = false;

    }

    private void Update()
    {
        Dash();
        Shield();

        Vector3 moveDirection = gameObject.transform.position - _origPos;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        _origPos = transform.position;
    }

    void FixedUpdate()
    {
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;
        switch (playerState)
        {
            case PlayerState.PlayerOne:
                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = Input.GetAxis("Vertical");
                break;
            case PlayerState.PlayerTwo:
                moveHorizontal = Input.GetAxis("Horizontal2");
                moveVertical = Input.GetAxis("Vertical2");
                break;
        }

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (dashing)
        {
            rig.velocity = movement * speed * dashSpeed;
        }
        else
        {
            rig.velocity = movement * speed;
        }

        rig.position = new Vector2
        (
            Mathf.Clamp(rig.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rig.position.y, boundary.yMin, boundary.yMax)
        );

    }

    public void DamagePlayer(int damages)
    {
        if (!isShielding)
        {
            health -= damages;
            if (health <= 0) Kill();
        }
    }

    private void Kill()
    {
        //play anim death etc
        Debug.Log("Mort");
    }

    private void Dash()
    {
        switch (playerState)
        {
            case PlayerState.PlayerOne:
                switch (dashStateOne)
                {
                    case DashState.Ready:
                        var isDashOneKeyDown = Input.GetButtonDown("Dash");
                        if (isDashOneKeyDown)
                        {
                            dashing = true;
                            dashStateOne = DashState.Dashing;
                        }
                        break;
                    case DashState.Dashing:
                        dashTime += Time.deltaTime * 3.0f;
                        if (dashTime >= dashDistance)
                        {
                            dashTime = 0.0f;
                            dashing = false;
                            dashStateOne = DashState.Cooldown;
                        }
                        break;
                    case DashState.Cooldown:
                        dashCD -= Time.deltaTime;
                        if (dashCD <= 0)
                        {
                            dashCD = dashCooldown;
                            dashStateOne = DashState.Ready;
                        }
                        break;
                }
                break;
            case PlayerState.PlayerTwo:
                switch (dashStateTwo)
                {
                    case DashState.Ready:
                        var isDashTwoKeyDown = Input.GetButtonDown("Dash2");
                        if (isDashTwoKeyDown)
                        {
                            dashing = true;
                            dashStateTwo = DashState.Dashing;
                        }
                        break;
                    case DashState.Dashing:
                        dashTime += Time.deltaTime * 3.0f;
                        if (dashTime >= dashDistance)
                        {
                            dashTime = 0.0f;
                            dashing = false;
                            dashStateTwo = DashState.Cooldown;
                        }
                        break;
                    case DashState.Cooldown:
                        dashCD -= Time.deltaTime;
                        if (dashCD <= 0)
                        {
                            dashCD = dashCooldown;
                            dashStateTwo = DashState.Ready;
                        }
                        break;
                }
                break;
        }
    }

    private void Shield()
    {
        switch (playerState)
        {
            case PlayerState.PlayerOne:
                switch (shieldStateOne)
                {
                    case ShieldState.Ready:
                        var isShieldOneKeyDown = Input.GetButtonDown("Shield");
                        if (isShieldOneKeyDown)
                        {
                            isShielding = true;
                            shieldStateOne = ShieldState.Shielding;
                            shield.SetActive(true);
                        }
                        break;
                    case ShieldState.Shielding:
                        shieldTime += Time.deltaTime;
                        if (shieldTime >= shieldDuration)
                        {
                            shieldTime = 0.0f;
                            isShielding = false;
                            shieldStateOne = ShieldState.Cooldown;
                            shield.SetActive(false);
                        }
                        break;
                    case ShieldState.Cooldown:
                        shieldCD -= Time.deltaTime;
                        if (shieldCD <= 0)
                        {
                            shieldCD = shieldCooldown;
                            shieldStateOne = ShieldState.Ready;
                        }
                        break;
                }
                break;
            case PlayerState.PlayerTwo:
                switch (shieldStateTwo)
                {
                    case ShieldState.Ready:
                        var isShieldTwoKeyDown = Input.GetButtonDown("Shield2");
                        if (isShieldTwoKeyDown)
                        {
                            isShielding = true;
                            shieldStateTwo = ShieldState.Shielding;
                            shield.SetActive(true);
                        }
                        break;
                    case ShieldState.Shielding:
                        shieldTime += Time.deltaTime;
                        if (shieldTime >= shieldDuration)
                        {
                            shieldTime = 0.0f;
                            isShielding = false;
                            shieldStateTwo = ShieldState.Cooldown;
                            shield.SetActive(false);
                        }
                        break;
                    case ShieldState.Cooldown:
                        shieldCD -= Time.deltaTime;
                        if (shieldCD <= 0)
                        {
                            shieldCD = shieldCooldown;
                            shieldStateTwo = ShieldState.Ready;
                        }
                        break;
                }
                break;
        }
    }
}

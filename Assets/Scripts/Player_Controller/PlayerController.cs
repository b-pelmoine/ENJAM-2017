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
    public ParticleSystem dashParticles;
    public GameObject tail;

    private GameManager gameManager;
    public int health;
    private Rigidbody2D rig;
    private Vector3 _origPos;
    private Vector2 savedVelocity;

    public GameObject collisionAnim;

    //Dash variables
    public GameObject dashAnim;
    public Transform dashAnimPos;
    private DashState dashState;
    private float dashSpeed;
    private float dashDistance;
    private float dashCooldown;
    private float dashTime;
    private float dashCD;
    private bool dashing;

    //Shield variables
    public GameObject shieldAnim;
    private ShieldState shieldState;
    private float shieldDuration;
    private float shieldTime;
    private float shieldCooldown;
    private float shieldCD;
    private bool isShielding;

    //Laser variables
    public GameObject chargeAnim;
    public Transform chargeAnimPos;
    public LaserController laser;
    private LaserState laserState;
    private float laserDuration;
    private float laserTime;
    private float laserCooldown;
    private float laserCD;
    private bool isFiring;
    private float chargeTime;
    private float chargeCD;

    public enum LaserState
    {
        Ready,
        Charging,
        Firing,
        Cooldown
    }

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
        dashState = DashState.Ready;
        dashTime = 0.0f;
        dashCD = dashCooldown;
        dashing = false;

        shieldState = ShieldState.Ready;
        shieldDuration = gameManager.shieldDuration;
        shieldCooldown = gameManager.shieldCooldown;
        shieldCD = shieldCooldown;
        shieldTime = 0.0f;
        isShielding = false;

        laserState = LaserState.Ready;
        laserDuration = gameManager.laserDuration;
        laserTime = 0.0f;
        laserCooldown = gameManager.attackCooldown;
        laserCD = laserCooldown;
        isFiring = false;
        chargeTime = gameManager.chargeTime;
        chargeCD = chargeTime;
    }

    private void Update()
    {
        UltraLaser();
        if (!isFiring)
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
        if (!isFiring)
        {
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

    }

    public float GetHealth()
    {
        return health / gameManager.maxHP;
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
        switch (dashState)
        {
            case DashState.Ready:
                switch (playerState)
                {
                    case PlayerState.PlayerOne:
                        var isDashOneKeyDown = Input.GetButtonDown("Dash");
                        if (isDashOneKeyDown)
                        {
                            dashing = true;
                            dashState = DashState.Dashing;
                            dashParticles.Play();
                            GameObject dashInstance = GameObject.Instantiate(dashAnim);
                            dashInstance.transform.position = dashAnimPos.position;
                            dashInstance.transform.rotation = transform.rotation;
                            dashInstance.GetComponent<Animator>().SetTrigger("StartDash");
                            EventManager.TriggerEvent("DashJ1");
                        }
                        break;
                    case PlayerState.PlayerTwo:
                        var isDashTwoKeyDown = Input.GetButtonDown("Dash2");
                        if (isDashTwoKeyDown)
                        {
                            dashing = true;
                            dashState = DashState.Dashing;
                            dashParticles.Play();
                            GameObject dashInstance = GameObject.Instantiate(dashAnim);
                            dashInstance.transform.position = dashAnimPos.position;
                            dashInstance.transform.rotation = transform.rotation;
                            dashInstance.GetComponent<Animator>().SetTrigger("StartDash");
                            EventManager.TriggerEvent("DashJ2");
                        }
                        break;
                }
                break;
            case DashState.Dashing:
                dashTime += Time.deltaTime * 3.0f;
                if (dashTime >= dashDistance)
                {
                    dashTime = 0.0f;
                    dashing = false;
                    dashState = DashState.Cooldown;
                    Destroy(GameObject.Find("DashAnim(Clone)"));
                }
                break;
            case DashState.Cooldown:
                dashCD -= Time.deltaTime;
                if (dashCD <= 0)
                {
                    dashCD = dashCooldown;
                    dashState = DashState.Ready;
                }
                break;
        }
    }

    private void Shield()
    {
        switch (shieldState)
        {
            case ShieldState.Ready:
                switch (playerState)
                {
                    case PlayerState.PlayerOne:
                        var isShieldOneKeyDown = Input.GetButtonDown("Shield");
                        if (isShieldOneKeyDown)
                        {
                            isShielding = true;
                            shieldState = ShieldState.Shielding;
                            shield.SetActive(true);
                            shieldAnim.GetComponent<Animator>().SetTrigger("StartShield");
                            EventManager.TriggerEvent("ShieldJ1");
                        }
                        break;
                    case PlayerState.PlayerTwo:
                        var isShieldTwoKeyDown = Input.GetButtonDown("Shield2");
                        if (isShieldTwoKeyDown)
                        {
                            isShielding = true;
                            shieldState = ShieldState.Shielding;
                            shield.SetActive(true);
                            shieldAnim.GetComponent<Animator>().SetTrigger("StartShield");
                            EventManager.TriggerEvent("ShieldJ2");
                        }
                        break;
                }
                break;
            case ShieldState.Shielding:
                shieldTime += Time.deltaTime;
                if (shieldTime >= shieldDuration)
                {
                    shieldTime = 0.0f;
                    isShielding = false;
                    shieldState = ShieldState.Cooldown;
                    shield.SetActive(false);
                }
                break;
            case ShieldState.Cooldown:
                shieldCD -= Time.deltaTime;
                if (shieldCD <= 0)
                {
                    shieldCD = shieldCooldown;
                    shieldState = ShieldState.Ready;
                }
                break;
        }
    }

    private void UltraLaser()
    {
        switch (laserState)
        {
            case LaserState.Ready:
                switch (playerState)
                {
                    case PlayerState.PlayerOne:
                        var isLaserOneKeyDown = Input.GetButtonDown("Laser");
                        if (isLaserOneKeyDown)
                        {
                            isFiring = true;
                            rig.simulated = false;
                            Rigidbody2D[] rigs = tail.GetComponentsInChildren<Rigidbody2D>();
                            foreach(Rigidbody2D r in rigs)
                            {
                                r.simulated = false;
                            }
                            laserState = LaserState.Charging;
                            EventManager.TriggerEvent("AttackJ1");
                            GameObject charge = GameObject.Instantiate(chargeAnim);
                            charge.transform.position = chargeAnimPos.position;
                            charge.transform.rotation = transform.rotation;
                            charge.GetComponent<Animator>().SetTrigger("Start");
                        }
                        break;
                    case PlayerState.PlayerTwo:
                        var isLaserTwoKeyDown = Input.GetButtonDown("Laser2");
                        if (isLaserTwoKeyDown)
                        {
                            isFiring = true;
                            rig.simulated = false;
                            Rigidbody2D[] rigs = tail.GetComponentsInChildren<Rigidbody2D>();
                            foreach (Rigidbody2D r in rigs)
                            {
                                r.simulated = false;
                            }
                            laserState = LaserState.Charging;
                            EventManager.TriggerEvent("AttackJ2");
                            GameObject charge = GameObject.Instantiate(chargeAnim);
                            charge.transform.position = chargeAnimPos.position;
                            charge.transform.rotation = transform.rotation;
                            charge.GetComponent<Animator>().SetTrigger("Start");
                        }
                        break;
                }
                break;
            case LaserState.Charging:
                chargeCD -= Time.deltaTime;
                if(chargeCD <= 0.0f)
                {
                    chargeCD = chargeTime;
                    laserState = LaserState.Firing;
                    laser.Fire();
                }
                break;
            case LaserState.Firing:
                laserTime += Time.deltaTime;
                if(laserTime >= laserDuration)
                {
                    Rigidbody2D[] rigs = tail.GetComponentsInChildren<Rigidbody2D>();
                    foreach (Rigidbody2D r in rigs)
                    {
                        r.simulated = true;
                    }
                    rig.simulated = true;
                    laserTime = 0.0f;
                    laserState = LaserState.Cooldown;
                    isFiring = false;
                    laser.Stop();
                }
                break;
            case LaserState.Cooldown:
                laserCD -= Time.deltaTime;
                if(laserCD <= 0.0f)
                {
                    laserCD = laserCooldown;
                    laserState = LaserState.Ready;
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FlyingAsteroid") && !isShielding)
        {
            ContactPoint2D contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            GameObject boom = GameObject.Instantiate(collisionAnim);
            boom.transform.position = pos;
            boom.transform.rotation = rot;
            boom.GetComponent<Animator>().SetTrigger("StartHit*");
        }
    }
}

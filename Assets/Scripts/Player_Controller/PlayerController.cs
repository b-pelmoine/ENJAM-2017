using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {

    private float speed;
    public Boundary boundary;

    private GameManager gameManager;
    private int health;
    private Rigidbody2D rig;
    private Vector3 _origPos;
    private float dashSpeed;
    private float dashDistance;
    private float dashCooldown;
    private Vector2 savedVelocity;
    private DashState dashState;
    private float dashTime;
    private float dashCD;
    private bool dashing;

    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    private void Start()
    {
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

    }

    private void Update()
    {
        Dash(dashSpeed, dashDistance, dashCooldown);

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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

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
        health -= damages;
        if (health <= 0) Kill();
    }

    private void Kill()
    {
        //play anim death etc
        Debug.Log("Mort");
    }

    private void Dash(float speed, float distance, float cooldown)
    {
        switch (dashState)
        {
            case DashState.Ready:
                var isDashKeyDown = Input.GetButtonDown("Dash");
                if (isDashKeyDown)
                {
                    Debug.Log("dash");
                    dashing = true;
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                dashTime += Time.deltaTime * 3.0f;
                if (dashTime >= distance)
                {
                    Debug.Log("dashing");
                    dashTime = 0.0f;
                    dashing = false;
                    dashState = DashState.Cooldown;
                }
                break;
            case DashState.Cooldown:
                dashCD -= Time.deltaTime;
                if (dashCD <= 0)
                {
                    Debug.Log("dashCD");
                    dashCD = dashCooldown;
                    dashState = DashState.Ready;
                }
                break;
        }
    }
}

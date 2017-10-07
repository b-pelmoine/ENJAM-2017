using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    //public float tilt;
    public Boundary boundary;

    private GameManager gameManager;
    private int health;
    private Rigidbody2D rig;
    private Vector3 _origPos;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        _origPos = transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        health = gameManager.maxHP;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rig.velocity = movement * speed;

        rig.position = new Vector2
        (
            Mathf.Clamp(rig.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rig.position.y, boundary.yMin, boundary.yMax)
        );

        Vector3 moveDirection = gameObject.transform.position - _origPos;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        //rig.rotation = rig.velocity.x * -tilt;

        _origPos = transform.position;
    }
}

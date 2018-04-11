using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxSpeed = 3;
    public float speed = 5f;
    public float jumpPower = 150f;

    public bool grounded;
    public bool falling;
    public bool canDoubleJump;

    private Rigidbody2D rb2d;
    private Animator anim;

	void Start ()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Animation conditions
        anim.SetBool("Falling", falling);
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));

        // Fall condition
        if (rb2d.velocity.y < 0)
        {
            falling = true;
        }
        if (rb2d.velocity.y >= 0)
        {
            falling = false;
        }

        // Sprite flip
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

    void FixedUpdate ()
    {

        // False friction
        Vector3 easeVelocity = rb2d.velocity;

        easeVelocity.x *= 0.75f;
        easeVelocity.y = rb2d.velocity.y;
        easeVelocity.z = 0.0f;

        if (grounded)
        {
            rb2d.velocity = easeVelocity;
        }

        // Player movement
        float h = Input.GetAxis("Horizontal");
        rb2d.AddForce((Vector2.right * speed) * h);

        // Player speed limit
        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rb2d.AddForce(Vector2.up * jumpPower);
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    rb2d.AddForce(Vector2.up * jumpPower / 1.75f);
                }
            }
        }
    }
}

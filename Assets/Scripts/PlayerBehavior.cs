﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
	Animator animator;
	Rigidbody2D rigidbody2D;
	bool leftScale = true;
	bool startedJumpTimer = false;
	float jumpTimeLeft = 0;
    bool hasJetpack = false;

	public Camera camera;
	public float walkForce = 15000;
	public float jumpForce = 15000;
    public float maxSpeedX = 50;
    public float maxSpeedY = 80;
    public string horizontalAxisName = "Horizontal";
	public string verticalAxisName = "Vertical";
	public float jumpMaxTime = 0.1f;

	void Start()
	{
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		camera.transform.position = new Vector3 (rigidbody2D.transform.position.x, rigidbody2D.transform.position.y, -15.0f);
	}
	
	void FixedUpdate()
	{
		float axisH = Input.GetAxis(horizontalAxisName);
		float axisV = Input.GetAxis(verticalAxisName);

		bool wannaJump = (axisV > 0);

		if (leftScale && axisH < 0) {
			transform.localScale = new Vector2 (transform.localScale.x * -1.0f, transform.localScale.y);
			leftScale = !leftScale;
		} else if (!leftScale && axisH > 0) {
			transform.localScale = new Vector2 (transform.localScale.x * -1.0f, transform.localScale.y);
			leftScale = !leftScale;
		}

		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * -1, 0.3f, ~(LayerMask.GetMask("Player")));
		bool onGround = (hit.collider != null);

		//print (hit.fraction + " " + hit.point + "\n");
		Debug.DrawRay(transform.position, transform.TransformDirection (Vector3.up) * -0.3f, onGround ? Color.green : Color.red);

        /*if (wannaJump) {
			if (!startedJumpTimer && onGround) {
				startedJumpTimer = true;
				jumpTimeLeft = jumpMaxTime;
			} else {
				jumpTimeLeft -= Time.fixedDeltaTime;
			}

			if (jumpTimeLeft > 0) {
				rigidbody2D.AddForce (transform.up * axisV * jumpForce);
			}
		} else {
			if (onGround) {
				startedJumpTimer = false;
				jumpTimeLeft = 0;
			}
		}*/

        if (onGround || hasJetpack)
        {
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            rigidbody2D.AddForce(transform.up * axisV * jumpForce);
        }

        if (Mathf.Abs (axisH) > 0) {
			rigidbody2D.AddForce(transform.right * axisH * walkForce);
		}

        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxSpeedX, maxSpeedX), Mathf.Clamp(rigidbody2D.velocity.y, -maxSpeedY, maxSpeedY));

        animator.SetFloat("velocity", Mathf.Abs(axisH));
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Detected" + other.gameObject.tag);

        if (other.gameObject.CompareTag ("PickUp"))
		{           
			other.transform.SetParent (transform);
            other.transform.localPosition = new Vector3(-0.01399994f, 0.1879997f, 1);
            other.transform.localScale = new Vector2(1, 1);
            other.transform.rotation = new Quaternion(0, 0, 0, 0);
            other.transform.localRotation = new Quaternion(0, 0, 0, 0);
            other.GetComponent<Rigidbody2D>().simulated = false;
            hasJetpack = true;
        }
	}
}

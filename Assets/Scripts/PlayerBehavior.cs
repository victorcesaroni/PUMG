using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
	Animator animator;
	Rigidbody2D rigidbody2D;
	bool leftScale = true;
	bool startedJumpTimer = false;
	float jumpTimeLeft = 0;
    bool hasJetpack = false;

    public bool ducking = false;

	public Camera camera;
	public float walkSpeed = 30;
	public float jumpSpeed = 45;
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
		//Debug.DrawRay(transform.position, transform.TransformDirection (Vector3.up) * -0.3f, onGround ? Color.green : Color.red);

        if (onGround || hasJetpack)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, axisV * jumpSpeed);
        }

        if (Mathf.Abs (axisH) > 0) {
            rigidbody2D.velocity = new Vector2(axisH * walkSpeed, rigidbody2D.velocity.y);
        }

        ducking = axisV < 0;

        animator.SetFloat("velocity", onGround ? Mathf.Abs(axisH) : 0.0f);
	}

	void OnTriggerEnter2D(Collider2D other)
    {
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

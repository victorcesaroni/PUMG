using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour {
	Animator animator;
	Rigidbody2D rb;
	bool leftScale = true;

    public bool ducking = false;

	public Camera followCamera;
	public float walkSpeed = 30;
	public float jumpSpeed = 45;
    public float walkSpeedVariation = 7.0f;
    public string horizontalAxisName = "Horizontal";
	public string verticalAxisName = "Vertical";

	public Image powerImageBar;
	public List<Pickup> pickups;
    
	void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
        Pickup power = GetUsablePower();

		followCamera.transform.position = new Vector3 (rb.transform.position.x, rb.transform.position.y, followCamera.transform.position.z);

        if (power)
        {
            powerImageBar.fillAmount = power.power;
        }
        else
        {
            powerImageBar.fillAmount = 0;
        }
    }

    Pickup GetUsablePower()
    {
        if (pickups.Count <= 0)
            return null;

        Pickup p = pickups[pickups.Count - 1];

        if (p.power <= 0.0f)
        {
            pickups.Remove(p);
            return null;
        }

        return p;
    }

    void FixedUpdate()
    {
        Pickup power = GetUsablePower();

        float axisH = Input.GetAxis(horizontalAxisName);
        float axisV = Input.GetAxis(verticalAxisName);

        Physics2D.queriesHitTriggers = false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * -1, 0.3f, ~(LayerMask.GetMask("Player")));
        bool onGround = (hit.collider != null);

        hit = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, ~(LayerMask.GetMask("Player")));
        bool hitLeft = (hit.collider != null);
        hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.9f, 0), Vector2.left, 0.5f, ~(LayerMask.GetMask("Player")));
        hitLeft |= hit.collider != null;

        hit = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, ~(LayerMask.GetMask("Player")));
        bool hitRight = (hit.collider != null);
        hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.9f, 0), Vector2.right, 0.5f, ~(LayerMask.GetMask("Player")));
        hitRight |= hit.collider != null;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 0.5f, hitLeft ? Color.green : Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 0.5f, hitRight ? Color.green : Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, 0.9f, 0), transform.TransformDirection(Vector3.left) * 0.5f, hitLeft ? Color.green : Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, 0.9f, 0), transform.TransformDirection(Vector3.right) * 0.5f, hitRight ? Color.green : Color.red);

        if (axisH < 0 && hitLeft)
            axisH = 0;
        if (axisH > 0 && hitRight)
            axisH = 0;

        if (leftScale && axisH < 0) {
			transform.localScale = new Vector2 (transform.localScale.x * -1.0f, transform.localScale.y);
			leftScale = !leftScale;
		} else if (!leftScale && axisH > 0) {
			transform.localScale = new Vector2 (transform.localScale.x * -1.0f, transform.localScale.y);
			leftScale = !leftScale;
		}

        //print (hit.fraction + " " + hit.point + "\n");
        //Debug.DrawRay(transform.position, transform.TransformDirection (Vector3.up) * -0.3f, onGround ? Color.green : Color.red);

        Jetpack jetpack = power != null ? power.GetJetpack() : null;
        
        if (jetpack && jetpack.active)
        {
            rb.velocity = new Vector2(rb.velocity.x, axisV * jumpSpeed + axisV * (power.level * (jumpSpeed / 2.0f)));
            jetpack.Consume(onGround, Mathf.Abs(axisH) > 0 || Mathf.Abs(axisV) > 0);            
        }    
        else if (onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, axisV * jumpSpeed);
        }

        if (Mathf.Abs(axisH) > 0)
        {
            rb.velocity = new Vector2(axisH * (walkSpeed + Random.Range(-walkSpeedVariation, walkSpeedVariation)), rb.velocity.y);
        }

        ducking = axisV < 0;

        animator.SetFloat("velocity", onGround ? Mathf.Abs(axisH) : 0.0f);		
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag ("PickUp"))
		{
            if (pickups.Count < 3)
            {
                other.transform.SetParent(transform);
                other.transform.localPosition = new Vector3(-0.01399994f, 0.1879997f, 1);
                other.transform.localScale = new Vector2(1, 1);
                other.transform.rotation = new Quaternion(0, 0, 0, 0);
                other.transform.localRotation = new Quaternion(0, 0, 0, 0);

                Pickup pickup = other.GetComponent<Pickup>();
                pickup.active = true;
                pickups.Add(pickup);
            }
        }
	}
}

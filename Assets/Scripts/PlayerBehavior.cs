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
    public string usePowerAxisName = "UsePower";

    public Image powerImageBar;

    public List<Pickup> pickups = new List<Pickup>();

    public GameObject enemy = null;
    private bool blockedFromUsingPower = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Pickup power = GetUsablePower();

        followCamera.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, followCamera.transform.position.z);

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

        if (blockedFromUsingPower)
            return null;

        Pickup p = pickups[pickups.Count - 1];

        if (p.power <= 0.0f)
        {
            pickups.Remove(p);
            blockedFromUsingPower = true;
            return null;
        }

        return p;
    }

    void FixedUpdate()
    {
        Pickup power = GetUsablePower();

        float axisH = Input.GetAxis(horizontalAxisName);
        float axisV = Input.GetAxis(verticalAxisName);
        bool usePower = Input.GetAxis(usePowerAxisName) > 0.1f;

        Physics2D.queriesHitTriggers = false;

        var bounds = GetComponent<CapsuleCollider2D>().bounds;
        Vector3 bottom = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        Vector3 top = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
        Vector3 mid = new Vector3(bounds.center.x, bounds.min.y + (bounds.max.y - bounds.min.y) / 2, bounds.center.z);
        float width = (bounds.max.x - bounds.min.x);

        RaycastHit2D hit = Physics2D.Raycast(bottom, Vector2.up * -1, 0.3f, ~(LayerMask.GetMask("Player")));
        bool onGround = (hit.collider != null);

        bool hitLeft =
            Physics2D.Raycast(bottom, Vector2.left, width * 0.9f, ~(LayerMask.GetMask("Player"))).collider != null ||
            Physics2D.Raycast(top, Vector2.left, width * 0.9f, ~(LayerMask.GetMask("Player"))).collider != null ||
            Physics2D.Raycast(mid, Vector2.left, width * 0.9f, ~(LayerMask.GetMask("Player"))).collider != null;

        bool hitRight =
            Physics2D.Raycast(bottom, Vector2.right, width * 0.9f, ~(LayerMask.GetMask("Player"))).collider != null ||
            Physics2D.Raycast(top, Vector2.right, width * 0.9f, ~(LayerMask.GetMask("Player"))).collider != null ||
            Physics2D.Raycast(mid, Vector2.right, width * 0.9f, ~(LayerMask.GetMask("Player"))).collider != null;

        Debug.DrawRay(bottom, transform.TransformDirection(Vector3.up) * -0.3f, onGround ? Color.green : Color.red);

		Debug.DrawRay(bottom, transform.TransformDirection(Vector3.left) * width * 0.9f, hitLeft ? Color.green : Color.red);
		Debug.DrawRay(top, transform.TransformDirection(Vector3.left) * width * 0.9f, hitLeft ? Color.green : Color.red);
		Debug.DrawRay(mid, transform.TransformDirection(Vector3.left) * width * 0.9f, hitLeft ? Color.green : Color.red);
        Debug.DrawRay(bottom, transform.TransformDirection(Vector3.right) * width * 0.9f, hitRight ? Color.green : Color.red);
        Debug.DrawRay(top, transform.TransformDirection(Vector3.right) * width * 0.9f, hitRight ? Color.green : Color.red);
        Debug.DrawRay(mid, transform.TransformDirection(Vector3.right) * width * 0.9f, hitRight ? Color.green : Color.red);

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

        if (!usePower)
            blockedFromUsingPower = false;

        Jetpack jetpack = power != null ? power.GetAs<Jetpack>() : null;
        Portal portal = power != null ? power.GetAs<Portal>() : null;
        Ice ice = power != null ? power.GetAs<Ice>() : null;

        if (portal && usePower)
        {
            portal.Consume(onGround, true);
        }
        else if (ice && usePower)
        {
            ice.Consume(onGround, true);
        }

        if (jetpack && jetpack.active && usePower)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(axisV, 1,1) * jumpSpeed + Mathf.Clamp(axisV, 1, 1) * (power.level * (jumpSpeed / 2.0f)));
            jetpack.Consume(onGround, /*Mathf.Abs(axisH) > 0 || Mathf.Abs(axisV) > 0*/true);            
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

        animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityY", (rb.velocity.y));
        animator.SetBool("onGround", onGround);
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            if (pickups.Count < 3)
            {
                Pickup pickup = other.GetComponent<Pickup>();

                if (pickup.owner == null)
                {
                    other.transform.SetParent(transform);
                    other.transform.localPosition = new Vector3(0, 0, 0);
                    other.transform.localScale = new Vector2(1, 1);
                    other.transform.rotation = new Quaternion(0, 0, 0, 0);
                    other.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    pickup.active = true;
                    pickup.owner = this;
                    
                    Portal portal = pickup != null ? pickup.GetAs<Portal>() : null;
                    Ice ice = pickup != null ? pickup.GetAs<Ice>() : null;

                    if (ice) ice.enemy = enemy;
                    if (portal) portal.enemy = enemy;

                    pickups.Add(pickup);
                }
            }
        }
	}
}

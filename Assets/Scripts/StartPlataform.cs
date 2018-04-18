using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlataform : MonoBehaviour
{

    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public float cooldown = 8.0f;
    public GameObject teleportObject;
    public List<GameObject> disableList;

    public float cooldownTimer = 0;
    public bool started = false;

    List<GameObject> players = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (started)
            return;

        if (players.Count != 2)
        {
            cooldownTimer = cooldown;
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0)
        {
            camera1.gameObject.SetActive(false);
            camera2.gameObject.SetActive(false);
            camera3.gameObject.SetActive(true);

            foreach (var player in players)
            {
                player.transform.position = teleportObject.transform.position + new Vector3(0, Random.Range(5, 10), 0);
                player.transform.position = teleportObject.transform.position + new Vector3(0, Random.Range(5, 10), 0);
            }

            foreach (var item in disableList)
            {
                item.SetActive(false);
            }

            started = true;
        }

        var sprite = GetComponent<SpriteRenderer>();

        sprite.color = new Color(sprite.color.r, 1 - (cooldownTimer / cooldown), sprite.color.b);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (started)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            players.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (started)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            players.Remove(other.gameObject);
        }
    }
}

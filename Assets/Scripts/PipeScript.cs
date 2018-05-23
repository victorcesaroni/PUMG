using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour {

    public List<GameObject> possiblePipes;
    public bool available = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!available || possiblePipes.Count <= 0)
            return;

        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            var player = other.GetComponent<PlayerBehavior>();

            if (player.ducking)
            {
                int index = Mathf.FloorToInt(Random.Range(0, (float)possiblePipes.Count));
                var pipe = possiblePipes[index].GetComponent<PipeScript>();

                player.transform.position = pipe.transform.position + new Vector3(0, 3, 0);
                
                available = false;
            }
        }
    }
 }

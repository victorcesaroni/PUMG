using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public bool allowOnce = true;
    public GameObject tpObject;
    public List<GameObject> disableList;
    public List<GameObject> enableList;

    bool locked = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (locked)
            return;

        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            foreach (var item in disableList)
            {
                item.SetActive(false);
            }

            foreach (var item in enableList)
            {
                item.SetActive(true);
            }

            other.gameObject.transform.position = tpObject.transform.position;

            if (allowOnce)
                locked = true;
        }
    }
}

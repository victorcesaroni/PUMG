using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasScript : MonoBehaviour {

    public float velocity = 2.0f;
    public bool gasEnabled = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!gasEnabled)
            return;
        transform.localScale += new Vector3(velocity * Time.deltaTime, velocity * Time.deltaTime, 0);
	}

    public void Reset()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}

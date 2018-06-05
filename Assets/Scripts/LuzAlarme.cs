using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzAlarme : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    float fraction = 0;
    bool s = false;
    public float speed = 3.0f;

    // Update is called once per frame
    void Update () {
        if (fraction >= 1.0f)
            s = !s;
        else if (fraction <= 0.0f)
            s = !s;

        fraction += (s ? 1 : -1) * (Time.deltaTime * speed);

        var c = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, fraction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool active = false;
    public int level = 1;
    public float power = 1.0f;
    
    void Start()
    {
    }

	void Update ()
    {
		
	}

    public void Consume(float amount)
    {
        if (!active)
            return;

        power -= amount;

        if (power < 0)
        {
            active = false;
            power = 0;
        }
    }

    public Jetpack GetJetpack()
    {
        if (this.GetType() == typeof(Jetpack))
            return (Jetpack)this;

        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool active = false;
    public int level = 1;
    public float power = 1.0f;
    public Sprite icon;
    public bool pickedUp = false;
    
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

    public T GetAs<T>() where T : class
    {
        if (this.GetType() == typeof(T))
            return this as T;

        return default(T);
    }

    public Ice GetIce()
    {
        if (this.GetType() == typeof(Ice))
            return (Ice)this;

        return null;
    }

    public Portal GetPortal()
    {
        if (this.GetType() == typeof(Portal))
            return (Portal)this;

        return null;
    }
}

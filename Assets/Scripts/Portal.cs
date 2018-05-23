using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Pickup
{
    public GameObject enemy = null;

    public void Consume(bool onGround, bool moving)
    {
        if (!active)
            return;

        owner.transform.position = enemy.transform.position;

        base.Consume(1);
    }
}

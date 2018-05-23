using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Pickup
{
    public GameObject enemy = null;

    public void Consume(bool onGround, bool moving)
    {
        if (!active)
            return;

        var player = enemy.GetComponent<PlayerBehavior>();

        player.pickups.Clear();

        base.Consume(1);
    }
}

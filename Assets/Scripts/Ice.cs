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
        
        if (player.pickups.Count > 0)
            player.pickups.RemoveAt(Random.Range(0, player.pickups.Count - 1));

        base.Consume(1);
    }
}

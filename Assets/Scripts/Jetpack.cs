using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : Pickup
{
    public void Consume(bool onGround, bool moving)
    {
        if (!active)
            return;

        if (onGround)
        {
            base.Consume(0.0002f / 2);
        }
        else
        {
            base.Consume(0.002f / 2);

            if (moving)
                base.Consume(0.003f / 2);            
        }
    }
}

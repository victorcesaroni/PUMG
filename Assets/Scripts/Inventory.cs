//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Sprite emptyIcon;
    public GameObject playerObject;

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        PlayerBehavior player = playerObject.GetComponent<PlayerBehavior>();
        
        var ts = GetComponentsInChildren<Image>();
        for (int i = 0; i < ts.Length; i++)
        {
            var t = ts[i];

            if (i >= player.pickups.Count)
            {
                t.sprite = emptyIcon;
                continue;
            }
            
            if (t.gameObject.name == "Slot" + (i + 1).ToString())
            {
                t.sprite = player.pickups[i].icon;
            }
        }
    }
}

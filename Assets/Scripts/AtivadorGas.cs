using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorGas : MonoBehaviour
{
    public GameObject gas = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        gas.GetComponent<GasScript>().gasEnabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gas.GetComponent<GasScript>().gasEnabled = true;
    }
}

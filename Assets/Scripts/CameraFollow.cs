using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public List<PlayerBehavior> players;
    Vector3 targetPos;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 medium = new Vector3(0, 0, 0);
        Vector3 higher = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        foreach (var player in players)
        {
            medium += player.transform.position;
            higher = (higher.y > player.transform.position.y) ? higher : player.transform.position;
        }

        medium /= players.Count;
        medium.z = transform.position.z;
        higher.z = transform.position.z;

        if ((higher - medium).magnitude < 10.0f)
            targetPos = medium;
        else
            targetPos = higher;

        float delta = (transform.position - targetPos).magnitude;

        transform.position = Vector3.Lerp(transform.position, targetPos, delta * 0.3f / 30.0f);
    }
}

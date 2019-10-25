using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothTime;
    float yVelocity;
    float xVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.y = Mathf.SmoothDamp(transform.position.y, player.position.y, ref yVelocity, smoothTime);
        position.x = Mathf.SmoothDamp(transform.position.x, player.position.x, ref xVelocity, smoothTime);

        transform.position = position;
    }
}

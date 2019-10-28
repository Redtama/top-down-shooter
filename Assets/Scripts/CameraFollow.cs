using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float smoothTime;
    public float lookAheadDistance;

    private Vector3 currentVelocity;
    private Vector3 offset;
    private Vector3 playerPos;
    private Vector3 target;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 0, transform.position.z - player.transform.position.z);
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

        Vector2 moveDir = playerController.input;
        Vector2 lookDir = player.transform.up;

        target = playerPos + offset + player.transform.up * lookAheadDistance;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothTime);
    }
}

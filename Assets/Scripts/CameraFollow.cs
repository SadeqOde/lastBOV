using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField] private float followSpeed = 5f;

    private void Update()
    {
        if (playerTransform != null)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        // Get the player's position
        Vector3 playerPos = playerTransform.position;

        // Adjust the camera's position
        Vector3 newPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);

        // Move the camera
        transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}

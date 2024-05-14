using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnArea : MonoBehaviour
{
    private GameObject player;
    float moveDist = 19.5f;
    float moveFlag = 13f;
    void Update()
    {
        if (player == null) 
        {
            player = GameObject.FindWithTag("Player");
        } else 
        {
            float distanceBetween = transform.position.x - player.transform.position.x;
            if (distanceBetween <= -moveFlag) 
            {
                transform.position = new Vector3(transform.position.x + moveDist, transform.position.y, transform.position.z);
            } else if (distanceBetween >= moveFlag) 
            {
                transform.position = new Vector3(transform.position.x - moveDist, transform.position.y, transform.position.z);
            }
        }
    }
}

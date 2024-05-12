using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject player;
    public CinemachineVirtualCamera virtualCamera;

    private float duration;
    private bool timerActive = false;

    void Start()
    {
        GameObject newPlayer = Instantiate(player, respawnPoint.position, respawnPoint.rotation);
        virtualCamera.Follow = newPlayer.transform;
    }

    void Update()
    {
        if (timerActive) 
        {
            duration -= Time.deltaTime;
            if (duration <= 0) 
            {
                GameObject newPlayer = Instantiate(player, respawnPoint.position, respawnPoint.rotation);
                virtualCamera.Follow = newPlayer.transform;
                timerActive = false;
            }
        }
    }

    public void RespawnPlayer()
    {
        timerActive = true;
        duration = 1f;
    }
}

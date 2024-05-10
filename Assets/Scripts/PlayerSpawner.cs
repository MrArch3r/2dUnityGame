using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject player;
    public CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        GameObject newPlayer = Instantiate(player, respawnPoint.position, respawnPoint.rotation);

        virtualCamera.Follow = newPlayer.transform;
    }
}

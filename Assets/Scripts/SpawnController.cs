using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject playerPrefab;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // No player yet
        if (player == null)
        {
            SpawnPlayer();
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        // Player has used a door
        if (playerController.destinationDoor > 0)
        {
            Vector3 newPosition = new Vector3();
            bool doorFound = false;
            foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door"))
            {
                DoorController doorController = door.GetComponent<DoorController>();
                if (doorController.doorID == playerController.destinationDoor)
                {
                    newPosition = doorController.spawnpoint.transform.position;
                    doorFound = true;
                }
            }

            // Invalid door
            if (!doorFound)
            {
                playerController.destination.position = transform.position;

            }
            else
            {
                playerController.destination.position = newPosition;
            }

            playerController.moveImmediately = true;
            playerController.destinationDoor = -1;
        }
    }

    private void SpawnPlayer()
    {
        // Spawn player at the spawn point
        Instantiate(playerPrefab, null, transform);
    }
}

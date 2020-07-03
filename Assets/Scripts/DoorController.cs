using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    // Open the door while player is within this radius
    public float openingRadius;

    // Speed of the door opening
    public float openingSpeed = 1;
    
    // Position where the player will spawn after moving through the door
    public Transform spawnpoint;

    // Unique ID of this door
    public int doorID;

    // Destination door
    public int destinationID;
    public string destinationScene;

    private PlayerController player;
    private Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        doorAnimator = GetComponent<Animator>();

        spawnpoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);
        UpdateAnimator(playerDistance);
    }

    private void UpdateAnimator(float distance)
    {
        bool playerNear = distance < openingRadius;
        doorAnimator.SetBool("player_near", playerNear);
    }
}

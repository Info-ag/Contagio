using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Open the door while player is within this radius
    public float openingRadius;

    // Speed of the door opening
    public float openingSpeed = 1;

    private PlayerController player;
    private Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        bool playerNear = Vector2.Distance(transform.position, player.transform.position) < openingRadius;
        doorAnimator.SetBool("player_near", playerNear);
    }
}

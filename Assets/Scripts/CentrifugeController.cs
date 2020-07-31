using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentrifugeController : MonoBehaviour, IInteractable
{
    public float ticksUntilComplete;
    public int samplesRequired;
    public int cureReturned;

    public bool running = false;
    public float progress = 0;

    private Animator animator;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Ticked && running)
        {
            // Calculate progress
            progress = Mathf.Clamp(progress + ((1 / ticksUntilComplete) * 100), 0, 100);
        }

        // Set new animation state
        animator.SetBool("running", running);
        animator.SetInteger("progress_percent", (int)progress);
    }

    public bool Interact(PlayerController player)
    {
        if (!running && player.collectedSamples >= samplesRequired)
        {
            player.collectedSamples -= samplesRequired;
            running = true;

            return true;
        }

        else if (running && progress >= 100)
        {
            player.synthesizedCure += cureReturned;
            running = false;
            progress = 0;

            return true;
        }

        else
        {
            return false;
        }

    }
}

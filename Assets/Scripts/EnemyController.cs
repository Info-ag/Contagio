using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public MovementMode infectedMode;
    public MovementMode healedMode;
    public Transform destination;
    public LayerMask collisionMask;

    public int maxInfection;
    public int currentInfection;
    public int reinfectionPerTick;

    private PlayerController player;
    private MovementMode mode;

    // Start is called before the first frame update
    void Start()
    {
        destination.parent = null;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.white, (float)currentInfection / (float)maxInfection);

        if (player.Ticked)
        {
            UpdateInfection();
            UpdatePosition();
        }
    }

    // Heals the enemy. Set reinfection to 0 for permanent healing. Returns the amount of infection healed. Enemy cannot be healed unless at full infection.
    public int Heal(int reinfection)
    {
        if (currentInfection != maxInfection)
        {
            return 0;
        }

        currentInfection = 0;
        reinfectionPerTick = reinfection;
        return maxInfection;
    }

    private void UpdateInfection()
    {
        currentInfection = Mathf.Clamp(currentInfection + reinfectionPerTick, 0, maxInfection);
        mode = (currentInfection < maxInfection) ? healedMode : infectedMode;
    }

    private void UpdatePosition()
    {
        Vector3 movementVector = new Vector3();
        if (mode == MovementMode.random)
        {
            movementVector = RandomDirection();
        }
        else if (mode == MovementMode.targetingPlayer)
        {
            movementVector = TargetedDirection();
        }
        else if (mode == MovementMode.frozen)
        {
            movementVector = transform.position;
        }

        if (!Physics2D.OverlapCircle(movementVector, 0.2f, collisionMask) && !DestinationConflict(movementVector))
        {
            // Do not update the destination transform if there are obstacles
            destination.position = movementVector;
        }
    }

    private Vector3 RandomDirection()
    {
        float[] directions = { -1, 0, 1 };
        float direction_x = directions[Random.Range(0, directions.Length)];
        float direction_y = directions[Random.Range(0, directions.Length)];

        return new Vector3(direction_x, direction_y) + destination.position;
    }

    private Vector3 TargetedDirection()
    {
        Vector3 optimalMove = new Vector3();
        float shortestDistance = float.MaxValue;

        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y) 
            {
                Vector3 movement = new Vector3(x, y) + destination.position;
                float distance = Vector3.Distance(player.transform.position, movement);

                if (distance < shortestDistance)
                {
                    optimalMove = movement;
                    shortestDistance = distance;
                }
            }
        }

        return optimalMove;
    }

    // Returns true if another entity wants to move to the destination
    private bool DestinationConflict(Vector3 tile)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Destination");
        foreach (var currentObject in objects)
        {
            if (currentObject.transform.position.Equals(tile) && !currentObject.Equals(destination.gameObject))
            {
                return true;
            }
        }

        return false;
    }

    public enum MovementMode { frozen, random, targetingPlayer };
}

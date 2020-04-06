using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform destination;
    public LayerMask collisionMask;

    public int syringeLevel;
    public int syringeBaseEffectivity;

    public bool Ticked { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        destination.parent = null;
        Ticked = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);

        if (AtDestination())
        {
            Ticked = UpdateDestination();
            if (Ticked)
            {
                Debug.Log("Ticked");
            }
        }
        else
        {
            Ticked = false;
        }
    }

    private bool UpdateDestination()
    {
        Vector3 movementVector = destination.position;
        bool moved = false;

        if (Input.GetKey(KeyCode.W))
        {
            movementVector += new Vector3(0, 1);
            moved = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementVector += new Vector3(0, -1);
            moved = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementVector += new Vector3(-1, 0);
            moved = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementVector += new Vector3(1, 0);
            moved = true;
        }

        // Do not update the destination transform if there are obstacles
        if (!Physics2D.OverlapCircle(movementVector, 0.2f, collisionMask) && !DestinationConflict(movementVector))
        {
            destination.position = movementVector;
        }
        // Check if the obstacle is a healable enemy
        else
        {
            moved = CheckAndHeal(movementVector);
        }

        return moved;
    }

    // Check if there is an enemy that could be healed at a given tile, heal it if true, return false if there is no enemy
    private bool CheckAndHeal(Vector3 tile)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.position == tile)
            {
                EnemyController controller = enemy.GetComponent<EnemyController>();
                int healedAmount = controller.Heal(syringeBaseEffectivity / syringeLevel);

                return healedAmount > 0;
            }
        }

        return false;
    }

    public bool AtDestination()
    {
        return Vector3.Distance(transform.position, destination.position) < 0.05;
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
}

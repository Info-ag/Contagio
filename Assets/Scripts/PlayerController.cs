using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform destination;
    public LayerMask collisionMask;

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

        if (!Physics2D.OverlapCircle(movementVector, 0.2f, collisionMask) && !DestinationConflict(movementVector))
        {
            // Do not update the destination transform if there are obstacles
            destination.position = movementVector;
        }
        else
        {
            moved = false;
        }

        return moved;
    }

    public bool AtDestination()
    {
        return Vector3.Distance(transform.position, destination.position) < 0.05;
    }

    // Returns true is another entity wants to move to the destination
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

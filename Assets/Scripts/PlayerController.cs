using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform destination;
    public LayerMask collisionMask;

    // Start is called before the first frame update
    void Start()
    {
        destination.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);

        if (AtDestination())
        {
            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        Vector3 movementVector = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            movementVector = new Vector3(0, 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementVector = new Vector3(0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementVector = new Vector3(-1, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementVector = new Vector3(1, 0);
        }

        movementVector += destination.position;
        if (!Physics2D.OverlapCircle(movementVector, 0.2f, collisionMask))
        {
            // Do update the destination transform if there are obstacles
            destination.position = movementVector;
        }
    }

    public bool AtDestination()
    {
        return Vector3.Distance(transform.position, destination.position) < 0.05;
    }
}

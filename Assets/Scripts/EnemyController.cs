using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public Transform destination;
    public LayerMask collisionMask;

    private PlayerController player;

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

        if (player.Ticked)
        {
            Vector3 movementVector = RandomDirection() + destination.position;

            if (!Physics2D.OverlapCircle(movementVector, 0.2f, collisionMask))
            {
                // Do not update the destination transform if there are obstacles
                destination.position = movementVector;
            }
        }
    }

    private Vector3 RandomDirection()
    {
        float[] directions = { -1, 0, 1 };
        float direction_x = directions[Random.Range(0, directions.Length)];
        float direction_y = directions[Random.Range(0, directions.Length)];

        return new Vector3(direction_x, direction_y);
    }
}

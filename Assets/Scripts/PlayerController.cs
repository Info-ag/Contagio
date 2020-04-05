using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform destination;

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
        if (Input.GetKey(KeyCode.W))
        {
            destination.position += new Vector3(0, 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            destination.position += new Vector3(0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            destination.position += new Vector3(-1, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            destination.position += new Vector3(1, 0);
        }
    }

    public bool AtDestination()
    {
        return Vector3.Distance(transform.position, destination.position) < 0.05;
    }
}

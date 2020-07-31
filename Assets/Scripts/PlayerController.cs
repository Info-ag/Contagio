using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform destination;
    public bool moveImmediately;
    public LayerMask collisionMask;

    public int syringeLevel;
    public int syringeBaseEffectivity;

    public int maxInfection;
    public int currentInfection;
    public int collectedSamples;
    public int synthesizedCure;

    public UIController uiController;

    // Set to -1 for no destination door
    public int destinationDoor = -1;

    public bool Ticked { get; private set; }
    public int currentTick;

    private float rotation = 0;

    private void Awake()
    {
        // Keep player data between scenes
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // Add the callback
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        // Remove the callback when not needed anymore
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "GameOver")
        {
            // Refresh the UI Controller reference
            uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        destination.parent = null;
        Ticked = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateUI();

        if (AtDestination())
        {
            Ticked = UpdateInput();
            if (Ticked)
            {
                currentTick += 1;
                Debug.Log("Ticked");
            }
        }
        else
        {
            Ticked = false;
        }
    }

    private void UpdateUI()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, (float)currentInfection / (float)maxInfection);

        uiController.Samples = collectedSamples;
        uiController.Infection = (float)currentInfection / (float)maxInfection;
        uiController.Cure = synthesizedCure;
    }

    private void UpdateMovement()
    {
        if (moveImmediately)
        {
            transform.position = destination.position;
            moveImmediately = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
        }

        if (transform.position != destination.position)
        {
            Vector2 rotationVector = destination.position - transform.position;
            rotation = Quaternion.LookRotation(Vector3.forward, rotationVector).eulerAngles.z;
        }

        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private bool UpdateInput()
    {
        // Self-heal
        if (Input.GetKeyDown(KeyCode.E))
        {
            return SelfHeal();
        }
        else
        {
            return UpdateDestination();
        }
    }

    private bool UpdateDestination()
    {
        Vector3 movementVector = destination.position;
        bool moved = false;

        if (Input.GetKeyDown(KeyCode.W))
        {
            movementVector += new Vector3(0, 1);
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            movementVector += new Vector3(0, -1);
            moved = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            movementVector += new Vector3(-1, 0);
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            movementVector += new Vector3(1, 0);
            moved = true;
        }

        // Do not update the destination transform if there are obstacles
        if (!Physics2D.OverlapCircle(movementVector, 0.2f, collisionMask) && !DestinationConflict(movementVector))
        {
            destination.position = movementVector;
        }
        // Check if the obstacle is a healable enemy or a door
        else
        {
            moved = CheckEnemyAndHeal(movementVector) || CheckDoor(movementVector) || CheckInteractable(movementVector);
        }

        return moved;
    }

    private GameObject[] ObjectsAtTileWithTag(string tag, Vector3 tile)
    {
        List<GameObject> results = new List<GameObject>();

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject taggedObject in taggedObjects)
        {
            if (taggedObject.transform.position == tile)
            {
                results.Add(taggedObject);
            }
        }

        return results.ToArray();
    }

    // Check if there is a door at the destination; move though it if true
    private bool CheckDoor(Vector3 tile)
    {
        GameObject[] doors = ObjectsAtTileWithTag("Door", tile);

        if (doors.Count() == 0)
        {
            return false;
        }

        DoorController door = doors[0].GetComponent<DoorController>();
        destinationDoor = door.destinationID;
        SceneManager.LoadScene(door.destinationScene);

        return true;
    }

    // Check if there is an interactable person or object at the destination
    private bool CheckInteractable(Vector3 tile)
    {
        GameObject[] interactables = ObjectsAtTileWithTag("Interactable", tile);

        if (interactables.Count() == 0)
        {
            return false;
        }
        else
        {
            IInteractable controller = interactables[0].GetComponent<IInteractable>();
            return controller.Interact(this);
        }
    }

    // Check if there is an enemy that could be healed at a given tile, heal it if true, return false if there is no enemy
    private bool CheckEnemyAndHeal(Vector3 tile)
    {
        GameObject[] enemies = ObjectsAtTileWithTag("Enemy", tile);

        if (enemies.Count() == 0)
        {
            return false;
        }
        
        EnemyController controller = enemies[0].GetComponent<EnemyController>();
        int healedAmount = controller.Heal(syringeBaseEffectivity / syringeLevel);

        collectedSamples += healedAmount / 100;
        UpdateUI();

        return healedAmount > 0;
    }

    // Get infected with a certain strength as a percentage of the maximum infection
    public void Infect(float strength)
    {
        currentInfection += Mathf.RoundToInt(strength * maxInfection);
        UpdateUI();

        CheckGameOver();
    }

    // Heal yourself with one cure, returns true if successful
    private bool SelfHeal()
    {
        if (synthesizedCure > 0 && currentInfection > 0)
        {
            currentInfection = 0;
            synthesizedCure -= 1;

            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckGameOver()
    {
        if (currentInfection >= maxInfection)
        {
            // Reset the game
            SceneManager.LoadScene("GameOver");
        }
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

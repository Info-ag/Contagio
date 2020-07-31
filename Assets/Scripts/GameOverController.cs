using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public string initialScene;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        GameObject destination = playerController.destination.gameObject;

        Text gameoverText = GameObject.Find("Text").GetComponent<Text> ();

        gameoverText.text += "You survived for " + playerController.currentTick + " turns";

        Destroy(player);
        Destroy(destination);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(initialScene);
        }
    }
}

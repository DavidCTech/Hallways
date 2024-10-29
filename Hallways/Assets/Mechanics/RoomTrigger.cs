using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{    
    public HallwayController hallwayController; // Reference to the RoomSpawner script

    // Start is called before the first frame update
    void Start()
    {
        // Automatically find and reference the HallwayController in the scene
        hallwayController = FindObjectOfType<HallwayController>();

        // Optional: Log a warning if the HallwayController is not found
        if (hallwayController == null)
        {
            Debug.LogWarning("HallwayController not found in the scene. Please ensure there is a HallwayController object.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the player
        {
            hallwayController.SpawnRoom(); // Call the SpawnRoom method in the RoomSpawner script
            Destroy(gameObject);
        }
    }
}

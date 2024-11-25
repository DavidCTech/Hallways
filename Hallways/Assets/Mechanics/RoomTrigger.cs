using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour, IInteractable
{    
    public HallwayController hallwayController; // Reference to the RoomSpawner script
    private bool hasTriggered = false; // Flag to prevent multiple triggers


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

    public void Interact()
    {
        if (!hasTriggered) // Check if the colliding object is the player and if it hasn't already triggered
        {
            hallwayController.SpawnRoom(); // Call the SpawnRoom method in the HallwayController script
            hasTriggered = true; // Set the flag to true to prevent further triggers
            //Destroy(gameObject); // Destroy the trigger object
        }
    }

}

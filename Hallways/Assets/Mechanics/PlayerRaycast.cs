using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class PlayerRaycast : MonoBehaviour
{    
    public LayerMask interactMask; // Layer of the target objects
    public float rayDistance = 10f; // Distance of the raycast
    private Camera mainCamera; // Cache the main camera


    public Color rayColor = Color.red; // Color for the raycast line

    // Start is called before the first frame update
    void Start()
    {
        // Grab the main camera reference once
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Only proceed if the main camera exists
        if (mainCamera == null)
        {
            return;
        }

        // Set the origin and direction of the ray to be from the camera
        Vector3 rayOrigin = mainCamera.transform.position;
        Vector3 rayDirection = mainCamera.transform.forward;

        // Draw the raycast in the scene view
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, rayColor);

        // Check if the player presses the interact button (e.g., E key)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Perform the raycast
            Ray ray = new Ray(rayOrigin, rayDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, interactMask))
            {
                // Check if the hit object has an IInteractable component
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    // Call the Interact method on the object
                    interactable.Interact();
                }
            }
        }
    }
}

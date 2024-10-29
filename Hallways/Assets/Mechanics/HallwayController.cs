using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayController : MonoBehaviour
{
    public Transform hallwaySpawnPoint; // Position where new hallway segments spawn
    public GameObject[] regularRooms; // Array of regular room prefabs
    public GameObject[] specialRooms; // Array of special room prefabs in the order they should appear
    public GameObject lobbyRoomPrefab; // Prefab for the beginning room

    public int specialRoomInterval = 5; // Number of regular rooms before a special room

    [SerializeField]
    private int roomCounter = 0; // Counts total rooms passed
    private int specialRoomIndex = 0; // Tracks the current special room in order

    [SerializeField]
    private List<GameObject> spawnedRooms = new List<GameObject>(); // List to track spawned rooms



    // Start is called before the first frame update
    void Start()
    {
        SpawnRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SpawnRoom()
    {

        // Increment the room counter
        roomCounter++;
        GameObject newRoom;

        // Determine if we should spawn a special room
        if (roomCounter % specialRoomInterval == 0)
        {
            // Spawn the next special room in sequence
            newRoom = Instantiate(specialRooms[specialRoomIndex], hallwaySpawnPoint.position, Quaternion.identity);

            // Cycle to the next special room, looping back to the beginning if needed
            specialRoomIndex = (specialRoomIndex + 1) % specialRooms.Length;
        }
        else
        {
            // Spawn a random regular room
            int randomIndex = Random.Range(0, regularRooms.Length);
            newRoom = Instantiate(regularRooms[randomIndex], hallwaySpawnPoint.position, Quaternion.identity);
        }

        // Move the spawn point forward for the next room spawn
        hallwaySpawnPoint.position += Vector3.forward * 10f; // Adjust length to match room size

        // Add the new room to the start of the list
        spawnedRooms.Insert(0, newRoom);

        // If the list has 3 or more rooms, replace the third room with the lobby room
        if (spawnedRooms.Count >= 3)
        {
            GameObject thirdRoom = spawnedRooms[2]; // Get the third room in the list

            // Replace the third room with the lobby room
            Vector3 thirdRoomPosition = thirdRoom.transform.position;
            Destroy(thirdRoom); // Destroy the old room
            GameObject lobbyRoom = Instantiate(lobbyRoomPrefab, thirdRoomPosition, Quaternion.identity);

            // Update the third room in the list to point to the new lobby room
            spawnedRooms[2] = lobbyRoom;
        }

        if (spawnedRooms.Count >= 4)
        {
            GameObject fourthRoom = spawnedRooms[3]; // Get the fourth room in the list
            Destroy(fourthRoom); // Destroy the old room
            spawnedRooms.RemoveAt(3); // Remove the reference from the list
        }
    }

}

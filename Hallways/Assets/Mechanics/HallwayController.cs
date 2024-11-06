using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayController : MonoBehaviour
{
    public GameObject[] regularRooms; // Array of regular room prefabs
    public GameObject[] specialRooms; // Array of special room prefabs in the order they should appear
    public Transform hallwaySpawnPoint; // Position where new hallway segments spawn

    [SerializeField]
    private int roomCounter = 0; // Counts total rooms passed
    [SerializeField]
    private int specialRoomIndex = 0; // Tracks the current special room in order
    public int specialRoomInterval = 5; // Number of regular rooms before a special room

    [SerializeField]
    private List<GameObject> spawnedRooms = new List<GameObject>(); // List to track spawned rooms
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController playerController; // Reference to the player's CharacterController



    // Start is called before the first frame update
    void Start()
    {
        //SpawnRoom();
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

        // Add the new room to the start of the list
        spawnedRooms.Insert(0, newRoom);

        RoomShift();

    }

    public void RoomShift()
    {
        // Move the hallway spawn point forward by 10 units if there are fewer than 3 rooms
        if (spawnedRooms.Count < 3)
        {
            hallwaySpawnPoint.position += Vector3.forward * 10f;
        }

        // Only shift when there are exactly 3 rooms in the list
        if (spawnedRooms.Count == 3)
        {

            // Move the last two rooms back by 10 units relative to their current positions
            spawnedRooms[0].transform.position -= new Vector3(0, 0, 10);
            spawnedRooms[1].transform.position -= new Vector3(0, 0, 10);

            playerController.enabled = false;
            playerTransform.position -= new Vector3(0, 0, 10);
            playerController.enabled = true;


            // Destroy the oldest room (third in the list) and remove it
            GameObject oldestRoom = spawnedRooms[2];
            Destroy(oldestRoom);
            spawnedRooms.RemoveAt(2);

            // Move hallway spawn point back by 10 units for alignment
            //hallwaySpawnPoint.position += Vector3.back * 10f;
        }

    }
}

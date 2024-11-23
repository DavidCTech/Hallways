using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomSet
{
    public GameObject[] rooms; // Array of rooms in this set
}
public class HallwayController : MonoBehaviour
{
    public Transform hallwaySpawnPoint; // Position where new hallway segments spawn
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController playerController; // Reference to the player's CharacterController

    [SerializeField]
    private int roomCounter = 0; // Counts total rooms passed
    [SerializeField]
    private int specialRoomIndex = 0; // Tracks the current special room in order
    public int specialRoomInterval = 5; // Number of regular rooms before a special room
    public GameObject[] specialRooms; // Array of special room prefabs in the order they should appear

    [SerializeField]
    private int regularRoomSetIndex = 0; // Tracks the current regular room set
    [SerializeField]
    private List<RoomSet> regularRoomSets; // List of room sets
    [SerializeField]
    private List<GameObject> currentRoomPool = new List<GameObject>(); // Temporary pool for tracking unpicked rooms in the current set
    [SerializeField]
    private List<GameObject> spawnedRooms = new List<GameObject>(); // List to track spawned rooms



    // Start is called before the first frame update
    void Start()
    {
        InitializeRoomPool();
        SpawnRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Initialize the temporary pool with the current room set
    private void InitializeRoomPool()
    {
        currentRoomPool.Clear();
        RoomSet currentRoomSet = regularRoomSets[regularRoomSetIndex];
        currentRoomPool.AddRange(currentRoomSet.rooms);
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

            // Cycle to the next regular room set, looping back to the beginning if needed
            regularRoomSetIndex = (regularRoomSetIndex + 1) % regularRoomSets.Count;

            // Reinitialize the pool for the new regular room set
            InitializeRoomPool();
        }
        else
        {
            // If the current pool is empty, refill it with the current room set
            if (currentRoomPool.Count == 0)
            {
                InitializeRoomPool();
            }

            // Choose a random room from the pool and remove it
            int randomIndex = Random.Range(0, currentRoomPool.Count);
            newRoom = Instantiate(currentRoomPool[randomIndex], hallwaySpawnPoint.position, Quaternion.identity);
            currentRoomPool.RemoveAt(randomIndex);
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


        }

    }

}

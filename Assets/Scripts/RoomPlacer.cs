using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    public static RoomPlacer instance;
    public Room[] RoomPrefabs;
    public Room StartingRoom;
    public Room LastRoom;
    public Room SecretRoom;

    public int maxRooms = 12;
    private Room[,] spawnedRooms;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spawnedRooms = new Room[11, 11];
        spawnedRooms[5, 5] = StartingRoom;

        for (int i = 0; i < maxRooms; i++)
        {            
            PlaceOneRoom(i);
        }
    }

    private void PlaceOneRoom(int i)
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Room newRoom;
        if (i == maxRooms - 8)
        {
            newRoom = Instantiate(SecretRoom);
        }
        else if (i != maxRooms - 1)
        {
            // Эту строчку можно заменить на выбор комнаты с учётом её вероятности, вроде как в ChunksPlacer.GetRandomChunk()
            newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);
        }
        else
            newRoom = Instantiate(LastRoom);


        int limit = 500;
        while (limit-- > 0)
        {
            // Эту строчку можно заменить на выбор положения комнаты с учётом того насколько он далеко/близко от центра,
            // или сколько у него соседей, чтобы генерировать более плотные, или наоборот, растянутые данжи
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));            

            if (ConnectToRoom(newRoom, position))
            {                
                newRoom.transform.position = new Vector3((position.x - 5) * 18, (position.y - 5) * 10, 0);
                spawnedRooms[position.x, position.y] = newRoom;
                return;
            }
            else
            {
                newRoom.RotateRandomly();
            }
        }

        Destroy(newRoom.gameObject);
    }
    
    private bool ConnectToRoom(Room room, Vector2Int pos)
    {       
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.DoorUp != null && pos.y < maxY && spawnedRooms[pos.x, pos.y + 1]?.DoorDown != null) neighbours.Add(Vector2Int.up);
        if (room.DoorDown != null && pos.y > 0 && spawnedRooms[pos.x, pos.y - 1]?.DoorUp != null) neighbours.Add(Vector2Int.down);
        if (room.DoorRight != null && pos.x < maxX && spawnedRooms[pos.x + 1, pos.y]?.DoorLeft != null) neighbours.Add(Vector2Int.right);
        if (room.DoorLeft != null && pos.x > 0 && spawnedRooms[pos.x - 1, pos.y]?.DoorRight != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[pos.x + selectedDirection.x, pos.y + selectedDirection.y];
        
        if (room.isLastRoom && selectedRoom.isSecretRoom) return false;

        if (selectedDirection == Vector2Int.up)
        {
            room.DoorUp.SetActive(false);
            selectedRoom.DoorDown.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.DoorDown.SetActive(false);
            selectedRoom.DoorUp.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.DoorRight.SetActive(false);
            selectedRoom.DoorLeft.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.DoorLeft.SetActive(false);
            selectedRoom.DoorRight.SetActive(false);
        }

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DungeonRoom : MonoBehaviour
{
    public Vector2 GridLocation;

    public bool isSpawnRoom = false;
    public bool isExitRoom = false;

    // VOLGORDE
    // LINKS > TOP > RECHTS > BOTTOM
    [Header("LINKS > TOP > RECHTS > BOTTOM")]
    public bool[] Connections = new bool[4];
    public DungeonRoom[] ConnectedRooms = new DungeonRoom[4];

    public Doorways[] doorways;
    public List<Enemy> AllEnemiesInRoom = new List<Enemy>();

    public GameObject MiniMapTile;
    private bool SpawnedEnemies = false;
    private void Start()
    {
        
    }

    public void SetRoom(Vector2 location, gridSpace[,] grid)
    {
        GridLocation = location;

        int x = (int)location.x;
        int y = (int)location.y;

        // checks left
        if(grid[x-1, y] != gridSpace.empty)
        {
          
            Connections[0] = true;
        }
        // checks top
        if (grid[x, y+1] != gridSpace.empty)
        {
            
            Connections[1] = true;
        }
        // checks right
        if (grid[x + 1, y] != gridSpace.empty)
        {
         
            Connections[2] = true;

        }
        // checks bottom
        if (grid[x, y-1] != gridSpace.empty)
        {
           
            Connections[3] = true;

        }

        SetDoors();

       
    }

    private void SpawnEnemies()
    {
        if (SpawnedEnemies)
            return;
        DungeonManager.dungeonManager.CreateMesh();

        int rand = Random.Range(1, 4);
        for (int i = 0; i < rand; i++)
        {
            DungeonManager.dungeonManager.SpawnEnemy(transform, RandomPositionNoCheck(AstarPath.active.data.gridGraph.nodes, 0.1f));
        }

        SpawnedEnemies = true;


    }

    public Vector3 RandomPositionNoCheck(GraphNode[]nodes, float HeightOffset)
    {
        for (int i = 0; i < 1000; i++)
        {
            
            int rand = Random.Range(0, nodes.Length);
            if (nodes[rand].Walkable)
            {
                Vector3 pos = (Vector3)nodes[rand].position;
                pos.y += 1;

                return pos;
            }
   
        }
        return Vector3.zero;
    }

    public void SetConnectingRooms()
    {
        int x = (int)GridLocation.x;
        int y = (int)GridLocation.y;


        // checks if we have a connection on the grid if this is the case grab that room
        if (Connections[0])
        {
           
            ConnectedRooms[0] = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x - 1, y));
        }
        if (Connections[1])
        {
            ConnectedRooms[1] = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x, y + 1));
        }
        if (Connections[2])
        {
            ConnectedRooms[2] = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x + 1, y));
        }
        if (Connections[3])
        {
            ConnectedRooms[3] = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x, y - 1));
        }
    }

    public void SetSpawnRoom()
    {
        isSpawnRoom = true;

        gameObject.SetActive(true);

        int randIndex = Random.Range(0, doorways.Length);
        doorways[randIndex].SetPlayerPos();


        StartCoroutine(SetMiniMapOnSpawn());

        IEnumerator SetMiniMapOnSpawn()
        {
            yield return new WaitForSeconds(0.1f);

            MoveMiniMap(this);
        }

        // grab random location in that room and spawn player on it
    }

    public void SetExitRoom()
    {
        isExitRoom = true;
    }

    // sets the corresponding doors on active and deactive
    private void SetDoors()
    {
        // grabs all the door components in the room
        doorways = GetComponentsInChildren<Doorways>();

        foreach (var door in doorways)
        {
            door.ConnectDoorToRoom(this);
        }

    }

    public void TravelToNextRoom(int index)
    {
        // need to invert this if a player enters a door on the right he should popout on the left in the new room
        gameObject.SetActive(false);
        DungeonRoom NewRoom = ConnectedRooms[index];
        MoveMiniMap(NewRoom);

        Doorways NewDoorway = null;
        switch (index)
        {
            case 0:
                NewDoorway = NewRoom.doorways[2];
            break;
            case 1:
                NewDoorway = NewRoom.doorways[3];
                break;
            case 2:
                NewDoorway = NewRoom.doorways[0];
                break;
            case 3:
                NewDoorway = NewRoom.doorways[1];
                break;

         
        }



        NewRoom.gameObject.SetActive(true);
        DungeonManager.dungeonManager.CreateMesh();
        NewDoorway.SetPlayerPos();

        NewRoom.SpawnEnemies();





    }

    public void MoveMiniMap(DungeonRoom room)
    {
        MiniMapCamera.mapCamera.MoveCamToTile(room.MiniMapTile.transform.position);
    }

    public void DestroyRoom()
    {

        Destroy(MiniMapTile);
        Destroy(gameObject);
    }

    public void SetMiniMapTile(GameObject tile)
    {
        MiniMapTile = tile;
    }

}


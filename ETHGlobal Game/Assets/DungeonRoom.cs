using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform PlayerSpawnPoint;
    public List<Enemy> AllEnemiesInRoom = new List<Enemy>();

    public GameObject MiniMapTile;

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

        PlayerController.playerController.transform.position = PlayerSpawnPoint.position;


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

        Doorways NewDoorway = null;
        switch (index)
        {
            case 0:
                NewDoorway = doorways[2];
            break;
            case 1:
                NewDoorway = doorways[3];
                break;
            case 2:
                NewDoorway = doorways[0];
                break;
            case 3:
                NewDoorway = doorways[1];
                break;

         
        }

        DungeonRoom NewRoom = ConnectedRooms[index];
        MoveMiniMap(NewRoom);

        NewRoom.gameObject.SetActive(true);

        NewDoorway.SetPlayerPos();



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


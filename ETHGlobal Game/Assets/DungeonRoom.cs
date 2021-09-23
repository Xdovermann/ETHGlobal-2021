using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public Vector2 GridLocation;

    public bool LeftConnected = false;
    public bool TopConnected = false;
    public bool RightConnected = false;
    public bool BottomConnected = false;

    public DungeonRoom LeftRoom;
    public DungeonRoom TopRoom;
    public DungeonRoom RightRoom;
    public DungeonRoom BottomRoom;

    public void SetRoom(Vector2 location, gridSpace[,] grid)
    {
        GridLocation = location;

        int x = (int)location.x;
        int y = (int)location.y;

        // checks left
        if(grid[x-1, y] != gridSpace.empty)
        {
            LeftConnected = true;
          
        }
        if (grid[x, y+1] != gridSpace.empty)
        {
            TopConnected = true;
       
        }
        if (grid[x + 1, y] != gridSpace.empty)
        {
            RightConnected = true;
          
        }
        if (grid[x, y-1] != gridSpace.empty)
        {
            BottomConnected = true;
          
        }

        SetDoors();
    }

    public void SetConnectingRooms()
    {
        int x = (int)GridLocation.x;
        int y = (int)GridLocation.y;


        // checks if we have a connection on the grid if this is the case grab that room
        if (LeftConnected)
        {
            LeftRoom = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x - 1, y));
        }
        if (TopConnected)
        {
            TopRoom = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x, y + 1));
        }
        if (RightConnected)
        {
            RightRoom = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x + 1, y));
        }
        if (BottomConnected)
        {
            BottomRoom = DungeonManager.dungeonManager.ReturnConnectingRoom(new Vector2(x, y - 1));
        }
    }

    // sets the corresponding doors on active and deactive
    private void SetDoors()
    {

    }
}

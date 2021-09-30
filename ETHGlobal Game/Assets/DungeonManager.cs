using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public enum gridSpace { empty, Room, SpawnRoom, ExitRoom, LootRoomEntrance, BossRoom };


public class DungeonManager : MonoBehaviour
{
    public static DungeonManager dungeonManager;

    public string DungeonSeed = "";
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    public gridSpace[,] grid;

    struct walker
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    List<walker> walkers;
    float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
    float chanceWalkerDestoy = 0.05f;

    public int maxWalkers = 5;
    public float percentToFill = 0.05f;


    public int CurrentRoomCount = 0;

    public int roomHeight, roomWidth;

    public GameObject TestSquare;

    public GameObject TestSquareRoom;
    public GameObject TestSquareSpawn;
    public GameObject TestSquareExit;

    public List<GameObject> MiniMapTiles = new List<GameObject>();

    public List<DungeonRoom> AllRooms = new List<DungeonRoom>();
    public GameObject[] DungeonRooms;
    public Transform DungeonHolder;

    public Transform MiniMapHolder;

    private void Awake()
    {
        dungeonManager = this;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGenerating();
        }
    }

    public void StartGenerating()
    {
        ClearMap();
        // generates seed
        GenerateDungeonSeed();
        // sets up the grid 
        SetUpGrid();
        
    
        // doe dit toch anders
        // begin in het midden van de map en run gwn een random walker over de map 





        // removes tiles at random

    }

    private void GenerateDungeonSeed()
    {
        string holder = "";
        int charAmount = Random.Range(5, 20); //set those to the minimum and maximum length of your string

        for (int y = 0; y < charAmount; y++)
        {
            holder += glyphs[Random.Range(0, glyphs.Length)];
        }

        DungeonSeed = holder;

    }

    private void SetUpGrid()
    {
        grid = new gridSpace[roomWidth, roomHeight];
             
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
               // GameObject go= Instantiate(TestSquare, new Vector3(x,y,0),transform.rotation,transform);
             //   SpawnedObjects.Add(go);
                grid[x, y] = gridSpace.empty;
            }
        }

        walkers = new List<walker>();
     
        walker newWalker = new walker();
        newWalker.dir = RandomDirection();
    
        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2.0f),
                                        Mathf.RoundToInt(roomHeight / 2.0f));
        newWalker.pos = spawnPos;
      
        walkers.Add(newWalker);

        SetUpBaseFloor();
    }


    private void SetUpBaseFloor()
    {
        int iterations = 0;//loop will not run forever
        do
        {
            //create floor at position of every walker
            foreach (walker myWalker in walkers)
            {
                //if(CurrentRoomCount < MaxRoomCount)
                //   {

                grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = gridSpace.Room;
                //  }

            }

            //chance: destroy walker
            int numberChecks = walkers.Count; //might modify count while in this loop
            for (int i = 0; i < numberChecks; i++)
            {
                //only if its not the only one, and at a low chance
                if (Random.value < chanceWalkerDestoy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break; //only destroy one per iteration
                }
            }

            //chance: walker pick new direction
            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
                {
                    walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }

            //chance: spawn new walker
            numberChecks = walkers.Count; //might modify count while in this loop
            for (int i = 0; i < numberChecks; i++)
            {
                //only if # of walkers < max, and at a low chance
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    //create a walker 
                    walker newWalker = new walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            }

            //move walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                thisWalker.pos += thisWalker.dir;
                walkers[i] = thisWalker;
            }

            //avoid boarder of grid
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                //clamp x,y to leave a 1 space boarder: leave room for walls
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
                walkers[i] = thisWalker;
            }

            //check to exit loop
            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }

            iterations++;
        } while (iterations < 100000);


            RemoveRoomChunks();
           
            SetSpawnRoom();
            SetExitRoom();

          

            // this spawns the actual dungeon rooms
            SpawnDungeonRooms();

            MiniMapHolder.transform.position = new Vector3(0, 100, 0);
        // need to flip mini map its inverted
             MiniMapHolder.transform.localScale = new Vector3(-1, 1, 1);

        CreateMesh();

    }

    private void RemoveRoomChunks()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if(grid[x,y] == gridSpace.Room)
                {
                    if(grid[x+1, y] == gridSpace.Room &&
                       grid[x - 1, y] == gridSpace.Room &&
                       grid[x , y+1] == gridSpace.Room &&
                       grid[x, y - 1] == gridSpace.Room &&
                         grid[x+1, y + 1] == gridSpace.Room &&
                          grid[x - 1, y + 1] == gridSpace.Room &&
                          grid[x - 1, y - 1] == gridSpace.Room &&
                           grid[x + 1, y - 1] == gridSpace.Room)
                    {
                        grid[x, y] = gridSpace.empty;
                        Debug.Log("REMOVED MAP CHUNK");
                    }
                }
            }
        }
    }

    private void CheckForNonConnectingTiles()
    {
        // doe dit anders maak een walker die begint op de spawn
        // en die probeert te loopen naar de exit spot 


        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == gridSpace.Room)
                {
                    if (grid[x + 1, y] == gridSpace.Room || grid[x - 1, y] == gridSpace.Room || grid[x, y + 1] == gridSpace.Room || grid[x, y - 1] == gridSpace.Room)
                    {
                       
                        // do nothing tile connects
                    }
                    else
                    {
                        Debug.Log("NON ACCESIBLE TILE ROLL FOR LOOT ROOM OR remove");
                        int rand = Random.Range(0, 3);
                        if (rand == 1)
                        {
                            // make it a loot room
                            // pick another random tile for the room where the teleporter / trapdoor will be 
                            // dont pick spawn rooms , exit rooms or the boss room or any other special rooms
                        }
                        else
                        {
                            grid[x, y] = gridSpace.empty;
                        }
                    }
                }
            }
        }
    }

    Vector2 RandomDirection()
    {
        //pick random int between 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        //use that int to chose a direction
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }

    int NumberOfFloors()
    {
        int count = 0;
        foreach (gridSpace space in grid)
        {
            if (space == gridSpace.Room)
            {
                count++;
            }
        }
        return count;
    }

    private void ClearMap()
    {
        // reset mini map
        MiniMapHolder.transform.localScale = new Vector3(1, 1, 1);
        foreach (var room in AllRooms)
        {
            room.DestroyRoom();
        }

        AllRooms.Clear();
        MiniMapTiles.Clear();
        CurrentRoomCount = 0;


    }

    private void SetSpawnRoom()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == gridSpace.Room)
                {
                    grid[x, y] = gridSpace.SpawnRoom;
                  
                    return;
                }
            }
        }
    }

    private void SetExitRoom()
    {
        for (int x = roomWidth; x-- > 0;)
        {
            for (int y = roomHeight; y-- > 0;)
            {
                if (grid[x, y] == gridSpace.Room)
                {
                    grid[x, y] = gridSpace.ExitRoom;
           
                    return;
                }
            }
        }
    }

    private GameObject SpawnMiniMapTile(int x, int y, GameObject tile)
    {
        GameObject go = Instantiate(tile, new Vector3(x, y, 0), transform.rotation, MiniMapHolder);
        MiniMapTiles.Add(go);

        return go;
      
    }

    private void SpawnDungeonRooms()
    {
        // spawn a room for each non empty grid
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] != gridSpace.empty)
                {
                    CurrentRoomCount++;
                    int index = Random.Range(0, DungeonRooms.Length);
                    GameObject room = DungeonRooms[index];

                    GameObject go = Instantiate(room, DungeonHolder);
                    go.transform.position = new Vector3(0, 0, 0);
                    go.SetActive(false);

                    // set the location of the tile
                    DungeonRoom currentRoom = go.GetComponent<DungeonRoom>();
                    currentRoom.SetRoom(new Vector2(x, y), grid);

                    // add to list
                    AllRooms.Add(currentRoom);

                    if (grid[x, y] == gridSpace.SpawnRoom)
                    {
                        GameObject tile = SpawnMiniMapTile(x, y, TestSquareSpawn);
                        currentRoom.SetMiniMapTile(tile);

                        currentRoom.SetSpawnRoom();
                    }
                    else if (grid[x, y] == gridSpace.ExitRoom)
                    {
                        GameObject tile = SpawnMiniMapTile(x, y, TestSquareExit);
                        currentRoom.SetMiniMapTile(tile);

                        SpawnMiniMapTile(x, y, TestSquareExit);
                        currentRoom.SetExitRoom();
                    }
                    else if (grid[x, y] == gridSpace.Room)
                    {
                        GameObject tile = SpawnMiniMapTile(x, y, TestSquareRoom);
                        currentRoom.SetMiniMapTile(tile);

                     
                    }


                }
            }
        }
        // after adding everything to the list you loop over all the rooms and set teh connecting rooms
        foreach (var room in AllRooms)
        {
            room.SetConnectingRooms();
        }

    }

    // returns the connecting room based on grid location
    public DungeonRoom ReturnConnectingRoom(Vector2 locationOfRoom)
    {
        // we set all the grid locations on the rooms when spawning them so we can use that grid location to check if 
        // a room is neighbouring
        foreach (var rooms in AllRooms)
        {
            if(rooms.GridLocation == locationOfRoom)
            {
                return rooms;
            }
        }

        Debug.Log("Room doesnt exist");
        return null;
    }

    public void CreateMesh()
    {
        AstarPath.active.Scan();
    }

}



[System.Serializable]
public class ColorToTile
{
    public Color color;
    public GameObject Rooms;
}
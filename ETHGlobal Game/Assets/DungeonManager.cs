using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public string DungeonSeed = "";
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    public ColorToTile[] ColorMappings;

    public Sprite[] SourceImages;

    enum gridSpace { empty, Room, RemoveNode, BossRoom };
    gridSpace[,] grid;

   public int roomHeight, roomWidth;

    public GameObject TestSquare;

    public GameObject TestSquareRoom;


    public GameObject RemoveNodeSquare;

    public List<GameObject> SpawnedObjects = new List<GameObject>();
    public Texture2D texture;  
    public int maxNodeAmount =6;
    private int CurrentNodeAmount=0;
    // Start is called before the first frame update
    void Start()
    {
        
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
        DestroyAllSpawnedObjects();
        // generates seed
        GenerateDungeonSeed();
        // sets up the grid 
        SetUpGrid();

        // doe dit toch anders
        // begin in het midden van de map en run gwn een random walker over de map 



        // picks a random source image and places it on the grid
        GrabSourceImage();

     
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
                GameObject go= Instantiate(TestSquare, new Vector3(x,y,0),transform.rotation,transform);
                SpawnedObjects.Add(go);
                grid[x, y] = gridSpace.empty;
            }
        }
    }

    private void GrabSourceImage()
    {
        int index = Random.Range(0, SourceImages.Length);
        Sprite image = SourceImages[index];
        texture = image.texture;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                GenerateTileColor(x, y);
            }
        }
    }


    private void RemoveNodes()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if(grid[x,y] == gridSpace.Room)
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 1)
                    {
                        bool allChecks = false;
                        bool skipRestCheck = false;
                        for (int i = 0; i < 3; i++)
                        {

                            if(CheckForNodeSpace(x + i, y) && CheckForNodeSpace(x - i, y)&& CheckForNodeSpace(x, y + i) && CheckForNodeSpace(x, y - i)
                            && CheckForNodeSpace(x+i, y + i) && CheckForNodeSpace(x + i, y - i) && CheckForNodeSpace(x - i, y + i) &&  CheckForNodeSpace(x - i, y - i))
                            {

                            allChecks = true;
                            // place a new node
                             
                            }
                        else 
                        {
                            skipRestCheck = true;
                            allChecks = false;
                        }

                           
                    }

                    if (allChecks == true && !skipRestCheck)
                    {
                        CurrentNodeAmount++;
                        grid[x, y] = gridSpace.RemoveNode;
                        SpawnTestTile(x, y, RemoveNodeSquare);
                    }

                     }
                }
            }
        }
    }

    private void RemoveNodesUpdated(int startX, int startY)
    {
        bool canPlace = true;

        if (grid[startX, startY] == gridSpace.Room)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (grid[startX, startY] == gridSpace.RemoveNode)
                    {
                        canPlace = false;
                    }
                      
                  
                }
            }
        }

        if (canPlace)
        {
            SpawnTestTile(startX, startY, RemoveNodeSquare);
        }
     

    }

    private bool CheckForNodeSpace(int x, int y)
    {
        if(grid[x,y] == gridSpace.RemoveNode)
        {
            Debug.Log("test");
            return false;
        }
        else
        {
          
            return true;
        }
    }

    // checks teh color and places the corresponding tile on the map layout
    private void GenerateTileColor(int x, int y)
    {
       Color pixelColor = texture.GetPixel(x, y);

        // transparent pixel on texture no need to check any further
        if (pixelColor.a == 0)
            return;


        foreach (ColorToTile colormapping in ColorMappings)
        {
            if (colormapping.color.Equals(pixelColor)){

                SpawnTestTile(x, y, colormapping.Rooms);
                grid[x, y] = gridSpace.Room;
            }
        }

    }

    private void DestroyAllSpawnedObjects()
    {
        foreach (var item in SpawnedObjects)
        {
            Destroy(item.gameObject);
        }

        SpawnedObjects.Clear();
    }


    private void SpawnTestTile(int x, int y, GameObject tile)
    {
        GameObject go = Instantiate(tile, new Vector3(x, y, 0), transform.rotation, transform);
        SpawnedObjects.Add(go);
      
    }

}



[System.Serializable]
public class ColorToTile
{
    public Color color;
    public GameObject Rooms;
}
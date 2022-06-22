using System.Collections;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public enum TileType
    {
        Wall, Floor, Chest, Chair
    }
    public int columns = 100;                                                                   //Number of columns on the board
    public int rows = 100;                                                                      //The number of rows on the board
    public IntRange numRooms = new IntRange (4, 4);                                             //the range of the number of rooms
    public IntRange roomWidth = new IntRange (4, 4);                                            //the range of widths of the rooms
    public IntRange roomHeight = new IntRange (4, 4);                                           //the range of height of the rooms
    public IntRange corridorLength = new IntRange (4, 4);                                       //The range of lengths of the rooms
    public GameObject[] floorTiles;                                                             //array of floor tiles
    public GameObject[] wallTiles;                                                              //array of wall tiles
    public GameObject[] outerWallTiles;                                                         //array of outerwall tiles
    public GameObject chest;
    public GameObject player;
    public GameObject chair;

    private TileType[][] tiles;                                                                 //jagged array of tile types
    private Room[] rooms;                                                                       //all the rooms that are created
    private Corridor[] corridors;                                                               //all the corridors of the rooms
    private GameObject boardHolder;                                                             //container of all tiles



    private void Start()
    {
        boardHolder = new GameObject("BoardHolder");

        SetupTilesArray ();

        CreateRoomsAndCorridors ();

        SetTilesValuesForRooms ();

        SetTilesValuesForCorridors ();

        SpawnFurniture();

        InstantiateTiles ();

        InstantiateOuterWalls ();
    }

    void SetupTilesArray ()
    {
        //set the tiles array to width
        tiles = new TileType[columns][];
        
        //go through tile arrays 
        for (int i = 0; i < tiles.Length; i++)
        {
            //and set up tile rows
            tiles[i] = new TileType[rows];
        }
    }

    void CreateRoomsAndCorridors ()
    {
        //create array of random amount of rooms
        rooms = new Room[numRooms.Random];

        //one less corridor than there are rooms
        corridors = new Corridor[rooms.Length - 1];

        //first corridor and room
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        //setup first room with no first corridor
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room();

            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i -1]);

            if (i < corridors.Length)
            {
                corridors[i] = new Corridor();

                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }

    }

    void SpawnFurniture ()
    {
        for (int i = 0; i < rooms.Length; i++)
        {

            //goes through array of rooms and randomizes chance to spawn a chest
            Room currentRoom = rooms[i];
            int chestChance = Random.Range(0, 100);

            //goes through array of rooms and randomizes chance to spawn a Chair
            int chairChance = Random.Range(0, 100);

            //spawn the chests
            if (chestChance > 75)
            {
                //transfers variables from the "setuproom" method in the "Room" page
                rooms[i] = new Room();
                rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

                //sets local method variables to external variables
                int chestMinXPos = currentRoom.xPos + 1;
                int chestMinYPos = currentRoom.yPos + 1;
                int chestMaxXPos = currentRoom.xPos + currentRoom.roomWidth - 1;
                int chestMaxYPos = currentRoom.yPos + currentRoom.roomHeight - 1;

                //picks a random x,y inside room "i"
                int chestXPos = Random.Range(chestMinXPos, chestMaxXPos);
                int chestYPos = Random.Range(chestMinYPos, chestMaxYPos);

                //puts a chest in the room
                Vector3 ChestSpawn = new Vector3 (chestXPos, chestYPos, 0);
                Instantiate (chest, ChestSpawn, Quaternion.identity);
            }

            //spawn the chairs
            if (chairChance > 75)
            {
                //sets local method variables to external variables
                rooms[i] = new Room();
                rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

                int chairMinXPos = currentRoom.xPos + 1;
                int chairMinYPos = currentRoom.yPos + 1; 
                int chairMaxXPos = currentRoom.xPos + currentRoom.roomWidth - 1;
                int chairMaxYPos = currentRoom.yPos + currentRoom.roomHeight - 1;

                //picks a random x,y inside room "i"
                int chairXPos = Random.Range(chairMinXPos, chairMaxXPos);
                int chairYPos = Random.Range(chairMinYPos, chairMaxYPos);

                //puts a chest in the room
                Vector3 ChairSpawn = new Vector3(chairXPos, chairYPos, 0);
                Instantiate (chair, ChairSpawn, Quaternion.identity);
            }
        }
    }

    void SetTilesValuesForRooms()
    {
        int xCoord;
        int yCoord;

        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                xCoord = currentRoom.xPos + j;

                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    yCoord = currentRoom.yPos + k;

                    tiles[xCoord][yCoord] = TileType.Floor;                                         //go through all rooms and set all tiles to floor tiles

                }
            }
        }
    }

    void SetTilesValuesForCorridors ()
    {
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                tiles[xCoord][yCoord] = TileType.Floor;

            }
        }
    }

    void InstantiateTiles ()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                    if (tiles[i][j] == TileType.Floor)
                {
                    InstantiateFromArray(floorTiles, i, j);
                }
                if (tiles[i][j] == TileType.Wall)
                {
                    InstantiateFromArray(wallTiles, i, j);
                }
            }
        }
    }

    void InstantiateOuterWalls ()
    {
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float TopEdgeY = rows + 0f;

        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, TopEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, TopEdgeY);

        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, TopEdgeY);
    }

    void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
    {
        float currentY = startingY;

        while (currentY <= endingY)
        {
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }

    void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
    {
        float currentX = startingX;

        while (currentX <= endingX)
        {
            InstantiateFromArray(outerWallTiles, currentX, yCoord);


            currentX++;
        }
    }
 
    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        //picks up random tile from tile prefab list
        int randomIndex = Random.Range(0, prefabs.Length);

        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        //doesn't rotate the tiles
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        //puts all tiles into a folder for easy access/limits clutter (i think)
        tileInstance.transform.parent = boardHolder.transform;
    }
}
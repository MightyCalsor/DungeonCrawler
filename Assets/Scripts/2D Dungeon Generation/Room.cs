using UnityEngine;

public class Room
{
    public int xPos;
    public int yPos;
    public int roomWidth;
    public int roomHeight;
    public Direction enteringCorridor;


    //for first room
    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f);
    }

    //overload after first room
    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows, Corridor corridor)
    {

        enteringCorridor = corridor.direction;

        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        switch (corridor.direction)
        {

            //if the corridor entering the room is going north
            case Direction.North:
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);
                yPos = corridor.EndPositionY;
                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            //if the corridor entering the room is going east
            case Direction.East:
                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);
                xPos = corridor.EndPositionX;
                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;

            //if the corridor entering the room is going south
            case Direction.South:
                roomHeight = Mathf.Clamp(roomHeight, 1, corridor.EndPositionY);
                yPos = corridor.EndPositionY - roomHeight + 1;
                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);
                break;

            //if the corridor entering the room is going west
            case Direction.West:
                roomWidth = Mathf.Clamp(roomWidth, 1, corridor.EndPositionX);
                xPos = corridor.EndPositionX - roomWidth + 1;
                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);
                break;
        }
    }
}

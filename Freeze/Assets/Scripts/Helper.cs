using System;
using System.Collections;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static Vector2Int[] directions = 
    {
        Vector2Int.up,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.right
    };

    Vector2Int pos, dir;
    int steps, min, max;

    public void HelperInit(Vector2Int position, Vector2Int direction, int numSteps, int minimum, int maximum)
    {
        pos = position;
        dir = direction;
        min = minimum;
        max = maximum;
    }

    public void Walk()
    {
        if (MazeGenerator.generator.map[pos.x, pos.y].gameObject == null) { Destroy(gameObject); }
        if (steps == 0) Turn();
        else if (pos.x + (2 * dir.x) < 2 ||
            pos.x + (2 * dir.x) > MazeGenerator.generator.mazeSize - 3 ||
            pos.y + (2 * dir.y) < 2 ||
            pos.y + (2 * dir.y) > MazeGenerator.generator.mazeSize - 3) Turn(); 
    
        pos += dir;
        Destroy(MazeGenerator.generator.map[pos.x, pos.y].gameObject);
  
        pos += dir;
        Destroy(MazeGenerator.generator.map[pos.x, pos.y].gameObject);

        steps -= 2;
    }

    void Turn()
    {
        int currentDirIndex = Array.IndexOf(directions,dir);
        int[] possibleDirs = { currentDirIndex - 1, currentDirIndex + 1 };
        if (possibleDirs[0] == -1) possibleDirs[0] = 3;
        if (possibleDirs[1]== 4) possibleDirs[1] = 0;
        int dirChoice = UnityEngine.Random.Range(0, 2);
        Debug.Log("Turn");
        switch (dirChoice)
        {
            case 0:
                dir = directions[possibleDirs[0]];
                Debug.Log("Position: " + pos.x + "," + pos.y + " Current Direction: " + dir.x + "," + dir.y);
                    break;
            case 1:
                dir = directions[possibleDirs[1]];
                Debug.Log("Position: " + pos.x + "," + pos.y + " Current Direction: " + dir.x + "," + dir.y);
                break;
        }
        
        Debug.Log("Position: " + pos.x + "," + pos.y + " Current Direction: " + dir.x + "," + dir.y);
        steps = 2*UnityEngine.Random.Range(min/2,max/2);
    }
}

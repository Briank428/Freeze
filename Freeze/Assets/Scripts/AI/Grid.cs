using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private static float unit = 1.5f;
    private static Node[,] grid;

    //Equivalent of Start
    public static void BuildGrid(Node[,] nodes, int size)
    {
        grid = nodes;

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                //bottomLeft corner
                if (grid[i,j].isActive() && i==0 && j==0) {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j+1]); grid[i, j].up = true; }
                }
                //bottomRight corner
                else if (grid[i, j].isActive() && i == size-1 && j == 0)
                {
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                //topLeft corner
                else if (grid[i, j].isActive() && i == 0 && j == size-1)
                {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                }
                //topRight corner
                else if (grid[i, j].isActive() && i == size-1 && j == size-1)
                {
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                }
                //Left side
                else if (grid[i,j].isActive() && i==0)
                {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                //Right side
                else if (grid[i, j].isActive() && i == size-1)
                {
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                //Top Row
                else if (grid[i, j].isActive() && j == size-1)
                {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                }
                //Bottom Row
                else if (grid[i, j].isActive() && j == 0)
                {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                else if (grid[i, j].isActive())
                {
                        if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                        if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                        if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                        if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                }
            }
        }
        
        foreach (Node node in grid) {
            if (node.isActive())
            {
                GameObject tempNode = Instantiate(Resources.Load("nodeImage")) as GameObject;
                tempNode.transform.position = new Vector3((float)(node.GetX()), (float)(node.GetY()), -1f);
                if (node.up) node.Up.DrawLine();
                if (node.down) node.Down.DrawLine();
                if (node.left) node.Left.DrawLine();
                if (node.right) node.Right.DrawLine();
            }
            
        }
    }
    
    public static void UpdateGrid(GameObject player, bool frozen) {

    }
}

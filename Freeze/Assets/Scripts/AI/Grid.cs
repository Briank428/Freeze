using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private static float unit = 1.5f;
    private static Node[,] grid;
    private static int width;
    public int GetWidth() { return width; }

    //Equivalent of Start
    public static void BuildGrid(Node[,] nodes, int size)
    {
        grid = nodes;
        width = size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                //bottomLeft corner
                if (grid[i, j].isActive() && i == 0 && j == 0)
                {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                //bottomRight corner
                else if (grid[i, j].isActive() && i == size - 1 && j == 0)
                {
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                //topLeft corner
                else if (grid[i, j].isActive() && i == 0 && j == size - 1)
                {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                }
                //topRight corner
                else if (grid[i, j].isActive() && i == size - 1 && j == size - 1)
                {
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                }
                //Left side
                else if (grid[i, j].isActive() && i == 0)
                {
                    if (grid[i + 1, j].isActive()) { grid[i, j].Right = new NodeConnections(grid[i, j], grid[i + 1, j]); grid[i, j].right = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                //Right side
                else if (grid[i, j].isActive() && i == size - 1)
                {
                    if (grid[i - 1, j].isActive()) { grid[i, j].Left = new NodeConnections(grid[i, j], grid[i - 1, j]); grid[i, j].left = true; }
                    if (grid[i, j - 1].isActive()) { grid[i, j].Down = new NodeConnections(grid[i, j], grid[i, j - 1]); grid[i, j].down = true; }
                    if (grid[i, j + 1].isActive()) { grid[i, j].Up = new NodeConnections(grid[i, j], grid[i, j + 1]); grid[i, j].up = true; }
                }
                //Top Row
                else if (grid[i, j].isActive() && j == size - 1)
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

        foreach (Node node in grid)
        {
            if (node.isActive())
            {
                GameObject tempNode = GameObject.Instantiate(Resources.Load("nodeImage")) as GameObject;
                tempNode.transform.position = new Vector3((float)(node.GetX()), (float)(node.GetY()), -1f);
                if (node.up) node.Up.DrawLine();
                if (node.down) node.Down.DrawLine();
                if (node.left) node.Left.DrawLine();
                if (node.right) node.Right.DrawLine();
            }

        }
    }
    public bool ConnectionIsValid(Point a, Point b) {
        if (a.X == b.X && a.Y == b.Y) return false; if (!grid[a.X, a.Y].isActive()) return false;
        if (a.X == b.X && a.Y > b.Y) return grid[a.X, a.Y].up;
        if (a.X == b.X && a.Y < b.Y) return grid[a.X, a.Y].down;
        if (a.X > b.X && a.Y == b.Y) return grid[a.X, a.Y].left;
        if (a.X < b.X && a.Y== b.Y) return grid[a.X, a.Y].right;
        return false;
    }
    public static void UpdateGrid(GameObject player, bool frozen) {
        Vector2 temp = player.transform.position;
        bool twoNodes = false;
        Vector2 temp1 = new Vector2(-1f, -1f);
        if (temp.x % 1 == 0) { twoNodes = true;  temp1 = player.transform.position; temp.x -= 0.5f; temp1.x += 0.5f; }
        else temp.x = Round(temp.x);
        if (temp.y % 1 == 0) { twoNodes = true; temp1 = temp; temp.y -= 0.5f; temp1.y += 0.5f; }
        else if (twoNodes) { temp.y = Round(temp.y); temp1.y = Round(temp1.y); }
        else temp.y = Round(temp.y);
        if (twoNodes) { grid[(int)temp.x, (int)temp.y].SetActive(frozen); grid[(int)temp1.x, (int)temp1.y].SetActive(frozen); }
        else grid[(int)temp.x, (int)temp.y].SetActive(frozen);
    }
    public static float Round(float x)
    {
        if (x % 1 > 0.5) return x - (float)(x % 0.5);
        if (x % 1 < 0.5) return x + (0.5f - (float)(x % 0.5));
        return x;
    }
    void Update()
    {
        }
    }

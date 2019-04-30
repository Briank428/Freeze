using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour
{
    private static float unit = 1.5f;
    private static Node[,] grid;
    private static List<BreadCrumb> waypoints = new List<BreadCrumb>();
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
        int count = 0;
        foreach (Node node in grid)
        {
            if (node.isActive()) node.SetWayPoint();
            if (node.isWayPoint()) {
                BreadCrumb bc = new BreadCrumb(new Vector2((float)node.GetX(), (float)node.GetY()));
                if (node.up) {
                    int x = (int)(node.GetX() - 0.5);  int y = (int)(node.GetY() + 1.5);  Node temp = grid[x, y];
                    while (y < width-1 && temp.up) { y++; temp = grid[x, y]; }
                    bc.neighbors.Add(new BreadCrumb(new Vector2((float)(x + 0.5), (float)(y - 0.5))));
                    bc.upNeigh = true;
                }
                if (node.down)
                {
                    int x = (int)(node.GetX() - 0.5); int y = (int)(node.GetY() - 0.5); Node temp = grid[x, y];
                    while (y>1 && temp.down) { y--; temp = grid[x, y]; }
                    bc.neighbors.Add(new BreadCrumb(new Vector2((float)(x + 0.5), (float)(y - 0.5))));
                    bc.downNeigh = true;
                }
                if (node.right)
                {
                    int x = (int)(node.GetX() + 0.5); int y = (int)(node.GetY() + 0.5); Node temp = grid[x, y];
                    while (x < width-1 && temp.right) { x++; temp = grid[x, y]; }
                    bc.neighbors.Add(new BreadCrumb(new Vector2((float)(x + 0.5), (float)(y - 0.5))));
                    bc.rightNeigh = true;
                }
                if (node.left)
                {
                    int x = (int)(node.GetX() - 1.5); int y = (int)(node.GetY() + 0.5); Node temp = grid[x, y];
                    while (x> 1 && temp.left) { x--; temp = grid[x, y]; }
                    bc.neighbors.Add(new BreadCrumb(new Vector2((float)(x + 0.5), (float)(y - 0.5))));
                    bc.leftNeigh = true;
                }
                Debug.Log(count + " wp: (" + node.GetX() + ", " + node.GetY() + "). UP " + bc.upNeigh +
                    " DOWN " + bc.downNeigh + " RIGHT " + bc.rightNeigh + " LEFT " + bc.rightNeigh);
                waypoints.Add(bc);
                count++;
            }
            
        }
        
        /*foreach (Node node in grid)
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

        }*/
    }
    public bool ConnectionIsValid(Vector2 a, Vector2 b) {
        if (a.x == b.x && a.y == b.y) return false; if (!grid[(int)a.x, (int)a.y].isActive()) return false;
        if (a.x == b.x && a.y > b.y) return grid[(int)a.x, (int)a.y].up;
        if (a.x == b.x && a.y < b.y) return grid[(int)a.x, (int)a.y].down;
        if (a.x > b.x && a.y == b.y) return grid[(int)a.x, (int)a.y].left;
        if (a.x < b.x && a.y== b.y) return grid[(int)a.x, (int)a.y].right;
        return false;
    }
    public static void UpdateGrid(GameObject player, bool frozen) {
        Vector2[] array = WorldToGrid(player.transform.position);
        if (array.Length == 2) {
            Vector2 temp = array[0]; Vector2 temp1 = array[1];
            grid[(int)temp.x, (int)temp.y].SetActive(frozen);
            grid[(int)temp1.x, (int)temp1.y].SetActive(frozen); }
        else { Vector2 temp = array[0];  grid[(int)temp.x, (int)temp.y].SetActive(frozen); }
    }
    public static float Round(float x)
    {
        if (x % 1 > 0.5) return x - (float)(x % 0.5);
        if (x % 1 < 0.5) return x + (0.5f - (float)(x % 0.5));
        return x;
    }
    public static Vector2[] WorldToGrid(Vector3 pos)
    {
        Vector2 temp = new Vector2(pos.x, pos.y);
        bool twoNodes = false;
        Vector2 temp1 = new Vector2(-1f, -1f);
        if (temp.x % 1 == 0) { twoNodes = true; temp1 = new Vector2(pos.x, pos.y); temp.x -= 0.5f; temp1.x += 0.5f; }
        else temp.x = Round(temp.x);
        if (temp.y % 1 == 0) { twoNodes = true; temp1 = temp; temp.y -= 0.5f; temp1.y += 0.5f; }
        else if (twoNodes) { temp.y = Round(temp.y); temp1.y = Round(temp1.y); }
        else temp.y = Round(temp.y);

        if (twoNodes) { Vector2[] array1 = new Vector2[2]; array1[0] = temp; array1[1] = temp1; return array1; }
        Vector2[] array = new Vector2[1]; array[0] = temp; return array;
    }
    void Update()
    {
        /*Pathfinding demo
        if (Input.GetMouseButtonDown(0))
        {   
            //Convert mouse click point to grid coordinates
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);Debug.Log("Pressed Mouse at: (" + worldPos.x + ", " + worldPos.y + ")");
            Vector2[] gridArray = WorldToGrid(worldPos);
            Debug.Log("Closest Node: (" + gridArray[0].x + ", " + gridArray[0].y + ")");
            if (gridArray.Length != 0)
            {
                Debug.Log("gridarray");
                if (gridArray[0].x > 0 && gridArray[0].y > 0 && gridArray[0].x < width && gridArray[0].y < width)
                {

                    //Find path from player to clicked position
                    BreadCrumb bc = PathFinder.FindPath(this, GameObject.Find("AI"), gridArray[0]);
                    if (gridArray.Length == 2 && gridArray[1].x > 0 && gridArray[1].y > 0 && gridArray[1].x < width && gridArray[1].y < width)
                    {
                        BreadCrumb bc1 = PathFinder.FindPath(this, GameObject.Find("AI"), gridArray[1]);
                        if (bc1.CompareTo(bc) < 0) bc = bc1;
                    }
                    Debug.Log("BreadCrumb 0 at: (" + bc.position.x + ", " + bc.position.y + ")");
                    int count = 0;
                    LineRenderer lr = GameObject.Find("AI").GetComponent<LineRenderer>();
                    lr.SetVertexCount(100);  //Need a higher number than 2, or crashes out

                    //Draw out our path
                    while (bc != null)
                    {
                        lr.SetPosition(count, bc.position);
                        Debug.Log("BreadCrumb " + count +" at: (" + bc.position.x + ", " + bc.position.y + ")");
                        bc = bc.next;
                        count += 1;
                    }
                    lr.SetVertexCount(count);
                }
            }
        }*/
    }
}

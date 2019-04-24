using UnityEngine;

public class NodeConnections
{
    public Node start, end;

    public NodeConnections(Node a, Node b)
    {
        start = a; end = b;
    }
    public void DrawLine()
    {
            GameObject line = GameObject.Instantiate(Resources.Load("Line")) as GameObject;
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, new Vector3((float)start.GetX(), (float)start.GetY(), -1f));
            lineRenderer.SetPosition(1, new Vector3((float)end.GetX(), (float)end.GetY(), -1f));
    }
}

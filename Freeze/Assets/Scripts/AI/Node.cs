
public class Node
{
    private double x; public double GetX() { return x; }
    private double y; public double GetY() { return y;  }
    private bool active; public bool isActive () { return active; }
    public void SetActive(bool boolean) { active = boolean; }

    public bool up;
    public bool down;
    public bool left;
    public bool right;

    public NodeConnections Up;
    public NodeConnections Down;
    public NodeConnections Left;
    public NodeConnections Right;

    public Node(int xcoord, int ycoord, bool a) {
        x = xcoord + .5;
        y = ycoord - .5;
        active = a;
        up = false; down = false; left = false; right = false;
    }
}

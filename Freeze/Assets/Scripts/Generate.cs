using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using UnityEngine.AI;

public enum CELL_TYPE
{
    WALL = 1 << 0,
    PATH = 1 << 1,
    BEAN = 1 << 2,
    KERNEL = 1 << 3,

    CLOSED = WALL,
    OPEN = PATH | BEAN | KERNEL,
}

public enum DIR
{
    UP = 1 << 0, /* 0001 */
    RIGHT = 1 << 1, /* 0010 */
    DOWN = 1 << 2, /* 0100 */
    LEFT = 1 << 3, /* 1000 */

    VERTICAL = UP | DOWN,
    HORIZONTAL = LEFT | RIGHT,
    ALL = UP | RIGHT | DOWN | LEFT,
}

public class Generate : MonoBehaviour
{
    public static DIR[] DIR_ARRAY =
    {
        DIR.UP,
        DIR.RIGHT,
        DIR.DOWN,
        DIR.LEFT,
    };
    int size;
    public CELL_TYPE[,] cells;
    public List<Vector2Int> spawners;
    public List<Builder> builders;
    private List<Vector2Int> _startPoints = new List<Vector2Int>();
    public static Generate generator;
    public GameObject[] tiles;
    public static Node[,] nodes;

    void Start()
    {
        generator = this;
        GenerateMap();
    }

    public void GenerateMap()
    {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            size = 21;
            nodes = new Node[size, size];
            InitMap(4, 2, 10);
        }
        GameManager.Instance.spawnCells = spawners;
    }
    void InitMap(int spawners, int minDistance, int maxDistance)
    {
        this.spawners = new List<Vector2Int>();
        builders = new List<Builder>() ;
        InitStartPoints();
        InitCells();
        for (int i = 0; i < spawners; i++)
        {
            CreateSpawner(minDistance, maxDistance);
        }
        while (!Finished())
        {
            Next();
        }
        BuildMap();
    }

    void BuildMap()
    {
        PhotonNetwork.Instantiate("Block", new Vector2(-1, -1), Quaternion.identity);
        PhotonNetwork.Instantiate("Block", new Vector2(size, -1), Quaternion.identity);
        PhotonNetwork.Instantiate("Block", new Vector2(-1, size), Quaternion.identity);
        PhotonNetwork.Instantiate("Block", new Vector2(size, size), Quaternion.identity);

        for (int i = 0; i < size; i++)
        {
            PhotonNetwork.Instantiate("Block", new Vector2(-1, i), Quaternion.identity);
            PhotonNetwork.Instantiate("Block", new Vector2(size, i), Quaternion.identity);
            PhotonNetwork.Instantiate("Block", new Vector2(i, -1), Quaternion.identity);
            PhotonNetwork.Instantiate("Block", new Vector2(i, size), Quaternion.identity);

            for ( int j = 0; j < size; j++)
            {
                if (cells[i, j] == CELL_TYPE.WALL)
                {
                    nodes[j, i] = new Node(j, i, false);
                    switch (NumPathAdjacent(new Vector2Int(i, j)))
                    {
                        case 1:
                            if (j < size - 1 && cells[i, j + 1] != CELL_TYPE.WALL)
                            {
                                if (UnityEngine.Random.Range(0f, 1f) < 0.5f) PhotonNetwork.Instantiate("Right1", new Vector2(j, i), Quaternion.identity);
                                else PhotonNetwork.Instantiate("Right2", new Vector2(j, i), Quaternion.identity);
                            }
                            else if (j > 0 && cells[i, j - 1] != CELL_TYPE.WALL)
                            {
                                if (UnityEngine.Random.Range(0f, 1f) < 0.5f) PhotonNetwork.Instantiate("Left1", new Vector2(j, i), Quaternion.identity);
                                else PhotonNetwork.Instantiate("Left2", new Vector2(j, i), Quaternion.identity);
                            }
                            else if (i < size - 1 && cells[i + 1, j] != CELL_TYPE.WALL)
                            {
                                if (UnityEngine.Random.Range(0f, 1f) < 0.5f) PhotonNetwork.Instantiate("Top1", new Vector2(j, i), Quaternion.identity);
                                else PhotonNetwork.Instantiate("Top2", new Vector2(j, i), Quaternion.identity);
                            }
                            else if (i > 0 && cells[i - 1, j] != CELL_TYPE.WALL)
                            {
                                PhotonNetwork.Instantiate("Bottom", new Vector2(j, i), Quaternion.identity);
                            }
                            break;

                        case 2:
                            if (i == 0 || j == 0 || i == size - 1 || j == size - 1) PhotonNetwork.Instantiate("BottomLeft", new Vector2(j, i), Quaternion.identity);
                            else if (cells[i + 1, j] != CELL_TYPE.WALL && cells[i - 1, j] != CELL_TYPE.WALL) PhotonNetwork.Instantiate("Top2", new Vector2(j, i), Quaternion.identity);
                            else if (cells[i, j + 1] != CELL_TYPE.WALL && cells[i, j - 1] != CELL_TYPE.WALL) PhotonNetwork.Instantiate("BottomRight", new Vector2(j, i), Quaternion.identity);

                            else if (cells[i + 1, j] != CELL_TYPE.WALL && cells[i, j + 1] != CELL_TYPE.WALL) PhotonNetwork.Instantiate("TopRight", new Vector2(j, i), Quaternion.identity);
                            else if (cells[i + 1, j] != CELL_TYPE.WALL && cells[i, j - 1] != CELL_TYPE.WALL) PhotonNetwork.Instantiate("TopLeft", new Vector2(j, i), Quaternion.identity);
                            else if (cells[i - 1, j] != CELL_TYPE.WALL && cells[i, j + 1] != CELL_TYPE.WALL) PhotonNetwork.Instantiate("BottomRight", new Vector2(j, i), Quaternion.identity);
                            else if (cells[i - 1, j] != CELL_TYPE.WALL && cells[i, j - 1] != CELL_TYPE.WALL) PhotonNetwork.Instantiate("BottomLeft", new Vector2(j, i), Quaternion.identity);

                            break;

                        case 3:
                            PhotonNetwork.Instantiate("Block", new Vector2(j, i), Quaternion.identity);
                            break;
                        case 4:
                            PhotonNetwork.Instantiate("Dirt2", new Vector2(j,i),Quaternion.identity).AddComponent<BoxCollider2D>();
                            break;

                        default: //0
                            if (UnityEngine.Random.Range(0f,1f) < 0.8f) PhotonNetwork.Instantiate("Dirt1", new Vector2(j, i), Quaternion.identity);
                            else if (UnityEngine.Random.Range(0f, 1f) < 0.9f) PhotonNetwork.Instantiate("Dirt3", new Vector2(j, i), Quaternion.identity);
                            else PhotonNetwork.Instantiate("Dirt2", new Vector2(j, i), Quaternion.identity);
                            break;
                    }
                }
                else 
                {
                    GameObject temp = PhotonNetwork.Instantiate("Brick", new Vector2(j, i), Quaternion.identity) as GameObject;
                    nodes[j,i] = new Node(j, i, true);
                    temp.tag = "Path";
                }
            }
        }
        Grid.BuildGrid(nodes, size);
    }
    int NumPathAdjacent(Vector2Int pos)
    {
        int count = 0;
        foreach(DIR d in DIR_ARRAY)
        {
            int x = 0, y = 0;
            switch (d)
            {
                case DIR.UP:
                    x = 0;
                    y = -1;
                    break;

                case DIR.DOWN:
                    x = 0;
                    y = 1;
                    break;

                case DIR.LEFT:
                    x = -1;
                    y = 0;
                    break;

                case DIR.RIGHT:
                    x = 1;
                    y = 0;
                    break;
            }
            try
            {
                if (cells[pos.x + x, pos.y + y] != CELL_TYPE.WALL)
                {
                    count++; 
                }
            } catch { };
        }
        return count;
    }

    public bool Finished()
    {
        return builders.Count == 0;
    }

    void Next()
    {
        builders = builders.Where((builder) => MoveAndRemoveCollided(builder)).ToList();
    }
    
    bool MoveAndRemoveCollided(Builder builder)
    {
        builder.Move(spawners);

        // remove collided builders
        if (cells[builder.y, builder.x] != CELL_TYPE.CLOSED)
        {
            return false;
        }

        cells[builder.y, builder.x] = CELL_TYPE.BEAN;
        return true;
    }

    void InitStartPoints()
    {
        int half = size / 2 | 0;
        int[] x = Enumerable.Range(0,half).Select(xr => 2 * xr).ToArray();
        int[] y = Enumerable.Range(0, half).Select(yr => 2 * yr).ToArray();

        Shuffle(x);
        Shuffle(y);

        foreach(int i in Enumerable.Range(0, half))
        {
            _startPoints.Add(new Vector2Int(x[i],y[i]));
        }
    }

    void InitCells()
    {
        cells = new CELL_TYPE[size, size];
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                cells[i, j] = CELL_TYPE.WALL;
            }
        }
    }

    void CreateSpawner(int minDistance, int maxDistance)
    {
        if (_startPoints.Count == 0)
        {
            Debug.Log("too many spawners");
        }

        int limit = size - 1;
        Vector2Int xy = _startPoints[0];
        _startPoints.RemoveAt(0);

        cells[xy.y, xy.x] = CELL_TYPE.KERNEL;
        spawners.Add(new Vector2Int(xy.x, xy.y));

        DIR[] directions = DIR_ARRAY.Where((dir) => CreateSpawnerHelperSortDirections(xy,limit,dir)).ToArray();

        Shuffle(directions);

        int count = Math.Min(directions.Length, RandomInt(2, 4));

        for (int i = 0; i < count; i++)
        {
            builders.Add(new Builder(xy.x, xy.y, limit, directions[i], minDistance, maxDistance));
        }
    }

    bool CreateSpawnerHelperSortDirections(Vector2Int xy, int limit, DIR dir)
    {
        if ((xy.x == 0 || xy.x == 1) && dir == DIR.LEFT)
        {
            return false;
        }
        if ((xy.x == limit || xy.x == limit - 1) && dir == DIR.RIGHT)
        {
            return false;
        }

        if ((xy.y == 0 || xy.y == 1) && dir == DIR.UP)
        {
            return false;
        }
        if ((xy.y == limit || xy.y == limit - 1) && dir == DIR.DOWN)
        {
            return false;
        }

        return true;
    }

    public static int RandomInt(int lower = 0, int upper = 1)
    {
        return UnityEngine.Random.Range(lower, upper);
    }

    public static void Shuffle<T>(T[] array)
    {
        for (int i = 0, max = array.Length; i < max; i++)
        {
            int ramIdx = RandomInt(i, max - 1);
            T t = array[i];
            array[i] = array[ramIdx];
            array[ramIdx] = t;
        }
    }

    public static void Shuffle<T>(List<T> array)
    {
        for (int i = 0, max = array.Count; i < max; i++)
        {
            int ramIdx = RandomInt(i, max - 1);
            T t = array[i];
            array[i] = array[ramIdx];
            array[ramIdx] = t;
        }
    }

}

public class Builder
{
    private readonly int _startX;
    private readonly int _startY;

    private int _vx = 0;
    private int _vy = 0;

    public int x;
    public int y;
    public int limit;
    public DIR direction;
    public int _minDistance;
    public int _maxDistance;

    private int _remain;

    public Builder(int x,int y,int limit,DIR direction,int _minDistance,int _maxDistance )
    {
        _startX = x;
        _startY = y;
        this.x = x;
        this.y = y;
        this.limit = limit;
        this.direction = direction;
        this._minDistance = _minDistance;
        this._maxDistance = _maxDistance;

        _update();
    }

    public void Move(List<Vector2Int> spawners)
    {
        if (_remain == 0)
        {
            _turn(spawners);
            // return this.move();
        }

        x += _vx;
        y += _vy;
        _remain--;
    }

    void _update()
    {
        int distance = Generate.RandomInt(_minDistance, _maxDistance);

        switch (direction)
        {
            case DIR.UP:
                _vx = 0;
                _vy = -1;
                break;

            case DIR.DOWN:
                _vx = 0;
                _vy = 1;
                break;

            case DIR.LEFT:
                _vx = -1;
                _vy = 0;
                break;

            case DIR.RIGHT:
                _vx = 1;
                _vy = 0;
                break;
        }

        if (_vx == 1)
        {
            distance = Math.Min(distance, limit - x);
        }
        else if (_vx == -1)
        {
            distance = Math.Min(distance, x);
        }
        else if (_vy == 1)
        {
            distance = Math.Min(distance, limit - y);
        }
        else if (_vy == -1)
        {
            distance = Math.Min(distance, y);
        }

        if (!(distance % 2 == 0))
        {
            distance--;
        }

        _remain = distance;
    }

    DIR _chooseDirection(DIR direction, List<Vector2Int> spawners)
    {
        int weight = 0;

        int elemIdx;
        int valToCompare;

        switch (direction)
        {
            case DIR.HORIZONTAL:
                // x
                elemIdx = 0;
                valToCompare = x;
                break;

            case DIR.VERTICAL:
                // y
                elemIdx = 1;
                valToCompare = y;
                break;

            default:
                elemIdx = 0;
                valToCompare = 0;
                Debug.Log("Invalid Direction");
                break;
        }

        spawners.ForEach((s) => {
            // skip self
            if (s[0] == _startX && s[1] == _startY)
            {
                return;
            }

            weight += Math.Sign(s[elemIdx] - valToCompare);
        });

        return direction == DIR.HORIZONTAL ?
               (weight > 0 ? DIR.RIGHT : DIR.LEFT) :
               (weight > 0 ? DIR.DOWN : DIR.UP);
    }

    void _turn(List<Vector2Int> spawners)
    {
        DIR dir = direction;
        DIR back = (DIR)(((int)dir << 2 | (int)dir >> 2) & (int)DIR.ALL);

        DIR mask = DIR.ALL ^ dir ^ back;

        if (x == 0 || x == 1)
        {
            mask ^= DIR.LEFT;
        }
        if (x == limit || x == limit - 1)
        {
            mask ^= DIR.RIGHT;
        }

        if (y == 0 || y == 1)
        {
            mask ^= DIR.UP;
        }
        if (y == limit || y == limit - 1)
        {
            mask ^= DIR.DOWN;
        }

        DIR a = (DIR)(((int)dir << 1 | (int)dir >> 3 /* 4 - 1 */)) & mask; 
        DIR b = (DIR)(((int)dir << 3 | (int)dir >> 1 /* 4 - 3 */)) & mask;

        if (a != 0 && b != 0)
        {
            direction = _chooseDirection(a | b, spawners);
        }
        else
        {
            direction = a == 0 ? b : a;
        }

        _update();
    }
}

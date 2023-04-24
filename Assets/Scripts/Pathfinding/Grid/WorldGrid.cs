using UnityEngine;
using System.Diagnostics;

public class WorldGrid : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube((DL_BoundaryGameObject.transform.position + UR_BoundaryGameObject.transform.position) / 2, UR_BoundaryGameObject.transform.position - DL_BoundaryGameObject.transform.position);
    }

    [HideInInspector]
    public static WorldGrid _instance;

    public static WorldNode[,] NodeGrid;

    [SerializeField]
    private GameObject WalkableDebugNodeObject;
    [SerializeField]
    private GameObject EdgeDebugNodeObject;
    [SerializeField]
    private GameObject DangerDebugNodeObject;
    [SerializeField]
    private GameObject GroundDebugNodeObject;
    [SerializeField]
    private GameObject DropZoneDebugNodeObject;

    //Grid boundaries
    [SerializeField]
    private GameObject DL_BoundaryGameObject;
    [SerializeField]
    private GameObject UR_BoundaryGameObject;

    private int LeftDist;
    private int RightDist;
    private int UpDist;
    private int DownDist;

    private int GridWidth;
    private int GridHeight;

    private static IntVector2 WorldCentreNodePos;

    [SerializeField]
    private bool ShowGraph;

    [SerializeField]
    private int MaxConnectedNodeDist;
    private int SquaredMaxConnectedNodeDist;

    public static int MaxNumberOfConnectedNodes = 10;
    private IntVector2[] PathNodeAdresses;
    private int NumberOfPathNodes;

    IntVector2[] PlatformStartAdresses;
    private int NumberOfPlatforms;

    [SerializeField]
    private LayerMask GroundTileLayers;

    //private Stopwatch st;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            UnityEngine.Debug.LogError("Instance of singleton class \"" + this + "\" already exists in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }

        //st = new Stopwatch();
        //st.Start();

        LeftDist = (int)DL_BoundaryGameObject.transform.position.x - 1;
        DownDist = (int)DL_BoundaryGameObject.transform.position.y - 1;
        RightDist = (int)UR_BoundaryGameObject.transform.position.x + 1;
        UpDist = (int)UR_BoundaryGameObject.transform.position.y + 1;

        if (LeftDist >= RightDist)
        {
            UnityEngine.Debug.LogError("WorldGrid Left Boundary is to the right of the Right Boundary. Make sure the Left Boundary is to the left of the Right Boundary.");
        }
        if (DownDist >= UpDist)
        {
            UnityEngine.Debug.LogError("WorldGrid Down Boundary is above of the Up Boundary. Make sure the Deft Boundary is below the Up Boundary.");
        }

        WorldCentreNodePos = new IntVector2(-LeftDist, -DownDist);

        GridWidth = RightDist - LeftDist;
        GridHeight = UpDist - DownDist;

        NodeGrid = new WorldNode[GridWidth, GridHeight];
        NodeGrid[0, 0] = new WorldNode();

        SquaredMaxConnectedNodeDist = MaxConnectedNodeDist * MaxConnectedNodeDist; //Max node distance squared

        PlatformStartAdresses = new IntVector2[GridWidth]; //Possibly to small
        PathNodeAdresses = new IntVector2[(int)(GridWidth * 0.5f * GridHeight * 0.5f)]; //Possibly to small

        //WorldNode.SetLinkArraySize(MaxNumberOfConnectedNodes);
        WorldNode.SetLinkArraySize(0);

        GenerateWorldGrid();

        //st.Stop();
        //UnityEngine.Debug.Log($"Milliseconds to generate world grid: {st.ElapsedMilliseconds}, Total timestamp: {st.Elapsed}");
    }

    public static bool FreeSpaceAtPosition(Vector3 worldPosition)
    {
        IntVector2 nodeAdress = WorldToGridPosition(worldPosition);

        if(nodeAdress.x < 0 || nodeAdress.x >= _instance.GridWidth || nodeAdress.y < 0 || nodeAdress.y >= _instance.GridHeight) //Node is outside boundary.
        {
            return false;
        }
     
        return NodeAtAdress(nodeAdress).IsGround == false;
    }

    public static WorldNode NodeAtAdress(IntVector2 NodeAdress)
    {
        return NodeGrid[NodeAdress.x, NodeAdress.y];
    }

    public static IntVector2 WorldToGridPosition(Vector3 WorldPosition)
    {
        return new IntVector2(Mathf.FloorToInt(WorldCentreNodePos.x + WorldPosition.x), Mathf.FloorToInt(WorldCentreNodePos.y + WorldPosition.y));
    }

    public static IntVector2 WorldToGridPosition(Vector2 WorldPosition)
    {
        return new IntVector2(Mathf.FloorToInt(WorldCentreNodePos.x + WorldPosition.x), Mathf.FloorToInt(WorldCentreNodePos.y + WorldPosition.y));
    }

    public static Vector2 GridToWorldPosition(IntVector2 GridPosition)
    {
        return new Vector2(GridPosition.x - WorldCentreNodePos.x, GridPosition.y - WorldCentreNodePos.y);
    }

    public static Vector2 GridToWorldPosition(Vector2 GridPosition)
    {
        return new Vector2(GridPosition.x - WorldCentreNodePos.x, GridPosition.y - WorldCentreNodePos.y);
    }

    public static Vector2 GridToWorldPosition(float x, float y)
    {
        return new Vector2(x - WorldCentreNodePos.x, y - WorldCentreNodePos.y);
    }

    public static WorldNode NodeAtWorldPosition(Vector3 WorldPosition)
    {
        return NodeAtAdress(WorldToGridPosition(WorldPosition));
    }

    public static bool EdgeIsAtPosition(Vector2 WorldPosition)
    {
        return NodeAtWorldPosition(WorldPosition).IsEdge;
    }


    private void CreateNewPlatform(IntVector2 NodeAdress)
    {
        NumberOfPlatforms++;
        PlatformStartAdresses[NumberOfPlatforms] = NodeAdress;

        NodeAtAdress(NodeAdress).PlatformID = NumberOfPlatforms;
    }

    private void CreateEdgeNode(IntVector2 NodeAdress)
    {
        NodeAtAdress(NodeAdress).IsEdge = true;

        SavePathNodeAdress(NodeAdress);

    }

    private void SavePathNodeAdress(IntVector2 NodeAdress)
    {
        PathNodeAdresses[NumberOfPathNodes] = NodeAdress;
        NumberOfPathNodes++;

    }


    private void GenerateWorldGrid()
    {
        InstantiateNodeGrid();

        GenerateColumns();

        GenerateEdgeNodes();

        //GenerateDropZones();

        //GenerateNodeConnections();

        if (ShowGraph)
        {
            DebugNodeGraph();
        }
    }

    private void InstantiateNodeGrid()
    {
        //UnityEngine.Debug.Log($"--Instantiating {GridWidth * GridHeight} world nodes--");
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                NodeGrid[i, j] = new WorldNode();
            }
        }
    }


    private void GenerateColumn(int Column)
    {
        for (int ColumnY = GridHeight; ColumnY > 0; ColumnY -= 1)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(GridToWorldPosition(Column + 0.5f, ColumnY - 0.01f), Vector2.down, ColumnY, GroundTileLayers); ;

            if (raycastHit.distance < 0.01f)
            {
                if (raycastHit.collider == null) //Bottom of column is found
                {
                    return;

                }
                else //Raycast started inside obstacle;
                {
                    NodeGrid[Column, ColumnY + ~((ColumnY - 1) >> 31)].IsGround = true;
                    continue;
                }
            }

            raycastHit.distance += 0.02f; //Safety for very small inaccuracy for raycast.
            ColumnY -= (int)raycastHit.distance;

            NodeGrid[Column, ColumnY + ~((ColumnY - 1) >> 31)].IsGround = true;

            if (raycastHit.collider.gameObject.layer == 10)//Collision was with Danger
            {
                NodeGrid[Column, ColumnY + ~((ColumnY - 1) >> 31)].IsDanger = true; //Column - 1 -> Unit inside danger.
                continue;
            }
            NodeGrid[Column, ColumnY].IsWalkable = raycastHit.collider.gameObject.layer == 8; //Collision was with regular ground

            // Column + ~((Column -1) >> 31)) ////Column -1 if (Column - 1) >= 0 else Colummn
            if (NodeGrid[Column + ~((Column - 1) >> 31), ColumnY].PlatformID == -1) //Node to the left (or current if on first column) is not part of a platform
            {
                CreateNewPlatform(new IntVector2(Column, ColumnY)); //Create new platform starting from the current node
            }
            else //Node to the left is part of a platform
            {
                NodeGrid[Column, ColumnY].AddToPlatform(NodeGrid[Column - 1, ColumnY]); //Add this node to the node to the left's platforms. Can be "Column - 1" since this will never run on the first column (0);
            }
        }
    }

    private void GenerateColumns()
    {
        //UnityEngine.Debug.Log($"--Generating {GridWidth} columns--");
        for (int i = 0; i < GridWidth; i++)
        {
            GenerateColumn(i);
        }
    }

    private void GenerateEdgeNodes()
    {
        for (int CurrentPlatformId = 1; CurrentPlatformId <= NumberOfPlatforms; CurrentPlatformId++)
        {
            IntVector2 CurrentNodeLocation = PlatformStartAdresses[CurrentPlatformId];

            CreateEdgeNode(CurrentNodeLocation);

            if (NodeAtAdress(CurrentNodeLocation).NextNodeInFlatform == null)
            {
                continue;
            }

            while (NodeAtAdress(CurrentNodeLocation).NextNodeInFlatform != null)
            {
                CurrentNodeLocation.x++;
            }

            CreateEdgeNode(CurrentNodeLocation);
        }
    }


    private void GenerateDropZones() //Generate the drop zones for all of the edge nodes
    {
        int NumberOfEdgeNodes = NumberOfPathNodes;
        
        for (int i = 0; i < NumberOfEdgeNodes; i++)
        {
            GenerateDropZoneForNode(PathNodeAdresses[i]);
        }
    }

    private void GenerateDropZoneForNode(IntVector2 NodeAdress)
    {
        bool IsRightEdge = NodeAtAdress(NodeAdress).NextNodeInFlatform == null;

        IntVector2 TestedPosition = new IntVector2(NodeAdress.x + (IsRightEdge ? 1 : -1), NodeAdress.y);

        for (int StepsDown = 0; StepsDown < MaxConnectedNodeDist; StepsDown++)
        {
            if (TestedPosition.x < 0 || TestedPosition.x > GridWidth - 1 || TestedPosition.y < 0) //Tested node is outside the boundary of the world grid
            {
                return;
            }

            if (NodeGrid[TestedPosition.x, TestedPosition.y].IsGround) //Yes it should be here, dont move it. Otherwise it wont stop for edgenodes
            {
                return;
            }

            if (NodeAtAdress(TestedPosition).IsWalkable && !NodeAtAdress(TestedPosition).IsEdge) //DropZone node is found
            {
                NodeAtAdress(TestedPosition).IsDropZone = true;
                SavePathNodeAdress(TestedPosition);

                return;
            }

            TestedPosition.y--;
        }
    }


    private void GenerateNodeConnections()
    {
        for (int i = 0; i < NumberOfPathNodes; i++)
        {
            for (int j = i + 1; j < NumberOfPathNodes; j++)
            {
                MakeConnectionBetweenNodes(PathNodeAdresses[i], PathNodeAdresses[j]);
            }
        }
    }

    private void MakeConnectionBetweenNodes(IntVector2 StartNodeAdress, IntVector2 OtherNodeAdress)
    {
        if ((StartNodeAdress.SquaredDistanceTo(OtherNodeAdress) >= SquaredMaxConnectedNodeDist) && NodeAtAdress(StartNodeAdress).PlatformID != NodeAtAdress(OtherNodeAdress).PlatformID) //What?
        {
            return;
        }

        bool TargetIsToTheRight = StartNodeAdress.x < OtherNodeAdress.x;
        bool TargetIsBelow = StartNodeAdress.y > OtherNodeAdress.y;

        bool StartPositionChange = !(TargetIsToTheRight ^ TargetIsBelow);

        Vector2 RaycastStartPosition = new Vector2(StartNodeAdress.x + (StartPositionChange ? 0.99f : 0.01f), StartNodeAdress.y + 0.5f);
        Vector2 RaycastEndPosition = new Vector2(OtherNodeAdress.x + (StartPositionChange ? 0.99f : 0.01f), OtherNodeAdress.y + 0.5f);

        IntVector2 RelativePosition = OtherNodeAdress - StartNodeAdress;

        RaycastHit2D raycastHit = Physics2D.Linecast(GridToWorldPosition(RaycastStartPosition), GridToWorldPosition(RaycastEndPosition), GroundTileLayers);

        if (raycastHit.collider == null)
        {
            NodeAtAdress(StartNodeAdress).AddNodeConnection(RelativePosition, RelativePosition.Simpledistance());
            NodeAtAdress(OtherNodeAdress).AddNodeConnection(-RelativePosition, RelativePosition.Simpledistance());
        }
    }


    private void DebugNodeGraph() //Debugging purposes. Will show the graph in the game. Computational nightmare lol
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                if (NodeGrid[i, j].IsEdge)
                {
                    GameObject InstantiatedNode = Instantiate(EdgeDebugNodeObject, GridToWorldPosition(i + 0.5f, j), Quaternion.identity);

                    /*DebugNodeController NodeController = InstantiatedNode.GetComponent<DebugNodeController>();

                    for (int k = 0; k < NodeGrid[i, j].NumberOfLinks; k++)
                    {
                        NodeController.AddConnectedNodePosition(NodeGrid[i, j].LinkedNodes[k].RelativePosition + GridToWorldPosition(i + 0.5f, j));
                    }*/
                }
                /*else if (NodeGrid[i, j].IsDropZone)
                {
                    GameObject InstantiatedNode = Instantiate(DropZoneDebugNodeObject, GridToWorldPosition(i + 0.5f, j), Quaternion.identity);

                    DebugNodeController NodeController = InstantiatedNode.GetComponent<DebugNodeController>();
                    for (int k = 0; k < NodeGrid[i, j].NumberOfLinks; k++)
                    {
                        NodeController.AddConnectedNodePosition(NodeGrid[i, j].LinkedNodes[k].RelativePosition + GridToWorldPosition(i + 0.5f, j));
                    }
                }*/
                else if (NodeGrid[i, j].IsWalkable)
                {
                    Instantiate(WalkableDebugNodeObject, GridToWorldPosition(i + 0.5f, j), Quaternion.identity);
                }
                else if (NodeGrid[i, j].IsDanger)
                {
                    Instantiate(DangerDebugNodeObject, GridToWorldPosition(i + 0.5f, j + 0.5f), Quaternion.identity);

                }
                else if (NodeGrid[i, j].IsGround)
                {
                    Instantiate(GroundDebugNodeObject, GridToWorldPosition(i + 0.5f, j + 0.5f), Quaternion.identity);
                }

            }
        }
    }
}
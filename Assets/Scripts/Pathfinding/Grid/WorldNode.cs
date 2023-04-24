using UnityEngine;

/// <summary>
/// A node in the WorldGrid
/// </summary>
public class WorldNode
{
    public byte NodeStates = 0;

    private static byte walkable_flag = 1 << 0;
    private static byte edge_flag = 1 << 1;
    private static byte dropZone_flag = 1 << 2;
    private static byte danger_flag = 1 << 3;
    private static byte ground_flag = 1 << 4;

    public bool IsWalkable
    {
        get => FlagIsSet(walkable_flag);
        set
        {
            SetFlag(walkable_flag, value);
        }
    }

    public bool IsEdge
    {
        get => FlagIsSet(edge_flag);
        set
        {
            SetFlag(edge_flag, value);
        }
    }

    public bool IsDropZone
    {
        get => FlagIsSet(dropZone_flag);
        set
        {
            SetFlag(dropZone_flag, value);
        }
    }

    public bool IsDanger
    {
        get => FlagIsSet(danger_flag);
        set
        {
            SetFlag(danger_flag, value);
        }
    }

    public bool IsGround
    {
        get => FlagIsSet(ground_flag);
        set
        {
            SetFlag(ground_flag, value);
        }
    }


    private bool FlagIsSet(byte flag)
    {
        return (NodeStates & flag) > 0;
    }


    private void SetFlag(byte flag, bool value)
    {
        NodeStates = (byte)(NodeStates & ~flag);

        if (value)
        {
            NodeStates += flag;
        }
    }


    private void SetFlag(byte flag, int value)
    {
        NodeStates = (byte)((NodeStates & ~flag) + (flag & (~((value - 1) >> 31))));
    }


    public WorldNode NextNodeInFlatform = null;
    public int PlatformID = -1;

    private static int MaxNumberOfConnections;
    //public NodeConnection[] LinkedNodes; //Edgenodes wiewable from this note avaliable for pathfinding

    public int NumberOfLinks = 0;

    public WorldNode()
    {
        //LinkedNodes = new NodeConnection[MaxNumberOfConnections];
    }

    public void AddNodeConnection(IntVector2 RelativePosition, float Distance)
    {
        /*LinkedNodes[NumberOfLinks] = new NodeConnection(Distance, RelativePosition);
        NumberOfLinks++;*/
        //Debug.Log($"Number of links = {NumberOfLinks}");
    }

    public static void SetLinkArraySize(int _MaxNumberOfConnections)
    {
        MaxNumberOfConnections = _MaxNumberOfConnections;
    }


    public void AddToPlatform(WorldNode PreviousPlatformNode)
    {
        PlatformID = PreviousPlatformNode.PlatformID;
        PreviousPlatformNode.NextNodeInFlatform = this;
    }
}

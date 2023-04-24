using UnityEngine;

public static class GroundNavigator
{
    private static Vector2 HalfUnit = new Vector2(0.5f, 0.5f);
    private static Vector2 HalfX = new Vector2(0.5f, 0);

    /// <summary>
    /// Returns the adress of the node at the edge of the platform in the specified direction.
    /// </summary>
    public static IntVector2 NodeAtEndOfPlatform(IntVector2 StartNodeAdress, int Direction)
    {
        do
        {
            StartNodeAdress.x += Direction;
        }
        while (WorldGrid.NodeAtAdress(StartNodeAdress).IsWalkable == true);
 
        StartNodeAdress.x -= Direction;

        return StartNodeAdress;
    }

    /// <summary>
    /// Returnes the number of nodes left on the platform in the scefified direction.
    /// </summary>
    private static int NodesLeftOfPlatform(IntVector2 StartNodePosition, int Direction)
    {
        return Mathf.Abs(StartNodePosition.x - NodeAtEndOfPlatform(StartNodePosition, Direction).x);
    }

    /// <summary>
    /// Returns the position of the centre of the edge node at the end of the specified platform in the direction specified.
    /// </summary>
    public static Vector2 CentreOfNodeAtEndOfPlatform(IntVector2 PlatformAdress, int Direction)
    {
        return WorldGrid.GridToWorldPosition(NodeAtEndOfPlatform(PlatformAdress, Direction) + HalfUnit);
    }

    /// <summary>
    /// Returns the position of where the collider is standing.
    /// </summary>
    public static Vector2 CurrentPosition(Collider2D Collider)
    {
        return new Vector2(Collider.bounds.center.x, Collider.bounds.center.y - Collider.bounds.extents.y);
    }

    /// <summary>
    /// Returns the adress of where the main collider is standing.
    /// </summary>
    public static IntVector2 CurrentAdress(Collider2D Collider)
    {
        return WorldGrid.WorldToGridPosition(CurrentPosition(Collider));
    }

    /// <summary>
    /// Returns the node where the collider is standing.
    /// </summary>
    public static WorldNode NodeAtCurrentPosition(Collider2D Collider)
    {
        return WorldGrid.NodeAtWorldPosition(CurrentPosition(Collider));
    }

    /// <summary>
    /// Returns the world position of the centre of the node adressed.
    /// </summary>
    public static Vector2 CentrePositionOfNode(IntVector2 NodeAdress)
    {
        return WorldGrid.GridToWorldPosition(NodeAdress) + HalfUnit;
    }

    /// <summary>
    /// Returns the world position of the bottom-centre of the node adressed
    /// </summary>
    public static Vector2 BottomPositionOfNode(IntVector2 NodeAdress)
    {
        return WorldGrid.GridToWorldPosition(NodeAdress) + HalfX;
    }

    /// <summary>
    /// Returnes the distance untin the edge of the platform the main collider is standing on in the specified direction.
    /// </summary>
    public static float DistanceLeftOfPlatform(Collider2D MainCollider, int Direction)
    {
        Vector2 AgentPosition = CurrentPosition(MainCollider);
        IntVector2 AgentAdress = WorldGrid.WorldToGridPosition(AgentPosition);

        return Mathf.Abs(CentreOfNodeAtEndOfPlatform(AgentAdress, Direction).x - (AgentPosition.x + (MainCollider.bounds.extents.x - 0.4f) * Direction));
    }

    #region Get next node
    public static IntVector2 RandomNodeFromArray(IntVector2[] NodeAdresses)
    {
        return NodeAdresses[Random.Range(0, NodeAdresses.Length)];
    }

    /// <summary>
    /// Returns a the adress of a random node connected to the specified node.
    /// </summary>
    /*public static IntVector2 RandomConnectedNode(IntVector2 NodeAdress)
    {
        WorldNode NodeAtPosition = WorldGrid.NodeAtAdress(NodeAdress);

        return NodeAtPosition.LinkedNodes[Random.Range(0, NodeAtPosition.NumberOfLinks)].RelativePosition + NodeAdress;
    }*/

    /// <summary>
    /// Returns an queue of the adress of all connected nodes whose relative x position is in the specified direction
    /// </summary>
    /*public static Stack<IntVector2> GetConnectedNodesInDirection(IntVector2 NodeAdress, int Direction)
    {
        WorldNode StartNode = WorldGrid.NodeAtAdress(NodeAdress);

        Stack<IntVector2> PossibleNodes = new Stack<IntVector2>(WorldGrid.MaxNumberOfConnectedNodes);

        for (int i = 0; i < WorldGrid.MaxNumberOfConnectedNodes; i++)
        {
            if (StartNode.LinkedNodes[i] == null)
            {
                break;
            }

            if (StartNode.LinkedNodes[i].RelativePosition.x * Direction > 0)
            {
                PossibleNodes.Push(NodeAdress + StartNode.LinkedNodes[i].RelativePosition);
            }
        }

        return PossibleNodes;
    }*/
    #endregion
}

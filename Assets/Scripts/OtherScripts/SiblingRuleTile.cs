using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(fileName = "NewSiblingRuleTile", menuName = "2D/Tiles/SiblingRuleTile", order = 110)]
public class SiblingRuleTile : RuleTile<SiblingRuleTile.Neighbor>
{
    public bool alwaysConnect = true;

    public TileBase[] tilesToConnectTo;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Empty = 3;
    }

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    if (alwaysConnect)
                    {
                        return other != null;
                    }
                    else
                    {
                        return tilesToConnectTo.Contains(other) || other == this;
                    }
                }
            case TilingRule.Neighbor.NotThis:
                {
                    if (alwaysConnect)
                    {
                        return other == null;
                    }
                    else
                    {
                        return !tilesToConnectTo.Contains(other) && other != this;
                    }
                }
            case Neighbor.Empty:
                return other == null;
        }

        return base.RuleMatch(neighbor, other);
    }
}

/*
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ExampleSiblingRuleTile : RuleTile
{

    public enum SibingGroup
    {
        Poles,
        Terrain,
    }
    public SibingGroup sibingGroup;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return other is ExampleSiblingRuleTile
                        && (other as ExampleSiblingRuleTile).sibingGroup == this.sibingGroup;
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !(other is ExampleSiblingRuleTile
                        && (other as ExampleSiblingRuleTile).sibingGroup == this.sibingGroup);
                }
        }

        return base.RuleMatch(neighbor, other);
    }
}
*/

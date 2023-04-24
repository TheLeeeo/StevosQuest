using UnityEngine;

public static class GroundAIMovementController
{
    private static int NormalizedScale;

    private static void SetNormalizedScale(Transform Agent_Transform)
    {
        NormalizedScale = Agent_Transform.lossyScale.x > 0 ? 1 : -1;
    }

    public static void Jump(Rigidbody2D Agent_Rigidbody, float JumpForce)
    {
        Agent_Rigidbody.velocity = new Vector2(Agent_Rigidbody.velocity.x, JumpForce);
    }


    public static bool ClearUnitInFront(Collider2D Agent_Collider) //Checks unit infront if it is clear
    {
        SetNormalizedScale(Agent_Collider.transform);

        Vector2 RaycastOrigin = new Vector2(Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x * NormalizedScale, Agent_Collider.bounds.center.y);
        RaycastHit2D raycastHit = Physics2D.Raycast(RaycastOrigin, Vector2.right * Agent_Collider.transform.lossyScale.x, Agent_Collider.bounds.extents.x, CommonLayerMasks.GroundCheckLayers);

        Debug.DrawRay(RaycastOrigin, Vector2.right * Agent_Collider.bounds.extents.x * NormalizedScale, raycastHit.collider == null ? Color.blue : Color.red);

        return raycastHit.collider == null;
    }


    public static float DistanceToWall(Collider2D Agent_Collider, int DistanceToCast) //Checks 0.05 units above ground for obstacles
    {
        SetNormalizedScale(Agent_Collider.transform);

        Vector2 RaycastOrigin = new Vector2(Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x * NormalizedScale, Agent_Collider.bounds.center.y - Agent_Collider.bounds.extents.y + 0.05f);
        RaycastHit2D raycastHit = Physics2D.Raycast(RaycastOrigin, Vector2.right * Agent_Collider.transform.lossyScale.x, DistanceToCast, CommonLayerMasks.GroundCheckLayers);

        Debug.DrawRay(RaycastOrigin, Vector2.right * (raycastHit.distance > 0 ? raycastHit.distance : DistanceToCast) * NormalizedScale, Color.yellow);

        return raycastHit.distance > 0 ? raycastHit.distance : DistanceToCast;
    }

    public static float DistanceToBottomOfHole(Collider2D Agent_Collider, int DistanceDown) //Distance to the bottom of the hole in front, 0 if bottom is not found
    {
        SetNormalizedScale(Agent_Collider.transform);

        int Startposition;

        if (NormalizedScale > 0)
        {
            Startposition = Mathf.FloorToInt(Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x);
        }
        else
        {
            Startposition = Mathf.CeilToInt(Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x);
        }

        Vector2 RaycastOrigin = new Vector2(Startposition + 0.5f, Agent_Collider.bounds.center.y - Agent_Collider.bounds.extents.y);
        RaycastHit2D raycastHit = Physics2D.Raycast(RaycastOrigin, Vector2.down, DistanceDown, CommonLayerMasks.GroundCheckLayers);

        Debug.DrawRay(RaycastOrigin, Vector2.down * DistanceDown, raycastHit.collider == null ? Color.blue : Color.red);

        return raycastHit.collider != null ? raycastHit.distance : 0;
    }


    public static float TimeToFallDistance(float Distance)
    {
        return Mathf.Sqrt(Distance / 5);
    }


    public static float ForceToJumpHeight(float Height)
    {
        return 10 * Mathf.Sqrt(0.2f * Height);
    }


    


    public static float DistanceClearUntilHole(Collider2D Agent_Collider, int DistanceToCheck) //Checks all of the specified units in front for a hole
    {
        SetNormalizedScale(Agent_Collider.transform);
        int Startposition;

        if (NormalizedScale > 0)
        {
            Startposition = Mathf.CeilToInt(Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x);
        }
        else
        {
            Startposition = Mathf.FloorToInt(Agent_Collider.bounds.center.x - Agent_Collider.bounds.extents.x);
        }

        Vector2 RaycastOrigin = new Vector2(Startposition, Agent_Collider.bounds.center.y - Agent_Collider.bounds.extents.y - 0.05f);
        RaycastHit2D raycastHit = new RaycastHit2D();

        for (int i = 0; i < DistanceToCheck; i++)
        {
            RaycastOrigin.x += NormalizedScale;
            raycastHit = Physics2D.Raycast(RaycastOrigin, Vector2.left * NormalizedScale, 1.1f, CommonLayerMasks.GroundCheckLayers);

            if (raycastHit.distance > 0.1f)
            {
                Debug.DrawRay(RaycastOrigin - Vector2.right * NormalizedScale, Vector2.right * NormalizedScale *  (1 - raycastHit.distance), Color.yellow);
                break;
            }

            Debug.DrawRay(RaycastOrigin, Vector2.left * NormalizedScale, Color.yellow);
        }
        return raycastHit.point.x - Agent_Collider.bounds.center.x - Agent_Collider.bounds.extents.x * NormalizedScale; //Distance from edge of collider to raycast hit
    }

    public static Vector2 CheckForTopOfWall(Collider2D Agent_Collider, int DistanceToCheck) //Scans the wall in unit in front for the top
    {
        SetNormalizedScale(Agent_Collider.transform);

        float XPositionOfWall;

        RaycastHit2D raycastHit;

        if (NormalizedScale == 1)
        {
            XPositionOfWall = Mathf.CeilToInt(Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x) + 0.5f;
        }
        else
        {
            XPositionOfWall = Mathf.FloorToInt(Agent_Collider.bounds.center.x - Agent_Collider.bounds.extents.x) - 0.5f;
        }

        Vector2 raycastOrigin = new Vector2(XPositionOfWall, Agent_Collider.bounds.center.y - Agent_Collider.bounds.extents.y + 1);

        Debug.DrawRay(raycastOrigin - Vector2.left * 0.05f, Vector2.up * DistanceToCheck, Color.magenta);

        for (int i = 1; i < DistanceToCheck; i++)//CAUTIOUS ---- i starts as 1 ---- , it is because checking the first unit is useless
        {
            raycastOrigin.y += 1;

            raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.down, 1.1f, CommonLayerMasks.GroundCheckLayers);

            if (raycastHit.distance > 0.75f)
            {
                Vector2 RelativePosition = raycastHit.point - new Vector2(Agent_Collider.bounds.center.x, Agent_Collider.bounds.center.y -Agent_Collider.bounds.extents.y);
                RelativePosition.x -= 0.5f * NormalizedScale; //Make point to land at the edge of the unit

                Debug.DrawRay(raycastOrigin, Vector2.down * 1.1f, Color.yellow);

                Debugging.DrawCross(RelativePosition + (Vector2)Agent_Collider.bounds.center, 0.2f, Color.cyan);
                //Debug.DrawRay(RelativePosition + (Vector2)Agent_Collider.bounds.center, Vector2.up, Color.cyan);

                return RelativePosition; //The point on the edge of the top of the wall
            }
        }


        return Vector2.zero; //Top of wall is above the checked distance
    }


    private static Vector2 CheckForTopOfWall(Vector2 WallPosition, float ExtraJumpHeight)
    {
        RaycastHit2D raycastHit;

        Vector2 raycastOrigin = new Vector2(WallPosition.x + 0.5f * NormalizedScale, Mathf.CeilToInt(WallPosition.y) + 1);

        raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.down, 1.1f, CommonLayerMasks.GroundCheckLayers);

        if (raycastHit.distance > 0.75f)
        {
            Vector2 TopOfWall = raycastHit.point;
            TopOfWall.x -= 0.5f * NormalizedScale; //Make point to land at the edge of the unit

            return TopOfWall; //The point on the edge of the top of the wall
        }

        return Vector2.zero; //Top of wall is above the checked distance
    }


    public static bool IfHoleInFront(Collider2D Agent_Collider) //Checks the ground unit in front for a hole
    {
        SetNormalizedScale(Agent_Collider.transform);

        float XPositionToStart;

        float AgentXBounds = Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x;

        if (NormalizedScale == 1)
        {
            XPositionToStart = Mathf.Ceil(Agent_Collider.bounds.center.x + Agent_Collider.bounds.extents.x) + 1;
        }
        else
        {
            XPositionToStart = Mathf.Floor(Agent_Collider.bounds.center.x - Agent_Collider.bounds.extents.x) - 1;
        }

        Vector2 raycastOrigin = new Vector2(XPositionToStart, Agent_Collider.bounds.center.y - Agent_Collider.bounds.extents.y - 0.05f);
        RaycastHit2D raycastHit = Physics2D.Raycast(raycastOrigin, Vector2.left * NormalizedScale, 1, CommonLayerMasks.GroundCheckLayers);

        Debug.DrawRay(raycastOrigin, Vector2.left * NormalizedScale, Color.yellow);

        return raycastHit.collider == null;

        //return raycastHit.point.x - XPositionToStart - NormalizedScale; // Distance from collider border to raycast hit
    }


    public static JumpParameters ParametersToJumpTo(Vector2 RelativeJumpLocation, float MaxSpeed, float MaxJumpForce, float MaxJumpHeight) //(Speed, JumpForce), null if unreachable
    {
        if (RelativeJumpLocation.y < 0) //Destination is below current position
        {
            RelativeJumpLocation.y *= -1;

            float TimeForFall = TimeToFallDistance(RelativeJumpLocation.y);

            if (RelativeJumpLocation.x <= TimeForFall * MaxSpeed) // Destinaiton can be reached without jumping
            {
                //Debug.Log("Does not have to jump");
                return new JumpParameters(RelativeJumpLocation.x / TimeForFall, 0);
            }
            else //Agent has to jump to reach destination
            {
                float TotalTime = RelativeJumpLocation.x / MaxSpeed;

                if (RelativeJumpLocation.x / TotalTime > MaxSpeed) //Destination out of range
                {
                    return JumpParameters.Zero;
                }

                //Debug.Log("Has to jump");
                return new JumpParameters(MaxSpeed, -(RelativeJumpLocation.y / TotalTime - 5 * TotalTime));
            }
        }
        else if (RelativeJumpLocation.y > 0.5f)//Destination is above current position
        {
            if (RelativeJumpLocation.y > MaxJumpHeight)
            {
                return JumpParameters.Zero; //Destination is of too high elevation
            }

            float TimeToJump = TimeToFallDistance(RelativeJumpLocation.y);

            if (TimeToJump * MaxSpeed < RelativeJumpLocation.x) //Have to jump more than the heigh of the landing
            {
                //Debug.Log("Has to do the jump");

                float TotalTime = RelativeJumpLocation.x / MaxSpeed;
                float JumpForceToUse = -(-RelativeJumpLocation.y / TotalTime - 5 * TotalTime);

                if (JumpForceToUse > MaxJumpForce)
                {
                    return JumpParameters.Zero;
                }

                return new JumpParameters(MaxSpeed, JumpForceToUse);
            }
            else //Can jump the height of the landing to reach the target
            {
                //Debug.Log("No has do do the jump");
                return new JumpParameters(RelativeJumpLocation.x / TimeToJump, ForceToJumpHeight(RelativeJumpLocation.y) + 0.1f);
            }

        }

        return JumpParameters.Zero;
    }


    public static float CheckForBottomOfHole(Collider2D Agent_Collider, float DistanceToCheck)
    {
        return -1;
    }


    public static float ClearDistanceAbove(Collider2D Agent_Collider, float DistanceToCheck)
    {
        return -1;
    }


    public static float DistanceClearUntilWall(Collider2D Agent_Collider, float DistanceToCheck)
    {
        return -1;
    }

}

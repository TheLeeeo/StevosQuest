using UnityEngine;

public struct JumpParameters
{
    public float Speed;
    public float JumpForce;

    public static JumpParameters Zero { private set; get; }

    public JumpParameters(float _Speed_, float _JumpForce_)
    {
        Speed = _Speed_;
        JumpForce = _JumpForce_;
    }

    static JumpParameters()
    {
        Zero = new JumpParameters(0, 0);
    }

    public static bool operator ==(JumpParameters a, JumpParameters b)
    {
        return Mathf.Abs(a.Speed - b.Speed) < 0.0001f && Mathf.Abs(a.JumpForce - b.JumpForce) < 0.0001f;
    }

    public static bool operator !=(JumpParameters a, JumpParameters b)
    {
        return !(Mathf.Abs(a.Speed - b.Speed) < 0.0001f && Mathf.Abs(a.JumpForce - b.JumpForce) < 0.0001f);
    }

    public override string ToString() => $"{Speed}, {JumpForce}";

    public override bool Equals(object obj) //Not implemented, dont care
    {
        throw new System.NotImplementedException();
    }
    public override int GetHashCode()  //Not implemented, dont care
    {
        throw new System.NotImplementedException();
    }

    #region Calculate jump parameters
    private static float TimeToFallDistance(float Distance)
    {
        return Mathf.Sqrt(Distance / 5); //Will only work for Gravity 10
    }

    public static float ForceToJumpHeight(float Height)
    {
        return 10 * Mathf.Sqrt(0.2f * Height); //Will only work for Gravity 10
    }


    private static JumpParameters ParametersForJump(Vector2 RelativePosition, EntityController entityController)
    {
        return CalculateJumpParameters(RelativePosition, entityController.movementSpeed, entityController.jumpForce, entityController.jumpHeight);
    }

    public static JumpParameters ParametersTo(IntVector2 GridAdress, EntityController entityController)
    {
        return ParametersToJumpTo(GroundNavigator.BottomPositionOfNode(GridAdress), entityController);
    }


    public static JumpParameters ParametersToJumpTo(Vector2 WorldPosition, EntityController entityController)
    {
        Vector2 EntityPosition = new Vector2(entityController.AI.groundCollider.bounds.center.x, entityController.AI.groundCollider.bounds.center.y - entityController.AI.groundCollider.bounds.extents.y);
        Vector2 TargetPosition = new Vector2(WorldPosition.x - (entityController.AI.groundCollider.bounds.extents.x + 0.5f - 0.1f) * Mathf.Sign(WorldPosition.x - EntityPosition.x), WorldPosition.y);

        return CalculateJumpParameters(TargetPosition - EntityPosition, entityController.movementSpeed, entityController.jumpForce, entityController.jumpHeight);
    }

    /// <summary>
    /// Calculates the speed and jump force required to reatch the relative position.
    /// </summary>
    /// <returns>Speed and JumpForce if point is reachable, JumpParameters.Zero if unreachable</returns>
    private static JumpParameters CalculateJumpParameters(Vector2 RelativeJumpLocation, float Speed, float JumpForce, float JumpHeight)
    {
        int actualRelativeXSign = (int)Mathf.Sign(RelativeJumpLocation.x); //Saves the sign of the relative location to compensate for the removal of the sign in the RelativeJumpLocation variable

        RelativeJumpLocation.x = Mathf.Abs(RelativeJumpLocation.x);

        if (RelativeJumpLocation.y < -0.1f) //Destination is below current position
        {
            RelativeJumpLocation.y *= -1;

            float TimeForFall = TimeToFallDistance(RelativeJumpLocation.y);

            if (RelativeJumpLocation.x <= TimeForFall * Speed) // Destinaiton can be reached without jumping
            {
                return new JumpParameters(RelativeJumpLocation.x * actualRelativeXSign / TimeForFall, 0);
            }
            else //Agent has to jump to reach destination
            {
                float TotalTime = RelativeJumpLocation.x / Speed; //Time it takes to move to destination

                if (RelativeJumpLocation.x / TotalTime > Speed)
                {
                    return JumpParameters.Zero; //Destination out of range
                }

                return new JumpParameters(Speed * actualRelativeXSign, (-(RelativeJumpLocation.y / TotalTime - 5 * TotalTime)) + 0.2f);
            }
        }
        else //Destination is above or at same height current position
        {
            if (RelativeJumpLocation.y > JumpHeight)
            {
                return JumpParameters.Zero; //Destination is of too high elevation
            }

            if (RelativeJumpLocation.y < 0.2f)
            {
                RelativeJumpLocation.y = 0.2f; //0.2 for safety lol
            }

            float TimeToJump = TimeToFallDistance(RelativeJumpLocation.y);

            if (TimeToJump * Speed < RelativeJumpLocation.x) //Has to jump more than the heigh of the landing
            {

                float TotalTime = RelativeJumpLocation.x / Speed;
                float JumpForceToUse = -(-RelativeJumpLocation.y / TotalTime - 5 * TotalTime);

                if (JumpForceToUse > JumpForce)
                {
                    return JumpParameters.Zero; //Cant jump high enough
                }
                return new JumpParameters(Speed * actualRelativeXSign, JumpForceToUse);
            }
            else //Can jump the height of the landing to reach the target
            {
                return new JumpParameters(RelativeJumpLocation.x * actualRelativeXSign / TimeToJump, ForceToJumpHeight(RelativeJumpLocation.y + 0.2f));
            }

        }
    }

    public static JumpParameters HighestXJump(float distance, float MaxY) //no safety checks at all because yolo and such
    {
        float airTime = 2 * TimeToFallDistance(MaxY);

        return new JumpParameters(distance / airTime, ForceToJumpHeight(MaxY));
    }
    #endregion
}
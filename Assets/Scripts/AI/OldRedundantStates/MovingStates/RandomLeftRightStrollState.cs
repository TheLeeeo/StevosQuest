using UnityEngine;

public class RandomLeftRightStrollState// : State //Dont fockinn work corectli an i donot kare
{
    /*public RandomLeftRightStrollState(AIBase _Agent_AIBase_, int _MinDistanceToMove_, int _MaxDistanceToMove_)
    {
        base.AgentAIBase = _Agent_AIBase_;

        CurrentSpeed = base.AgentAIBase.AgentEntityController.movementSpeed;

        MinDistanceToMove = _MinDistanceToMove_;
        MaxDistanceToMove = _MaxDistanceToMove_;
    }

    private int MinDistanceToMove;
    private int MaxDistanceToMove;

    private float DistanceLeftToMove; //How much further the agent has to move
    private int DirectionToMove; //In what direction the agent has to move, 1 for right, -1 for left

    private float DistanceLeftOfPlatform;

    private float CurrentSpeed;

    private IntVector2 TargetNode;

    public override void StateEnter()
    {
        DistanceLeftToMove = MinDistanceToMove + AgentAIBase.ControlIterator % (MaxDistanceToMove - MinDistanceToMove + 1); //Set how far the agent will move
        AgentAIBase.ControlIterator++;

        DirectionToMove = AgentAIBase.ControlIterator % 2; //Set what direction the agent will move
        AgentAIBase.ControlIterator++;

        if (DirectionToMove == 0)
        {
            DirectionToMove = -1;
        }

        DirectionToMove = 1;

        if (GroundCheck.CheckForGrounded(AgentAIBase.MainCollider))
        {
            DistanceLeftOfPlatform = GroundNavigator.DistanceLeftOfPlatform(AgentAIBase.MainCollider, DirectionToMove);
        }
        else
        {
            IsJumping = true;
        }

        /*Queue<IntVector2> a = GroundNavigator.GetConnectedNodesInDirection(GroundNavigator.CurrentAdress(AgentAIBase.MainCollider), 1);

        for(int i = 0; i < a.Length; i++)
        {
            Debug.Log("aaaa " + GroundNavigator.ParametersToJumpTo(a[i], AgentAIBase));
        }*/
    /*}


    public override void StateUpdate()
    {
        if(IsJumping)
        {
            if (GroundCheck.CheckForGrounded(AgentAIBase.MainCollider)) //Is touching ground
            {
                if (false == JustJumped) //Agent is in the air
                {
                    IsJumping = false;
                    CurrentSpeed = AgentAIBase.AgentEntityController.movementSpeed;

                    DistanceLeftOfPlatform = GroundNavigator.DistanceLeftOfPlatform(AgentAIBase.MainCollider, DirectionToMove);

                    if (DistanceLeftOfPlatform < 0.1f)//Node landed on is only a single block
                    {
                        JustLanded = true;
                    }
                }

            } else
            {
                JustJumped = false;
            }
        }
        else
        {
            DistanceLeftToMove -= CurrentSpeed * Time.deltaTime;
            DistanceLeftOfPlatform -= CurrentSpeed * Time.deltaTime;

            if (DistanceLeftOfPlatform < 0.1f) //Target is reached or overshot
            {
                AgentAIBase.Rigidbody.position = new Vector2(AgentAIBase.Rigidbody.position.x + DistanceLeftOfPlatform * DirectionToMove, base.AgentAIBase.Rigidbody.position.y);
                CurrentSpeed = 0;

                Stack<IntVector2> PossibleFutureNodes = GroundNavigator.GetConnectedNodesInDirection(GroundNavigator.CurrentAdress(AgentAIBase.MainCollider), DirectionToMove);

                JumpParameters JumpParamsToNextNode;

                if(PossibleFutureNodes.Length == 0) //No nodes in direction to move
                {
                    if(JustLanded)
                    {
                        //AgentAIBase.Inform("LeaveStrollState");
                    }

                    DirectionToMove *= -1;
                }
                else
                {
                    JumpParamsToNextNode = GroundNavigator.GetParamsToReachableNodeFromStack(PossibleFutureNodes, AgentAIBase);

                    if (JumpParamsToNextNode == JumpParameters.zero) //No avaliable node to jump to
                    {
                        DirectionToMove *= -1;
                    }
                    else
                    {
                        CurrentSpeed = JumpParamsToNextNode.Speed;
                        AgentAIBase.Rigidbody.velocity = new Vector2(CurrentSpeed * DirectionToMove, JumpParamsToNextNode.JumpForce);

                        IsJumping = true;
                        JustJumped = true;
                    }
                }
                
            }
        }


        AgentAIBase.Rigidbody.velocity = new Vector2(CurrentSpeed * DirectionToMove, base.AgentAIBase.Rigidbody.velocity.y);
    }*/
}

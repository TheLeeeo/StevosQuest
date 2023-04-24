public enum InformEnum
{
    #region Intercepts
    Disturbed, //When womething disturbes the entity, like knockback //NOT USED ANYMORE
    Calmed, //For when entity is calmed by outside sources. Not targeting player anymore //NOT USED ANYMORE
    #endregion

    #region Errors
    JumpUnavaliable,
    #endregion

    #region Timing
    TimerFinished,
    GotGrounded,
    #endregion

    #region GroundMovement
    MovementCompleted,
    ReachedXPosition,
    ReachedTargetRange,
    JumpCompleted,
    DropCompleted,
    ReachedWall,
    ReachedEdge,
    RechedHole,
    #endregion

    #region FlyingMovement
    Flight_MovementCompleted,
    Flight_ReachedTargetPosition,
    Flight_LandedOnGround,
    #endregion

    #region Aggresion
    OutOfRangeTimer,
    #endregion
}

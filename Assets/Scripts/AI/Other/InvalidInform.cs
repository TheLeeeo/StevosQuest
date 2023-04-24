using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidInform
{
    public static void Error(InformEnum InvalidInform, State sourceState) //Will get called if Agent AI does not handle an inform.
    {
        Debug.LogError("Inform of name " + InvalidInform + " is not handled in state: " + sourceState);
    }

    public static void Error(InformEnum InvalidInform, AIBase sourceAIBase) //Will get called if Agent AI does not handle an inform.
    {
        Debug.LogError("Inform of name " + InvalidInform + " is not handled on entity: " + sourceAIBase.gameObject);
    }
}

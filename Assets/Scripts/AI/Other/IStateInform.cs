using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateInform
{
    void Inform(InformEnum stateInfo); //cant be abstract, dont change it
}

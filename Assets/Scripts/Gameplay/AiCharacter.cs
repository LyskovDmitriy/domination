using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiCharacter : Character
{
    public override void StartTurn(bool isFirstTurn)
    {
        base.StartTurn(isFirstTurn);

        FinishTurn();
    }
}

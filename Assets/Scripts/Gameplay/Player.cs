using System;
using System.Collections.Generic;
using UnityEngine;
using Domination.Ui;


public class Player : Character
{
    public static event Action<int> OnCoinsCountChange;

    public Player()
    {
        LevelUi.OnEndTurnButtonClick += FinishTurn;
    }

    public override void Init(Castle castle)
    {
        base.Init(castle);
    }


    protected override void SetNewCoinsCount(int money)
    {
        base.SetNewCoinsCount(money);

        OnCoinsCountChange?.Invoke(money);
    }
}

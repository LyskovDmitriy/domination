using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Character
{
    public event Action OnTurnFinish;

    private const int DefaultCoinsCount = 50;

    protected List<Settlment> settlments = new List<Settlment>();
    private int coins = DefaultCoinsCount;


    public int Income => settlments.Sum((settlment) => settlment.Income);

    public int Coins
    {
        get => coins;
        set
        {
            if (coins != value)
            {
                SetNewCoinsCount(value);
            }
        }
    }


    public virtual void Init(Castle castle)
    {
        settlments.Add(castle);
    }


    public virtual void StartTurn(bool isFirstTurn)
    {
        if (!isFirstTurn)
        {
            Coins += Income;
        }
    }


    public bool HasSettlment(int settlmentId) => settlments.Exists((settlment) => settlment.Id == settlmentId);


    protected void FinishTurn()
    {
        OnTurnFinish?.Invoke();
    }


    protected virtual void SetNewCoinsCount(int money)
    {
        this.coins = money;
    }
}

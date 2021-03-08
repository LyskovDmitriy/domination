using Domination;
using Domination.LevelLogic;
using NUnit.Framework;
using Random = UnityEngine.Random;
using System;
using System.Linq;
using UnityEngine;


public class CharacterStateRestore
{
    [Test]
    public void RestoreCharacterState(
        [Random(10000, 100000, 1)] int initialCoinsCount, 
        [Random(2, 4, 1)] int settlmentsToAttackCount,
        [Random(2, 5, 1)] int ownedSettlmentsCount)
    {
        var settlmentsToAttack = CreateSettlments(settlmentsToAttackCount);
        var ownedSettlments = CreateSettlments(ownedSettlmentsCount);
        var allSettlments = settlmentsToAttack.Concat(ownedSettlments).ToArray();

        var oldCharacter = new Character();
        oldCharacter.Coins = initialCoinsCount;

        foreach (var settlment in ownedSettlments)
        {
            oldCharacter.AddSettlment(settlment);

            int stationedUnitsCount = Random.Range(2, 5);
            for (int i = 0; i < stationedUnitsCount; i++)
            {
                oldCharacter.Recruit(TestsUtils.CreateRandomUnit(), settlment.Id);
            } 
        }

        foreach (var settlmentToAttack in settlmentsToAttack)
        {
            int attackingUnitsCount = Random.Range(1, 4);
            for (int i = 0; i < attackingUnitsCount; i++)
            {
                oldCharacter.AddMarchingUnit(
                    TestsUtils.CreateRandomUnit(),
                    settlmentToAttack,
                    Random.Range(1, 5));
            }
        }

        var data = oldCharacter.GetData();
        var newCharacter = new Character(id => Array.Find(allSettlments, s => s.Id == id), data);

        Assert.AreEqual(oldCharacter.Id, newCharacter.Id);
        Assert.AreEqual(oldCharacter.Coins, newCharacter.Coins);

        foreach (var settlmentToAttack in settlmentsToAttack)
        {
            Assert.AreEqual(
                oldCharacter.GetMarchingUnits(settlmentToAttack),
                newCharacter.GetMarchingUnits(settlmentToAttack));
        }

        foreach (var ownedSettlment in ownedSettlments)
        {
            Assert.AreEqual(
                oldCharacter.GetSettlmentArmy(ownedSettlment).GetUnits(),
                newCharacter.GetSettlmentArmy(ownedSettlment).GetUnits());
        }
    }

    private Settlment[] CreateSettlments(int settlmentsCount)
    {
        var settlments = new Settlment[settlmentsCount];

        for (int i = 0; i < settlmentsCount; i++)
        {
            var position = Vector2Int.one * i;
            if (Random.value < 0.5f)
            {
                settlments[i] = new Village(position);
            }
            else
            {
                settlments[i] = new Castle(position);
            }
        }

        return settlments;
    }
}

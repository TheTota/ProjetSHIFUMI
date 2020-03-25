﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThrowback : AI
{
    private bool hasDoneFirstPick;
    private bool hasDoneSecondPick;

    /// <summary>
    /// Inits the AI.
    /// </summary>
    /// <param name="bm"></param>
    public AIThrowback(BattleManager bm)
    {
        base.battleManager = bm;
        base.SecondsBeforeAction = 10f;
        this.hasDoneFirstPick = false;
        this.hasDoneSecondPick = false;
    }

    /// <summary>
    /// Picks a first random unit. Then, picks a unit that counters the player's previously picked unit (2 possibilities).
    /// </summary>
    /// <returns></returns>
    public override UnitType PickUnit()
    {
        UnitType pickedUnit;

        // pick first random unit
        if (!hasDoneFirstPick)
        {
            pickedUnit = base.battleManager.EnemyBC.Army.GetRandomAvailableUnit();
            this.hasDoneFirstPick = true;
        }
        else if (!hasDoneSecondPick)
        {
            pickedUnit = base.battleManager.EnemyBC.Army.GetRandomAvailableUnit();
            this.hasDoneSecondPick = true;
        }
        else
        {
            // get last picked unit by the player, and pick a counter
            UnitType playerLastPickedUnit = base.battleManager.PlayerBC.PlaysHistory[base.battleManager.CurrentRound - 3];
            List<UnitType> availableCounters = base.battleManager.EnemyBC.Army.GetAvailableCounters(playerLastPickedUnit);
            if (availableCounters.Count != 0) // if we have available counters, get a random one of em
            {
                pickedUnit = availableCounters[Random.Range(0, availableCounters.Count - 1)];
            }
            else // if out of stock for counters, pick a random available unit
            {
                pickedUnit = base.battleManager.EnemyBC.Army.GetRandomAvailableUnit();
            }
        }

        return pickedUnit;
    }
}

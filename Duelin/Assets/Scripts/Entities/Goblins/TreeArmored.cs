using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreeArmored : Tank, IPassiveAbility
{
    public override int Cost() => 1;

    new public void HandlePassive()
    {
        base.HandlePassive();
        if (Health < InitialHealth) AddHealth(1);
    }
}

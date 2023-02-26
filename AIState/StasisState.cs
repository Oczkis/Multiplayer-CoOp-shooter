using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisState : State
{
    public override State Tick(Enemy enemy)
    {
        return this;
    }
}

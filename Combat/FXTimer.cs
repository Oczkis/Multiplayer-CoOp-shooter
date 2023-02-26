using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXTimer : MonoBehaviour
{
    public int durationInSecond;
    private int durationInTicks;
    private int ticksPassed;

    public void OnEnable()
    {
        durationInTicks = durationInSecond * GlobalTimer.Instance.ticksPerSecond;
        GlobalTimer.clockTicks += HandleClockTick;
    }

    public void OnDisable()
    {
        GlobalTimer.clockTicks -= HandleClockTick;
    }

    private void HandleClockTick()
    {
        ticksPassed++;

        if (ticksPassed >= durationInTicks)
            Destroy(gameObject);
    }
}

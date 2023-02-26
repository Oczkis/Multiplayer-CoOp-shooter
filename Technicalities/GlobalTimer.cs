using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class GlobalTimer : NetworkBehaviour
{
    private static GlobalTimer _instance;

    public static GlobalTimer Instance { get { return _instance; } }

    public int ticksPerSecond;

    public static Action clockTicks;

    [SyncVar]
    private bool clockRunning;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public override void OnStartServer()
    {
        StartClock();
    }

    public void Start()
    {
        InvokeRepeating("Timer", 1f, 1 / (float)ticksPerSecond);
    }

    public void StartClock()
    {
        clockRunning = true;
    }

    public void StopClock()
    {
        clockRunning = false;
    }

    private void Timer()
    {
        if(!clockRunning) { return; }
        clockTicks?.Invoke();
    }
}

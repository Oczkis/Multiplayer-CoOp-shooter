using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    Enemy enemy;
    NavMeshAgent guideOrbAgent;

    public State[] allStates = new State[0];
    public State currentState;

    public EnemyGuideOrb guideOrb;
    

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void Tick()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(enemy);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    public void GoStasisState()
    {
        SwitchToNextState(allStates[0]);
    }

    public void GoIdleState()
    {
        SwitchToNextState(allStates[1]);
    }
}

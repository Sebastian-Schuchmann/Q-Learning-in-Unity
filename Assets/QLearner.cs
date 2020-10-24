using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class QLearner : MonoBehaviour
{
    public AgentMover AgentMover;

    [Header("Hyperparameters")] [Range(0.05f, 100f)]
    public float TimeScale = 100f;

    public float ExplorationFactor = 1f;
    public float LearningRate = 1f;
    public float DiscountFactor = 0.9f;
    public float MaxStepCount = 20000;

    private Dictionary<State, float[]> QTable;
    private State currentState;

    public event Action OnStep;
    public event Action OnEpisodeComplete;


    private void Start()
    {
        Time.timeScale = TimeScale;
        InitializeQTable();
        currentState = AgentMover.GetState();
    }

    public void InitializeQTable()
    {
        QTable = new Dictionary<State, float[]>();

        int size = ArenaScaler.instance.Size;

        for (int x = -size; x <= size; x++)
        {
            for (int y = -size; y <= size; y++)
            {
                for (int xT = -size; xT <= size; xT++)
                {
                    for (int yT = -size; yT <= size; yT++)
                    {
                        var state = new State(x, y, xT, yT);
                        float[] qValues = {0, 0, 0, 0}; //4 Values, 1 for each Action (Up,Down,Left,Right)
                        QTable.Add(state, qValues);
                    }
                }
            }
        }

        Debug.Log("Create Q-Table of size " + QTable.Count);
    }

    private void FixedUpdate()
    {
        OnStep?.Invoke();
        Time.timeScale = TimeScale;

        DecayHyperparameters();

        int action;

        if (Random.Range(0f, 1f) < ExplorationFactor)
        {
            action = Random.Range(0, 4);
        }
        else
        {
            action = QTable[currentState].ToList().IndexOf(QTable[currentState].Max());
        }

        var envResult = AgentMover.Move(action);
        if (envResult.reward == 1) OnEpisodeComplete?.Invoke();


        float oldQValue = QTable[currentState][action];
        
        var nextMax = QTable[envResult.state].Max();

        var newQValue = oldQValue + LearningRate * (envResult.reward + DiscountFactor * nextMax - oldQValue);
        QTable[currentState][action] = newQValue;

        currentState = envResult.state;
    }

    private void DecayHyperparameters()
    {
        if (ExplorationFactor > 0.001f) ExplorationFactor -= 1 / MaxStepCount;
        if (LearningRate > 0.1f) LearningRate -= 1 / MaxStepCount;
        if (DiscountFactor > 0.1f) DiscountFactor -= 1 / MaxStepCount;
    }
}
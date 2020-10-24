using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatisticsDisplay : MonoBehaviour
{
    private QLearner _qLearner;
    
    [Header("Settings")]
    public int SnapShotSize = 10;
   
    [Header("Output, Don't Change Values")]
    public int StepCount;
    public int EpisodeCount;
    public float AvgStepsPerEpisode;
    public float AvgStepsPerEpisodeSnapshot;
    
    private int stepLengthLastEpisode;
    public Queue<float> EpisodeHistory = new Queue<float>();
    public List<float> EpisodeHistoryDisplay = new List<float>();

    private void Start()
    {
        _qLearner = GetComponent<QLearner>();
        _qLearner.OnStep += Step;
        _qLearner.OnEpisodeComplete += EpisodeComplete;
    }

    public void Step()
    {
        StepCount++;
    }

    public void EpisodeComplete()
    {
        EpisodeCount++;
        

        AvgStepsPerEpisode = (float)StepCount / EpisodeCount;
        
        EpisodeHistory.Enqueue(StepCount - stepLengthLastEpisode);
        AvgStepsPerEpisodeSnapshot = 0f;
        
        foreach (var f in EpisodeHistory)
        {
            AvgStepsPerEpisodeSnapshot += f;
        }

        AvgStepsPerEpisodeSnapshot /= EpisodeHistory.Count;
        if (EpisodeHistory.Count > SnapShotSize) EpisodeHistory.Dequeue();
        EpisodeHistoryDisplay = EpisodeHistory.ToList();
        
        stepLengthLastEpisode = StepCount;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public class RacerPositionData
    {
        public int CurrentLap;
        public int CurrentCheckpoint;
        public float DistanceToNext;

        public RacerPositionData(int currentLap = 0, int currentCheckpoint = -1, float distanceToNext = 0f)
        {
            CurrentLap = currentLap;
            CurrentCheckpoint = currentCheckpoint;
            DistanceToNext = distanceToNext;
        }
    }
    
    public class RacerFinishData
    {
        public float Time;
    
        public override string ToString()
        {
            return Time.ToString("F2");
        }
    }
    
    public class Racer : IComparable<Racer>
    {
        public AbstractDriver Driver { get; private set; }
        public RacerPositionData PositionData { get; private set; }
        public RacerFinishData FinishData { get; private set; }
    
        public event Action onStart;
        public event Action onCheckpoint;
        public event Action onLap;
        public event Action onFinish;
        
        public Racer(AbstractDriver driver)
        {
            Driver = driver;
            PositionData = new RacerPositionData();
        }
    
        public bool HasFinished()
        {
            return FinishData != null;
        }
    
        public void Start()
        {
            onStart?.Invoke();
        }
        
        public void Checkpoint()
        {
            onCheckpoint?.Invoke();
        }

        public void Lap()
        {
            PositionData.CurrentLap++;
            onLap?.Invoke();
        }
    
        public void Finish(RacerFinishData finishData)
        {
            FinishData = finishData;
            onFinish?.Invoke();
        }
        
        public int CompareTo(Racer other)
        {
            var compareLap = PositionData.CurrentLap.CompareTo(other.PositionData.CurrentLap);
            if (compareLap != 0)
                return -compareLap;
            
            var compareCheckpoint = PositionData.CurrentCheckpoint.CompareTo(other.PositionData.CurrentCheckpoint);
            if (compareCheckpoint != 0)
                return -compareCheckpoint;
            
            var compareDistance = PositionData.DistanceToNext.CompareTo(other.PositionData.DistanceToNext);
            if (compareDistance != 0)
                return -compareDistance;
    
            //equals
            return 0;
        }
    }
}
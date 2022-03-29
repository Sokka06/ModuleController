using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public struct CheckpointData
    {
        public int Index;
        public Vector2 Position;

        public CheckpointData(Vector2 position, int index = -1)
        {
            Index = index;
            Position = position;
        }
    }
    
    public class RacerPositionData
    {
        public Vector2 Position;
        public int CurrentLap;
        public CheckpointData Checkpoint;
        public int CurrentCheckpoint;
        public float DistanceToNext;

        public RacerPositionData(Vector2 position, CheckpointData checkpoint, int currentLap = 0, int currentCheckpoint = -1, float distanceToNext = 0f)
        {
            Position = position;
            CurrentLap = currentLap;
            CurrentCheckpoint = currentCheckpoint;
            DistanceToNext = distanceToNext;
            Checkpoint = checkpoint;
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
            PositionData = new RacerPositionData(new Vector2(driver.Vehicle.transform.position.x, driver.Vehicle.transform.position.z), new CheckpointData());
        }
    
        public bool HasFinished()
        {
            return FinishData != null;
        }
    
        public void Start()
        {
            onStart?.Invoke();
        }

        public void Checkpoint(CheckpointData checkpointData)
        {
            PositionData.Checkpoint = checkpointData;
            PositionData.CurrentCheckpoint = checkpointData.Index;
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
            
            var compareDistance = -PositionData.DistanceToNext.CompareTo(other.PositionData.DistanceToNext);
            if (compareDistance != 0)
                return -compareDistance;
    
            //equals
            return 0;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    
    public enum RacerState
    {
        Initial,
        Started,
        Finished
    }
    
    public class RacerCheckpointData
    {
        public int Index;
        public float Distance;

        public RacerCheckpointData(int index, float distance)
        {
            Index = index;
            Distance = distance;
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

    public class RacerLapData
    {
        public int Lap;
        public float Time;
        public readonly List<float> Times;

        public RacerLapData()
        {
            Lap = 0;
            Times = new List<float>();
        }
    }

    public class Racer : IComparable<Racer>
    {
        public readonly Race Race;
        public readonly AbstractDriver Driver;
        
        public RacerCheckpointData CheckpointData { get; private set; }
        public RacerLapData LapData { get; private set; }
        public RacerFinishData FinishData { get; private set; }
        public RacerState State { get; private set; }
    
        public event Action onStart;
        public event Action onCheckpoint;
        public event Action onLap;
        public event Action onFinish;
        public event Action<(RacerState prevState, RacerState newState)> onStateChanged;
        
        public Racer(Race race, AbstractDriver driver)
        {
            Race = race;
            Driver = driver;
            
            Reset();
        }

        /// <summary>
        /// Starts race for racer.
        /// </summary>
        public void Start()
        {
            SetState(RacerState.Started);
            onStart?.Invoke();
        }
        
        /// <summary>
        /// Sets current lap time.
        /// </summary>
        /// <param name="time"></param>
        public void SetCurrentLapTime(float time)
        {
            LapData.Time = time;
        }
        
        /// <summary>
        /// Returns current lap time.
        /// </summary>
        /// <returns></returns>
        public float GetCurrentLapTime()
        {
            return LapData.Time;
        }

        /// <summary>
        /// Returns best lap time from lap times.
        /// </summary>
        /// <returns></returns>
        public float GetBestLapTime()
        {
            if (!(LapData.Times.Count > 0))
                return 0f;
            
            var bestTime = float.MaxValue;
            for (int i = 0; i < LapData.Times.Count; i++)
            {
                if (LapData.Times[i] < bestTime)
                    bestTime = LapData.Times[i];
            }

            return bestTime;
        }

        /// <summary>
        /// Calculates total time from lap times.
        /// </summary>
        /// <returns></returns>
        public float GetTotalLapTime()
        {
            var time = LapData.Time;
            for (int i = 0; i < LapData.Times.Count; i++)
            {
                time += LapData.Times[i];
            }

            return time;
        }

        /// <summary>
        /// Sets checkpoint index and distance.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="distance"></param>
        public void SetCheckpoint(int index, float distance)
        {
            CheckpointData.Index = index;
            CheckpointData.Distance = distance;
            onCheckpoint?.Invoke();
        }

        /// <summary>
        /// Sets distance to current checkpoint.
        /// </summary>
        /// <param name="distance"></param>
        public void SetDistance(float distance)
        {
            CheckpointData.Distance = distance;
        }

        /// <summary>
        /// Adds new lap.
        /// </summary>
        public void AddLap()
        {
            LapData.Lap++;
            LapData.Times.Add(LapData.Time);
            LapData.Time = 0f;
            onLap?.Invoke();
        }
        
        /// <summary>
        /// Finishes race for racer.
        /// </summary>
        /// <param name="time"></param>
        public void Finish(float time)
        {
            FinishData = new RacerFinishData{Time = time};
            SetState(RacerState.Finished);
            onFinish?.Invoke();
        }

        public void Reset()
        {
            CheckpointData = new RacerCheckpointData(0, 0f);
            LapData = new RacerLapData();
            FinishData = new RacerFinishData();
            State = RacerState.Initial;
        }

        public int CompareTo(Racer other)
        {
            int result;
            
            if (State == RacerState.Finished && other.State == RacerState.Finished)
            {
                // Both have finished, compare times
                result = FinishData.Time.CompareTo(other.FinishData.Time);
                if (result != 0)
                    return result;
            }
            else
            {
                // Other has finished, compare states
                result = -State.CompareTo(other.State);
                if (result != 0)
                    return result;
            }
            
            // Compare laps
            result = -LapData.Lap.CompareTo(other.LapData.Lap);
            if (result != 0)
                return result;
            
            // Compare current checkpoint
            result = -CheckpointData.Index.CompareTo(other.CheckpointData.Index);
            if (result != 0)
                return result;
            
            // Compare distance to next checkpoint
            result = CheckpointData.Distance.CompareTo(other.CheckpointData.Distance);
            if (result != 0)
                return result;
            
            return 0;
        }
        
        private void SetState(RacerState state)
        {
            var prevState = State;
            State = state;
            onStateChanged?.Invoke((prevState, state));
        }
    }
}
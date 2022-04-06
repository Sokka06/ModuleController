using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public struct RacerCheckpointData
    {
        public int Index;
        public Vector2 Position;

        public RacerCheckpointData(int index, Vector2 position)
        {
            Index = index;
            Position = position;
        }
    }
    
    public class RacerPositionData
    {
        public Vector2 Position;
        public Vector2 Target;
        public float DistanceToNext;

        public RacerPositionData()
        {
            Position = Vector2.zero;
            Target = Vector2.zero;
            DistanceToNext = 0f;
        }

        public void UpdatePosition(Vector2 position, Vector2 target)
        {
            Position = position;
            Target = target;
            DistanceToNext = Vector2.Distance(Position, Target);
        }
    }
    
    public struct RacerFinishData
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
        public readonly List<float> Times;

        public RacerLapData()
        {
            Lap = 0;
            Times = new List<float> { 0f };
        }

        public void AddLap()
        {
            Times.Add(0f);
            Lap++;
        }
    }

    public enum RacerState
    {
        Initial,
        Started,
        Finished
    }
    
    public class Racer : IComparable<Racer>
    {
        public Race Race { get; private set; }
        public AbstractDriver Driver { get; private set; }
        public RacerPositionData PositionData { get; private set; }
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
            State = RacerState.Initial;
            CheckpointData = new RacerCheckpointData();
            PositionData = new RacerPositionData();
            LapData = new RacerLapData();
        }

        public void Start()
        {
            CheckpointData = new RacerCheckpointData(0,
                new Vector2(Race.Checkpoints[0].transform.position.x, Race.Checkpoints[0].transform.position.z));
            SetState(RacerState.Started);
            onStart?.Invoke();
        }

        public void Update(float deltaTime)
        {
            LapData.Times[LapData.Lap] += deltaTime;
            PositionData.UpdatePosition(new Vector2(Driver.Vehicle.Controller.Transform.position.x, Driver.Vehicle.Controller.Transform.position.z), CheckpointData.Position);
        }

        public void Checkpoint()
        {
            var nextCheckpointIndex = CheckpointData.Index + 1;
            nextCheckpointIndex = nextCheckpointIndex % Race.Checkpoints.Count;
            CheckpointData = new RacerCheckpointData(nextCheckpointIndex, new Vector2(Race.Checkpoints[nextCheckpointIndex].transform.position.x, Race.Checkpoints[nextCheckpointIndex].transform.position.z));
            onCheckpoint?.Invoke();
        }

        public void Lap()
        {
            LapData.AddLap();
            onLap?.Invoke();
        }
    
        public void Finish()
        {
            FinishData = new RacerFinishData{Time = Race.Data.ElapsedTime};
            SetState(RacerState.Finished);
            onFinish?.Invoke();
        }
        
        public int CompareTo(Racer other)
        {
            var result = 0;
            
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
            // TODO: Use race line to calculate more accurate distance
            result = PositionData.DistanceToNext.CompareTo(other.PositionData.DistanceToNext);
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
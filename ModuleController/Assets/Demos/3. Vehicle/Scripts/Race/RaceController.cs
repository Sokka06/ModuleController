using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Demos.Vehicle
{
    public class RaceController : MonoBehaviour
    {
        public CheckpointManager CheckpointManager;
        public DriverManager DriverManager;
        public RaceSettings Settings;

        public Race CurrentRace { get; private set; }

        public event Action<Race> onStartRace;
        public event Action<Race> onFinishRace;
        public event Action<Race> onResetRace;

        private void FixedUpdate()
        {
            CurrentRace?.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (CurrentRace != null)
                FinishRace();
        }

        /// <summary>
        /// Makes a new race
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="drivers"></param>
        /// <returns></returns>
        public void CreateRace(RaceSettings settings, List<(AbstractDriver, Vehicle)> drivers)
        {
            //initialize race
            var race = new Race (settings);

            //create racers from drivers
            for (int i = 0; i < drivers.Count; i++)
            {
                race.AddRacer(drivers[i].Item1);
            }

            CurrentRace = race;
        }

        public void StartRace()
        {
            CreateRace(Settings, DriverManager.CurrentDrivers);
            
            for (int i = 0; i < CheckpointManager.Checkpoints.Count; i++)
            {
                CheckpointManager.Checkpoints[i].onCheckpoint += OnCheckpoint;
            }

            CurrentRace.Start();
            
            onStartRace?.Invoke(CurrentRace);
        }

        public void FinishRace()
        {
            for (int i = 0; i < CheckpointManager.Checkpoints.Count; i++)
            {
                CheckpointManager.Checkpoints[i].onCheckpoint -= OnCheckpoint;
            }
            
            CurrentRace.Finish();
            onFinishRace?.Invoke(CurrentRace);
        }

        public void ResetRace()
        {
            CurrentRace.Reset();
            onResetRace?.Invoke(CurrentRace);
        }
        
        private void OnStart(Racer racer)
        {
            
        }

        /// <summary>
        /// Called when a driver hits a checkpoint.
        /// </summary>
        /// <param name="vehicle"></param>
        private void OnCheckpoint(Checkpoint checkpoint, Vehicle vehicle)
        {
            var racer = CurrentRace.GetRacer(vehicle.Driver);
            
            if (racer.HasFinished())
                return;
            
            CheckpointManager.GetIndex(checkpoint, out var checkpointIndex);

            //on checkpoint
            if (checkpointIndex != CheckpointManager.GetNextIndex(racer.PositionData.CurrentCheckpoint))
                return;
            racer.PositionData.CurrentCheckpoint = checkpointIndex;
            Debug.Log($"{racer.Driver.Name} on Checkpoint {checkpointIndex}, Time: {CurrentRace.Data.ElapsedTime}");
            
            //on lap
            if (!CheckpointManager.IsLast(checkpointIndex))
                return;
            racer.PositionData.CurrentLap++;
            Debug.Log($"{racer.Driver.Name} on Lap {racer.PositionData.CurrentLap}, Time: {CurrentRace.Data.ElapsedTime}");
            
            //on Finish
            if (racer.PositionData.CurrentLap < CurrentRace.Settings.Laps)
                return;
            racer.Finish(new RacerFinishData{Time = CurrentRace.Data.ElapsedTime});
            Debug.Log($"{racer.Driver.Name} on Finish {racer.FinishData}s.");
        }

        private void OnLap(Racer racer)
        {
            
        }
        
        private void OnFinish(Racer racer)
        {
            
        }
    }
}
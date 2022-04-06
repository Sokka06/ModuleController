using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Demos.Vehicle
{
    [Serializable]
    public struct DriverConfig
    {
        public AbstractDriver Driver;
        public Vehicle Vehicle;
    }
    
    public class RaceController : MonoBehaviour
    {
        public CheckpointManager CheckpointManager;
        public DriverManager DriverManager;
        public SpawnManager SpawnManager;
        
        [Space]
        public RaceSettings Settings;
        public List<DriverConfig> Drivers;

        public Race CurrentRace { get; private set; }

        public event Action<Race> onStartRace;
        public event Action<Race> onFinishRace;
        public event Action<Race> onResetRace;

        private void Awake()
        {
            for (int i = 0; i < Drivers.Count; i++)
            {
                SpawnManager.GetNext().GetSpawn(out var position, out var rotation);
                
                var driver = Instantiate(Drivers[i].Driver, transform);
                var vehicle = Instantiate(Drivers[i].Vehicle, position, rotation);
                driver.Setup(vehicle);
                
                DriverManager.RegisterDriver((driver, vehicle));
            }
            
            CreateRace(Settings, DriverManager.CurrentDrivers);
        }

        private void Update()
        {
            if (!(CurrentRace is { State: RaceState.Started }))
                return;

            var deltaTime = Time.deltaTime;

            // Update racers
            for (int i = 0; i < CurrentRace.Racers.Count; i++)
            {
                var racer = CurrentRace.Racers[i];
                racer.SetCurrentLapTime(racer.GetCurrentLapTime() + deltaTime);
                racer.SetDistance(GetDistanceToCheckpoint(racer, CheckpointManager.Checkpoints[racer.CheckpointData.Index]));
            }
            
            // Update race
            CurrentRace.SetRaceTime(CurrentRace.GetRaceTime() + deltaTime);
            CurrentRace.UpdateStandings();
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
            var race = new Race(settings);

            //create racers from drivers
            for (int i = 0; i < drivers.Count; i++)
            {
                var racer = race.AddRacer(drivers[i].Item1);
                racer.SetDistance(GetDistanceToCheckpoint(racer, CheckpointManager.Checkpoints[0]));
            }

            CurrentRace = race;
        }

        public void StartRace()
        {
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

        /// <summary>
        /// Called when a driver hits a checkpoint.
        /// </summary>
        /// <param name="vehicle"></param>
        private void OnCheckpoint(Checkpoint checkpoint, Vehicle vehicle)
        {
            var racer = CurrentRace.GetRacer(vehicle.Driver);
            
            // Ignore if racer has already finished.
            if (racer.State == RacerState.Finished)
                return;
            
            CheckpointManager.GetIndex(checkpoint, out var checkpointIndex);

            //on checkpoint
            if (checkpointIndex != racer.CheckpointData.Index)
                return;
            
            racer.SetCheckpoint(CheckpointManager.GetNextIndex(checkpointIndex), GetDistanceToCheckpoint(racer, checkpoint));
            Debug.Log($"{racer.Driver.Name} on Checkpoint {checkpointIndex}, Time: {CurrentRace.Data.Time}");
            
            //on lap
            if (!CheckpointManager.IsLast(checkpointIndex))
                return;
            
            racer.AddLap();
            Debug.Log($"{racer.Driver.Name} on Lap {racer.LapData.Lap}, Time: {CurrentRace.Data.Time}");
            
            //on Finish
            if (racer.LapData.Lap < CurrentRace.Settings.Laps)
                return;
            
            racer.Finish(CurrentRace.Data.Time);
            Debug.Log($"{racer.Driver.Name} on Finish {racer.FinishData}s.");
        }

        public float GetDistanceToCheckpoint(Racer racer, Checkpoint checkpoint)
        {
            // TODO: Use race line to calculate a more accurate distance.
            var distance = Vector2.Distance(new Vector2(racer.Driver.Vehicle.Controller.Transform.position.x,racer.Driver.Vehicle.Controller.Transform.position.z),
                new Vector2(checkpoint.transform.position.x, checkpoint.transform.position.z));
            return distance;
        }
    }
}
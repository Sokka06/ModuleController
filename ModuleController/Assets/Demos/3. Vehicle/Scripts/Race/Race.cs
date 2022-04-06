using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    public enum RaceState
    {
        Initial,
        Started,
        Finished
    }
    
    [Serializable]
    public struct RaceSettings
    {
        public int Laps;

        public RaceSettings(int laps = 2)
        {
            Laps = laps;
        }
    }

    public class RaceData
    {
        public float ElapsedTime = 0f;
        public RaceState State = RaceState.Initial;
    }

    public class Race
    {
        public readonly RaceSettings Settings;
        public readonly List<Racer> Racers;
        public readonly List<Checkpoint> Checkpoints;
        public readonly RaceStandings Standings;
        
        public RaceData Data { get; private set; }

        public event Action<(RaceState prevState, RaceState newState)> onStateChanged; 

        public Race(RaceSettings settings)
        {
            Settings = settings;
            
            Racers = new List<Racer>();
            Checkpoints = new List<Checkpoint>();
            Standings = new RaceStandings();
            Data = new RaceData();
        }
        
        /// <summary>
        /// Adds driver to race.
        /// </summary>
        /// <param name="driver"></param>
        public Racer AddRacer(AbstractDriver driver)
        {
            var racer = new Racer(this, driver);
            Racers.Add(racer);
            Standings.Add(racer);
            
            return racer;
        }

        /// <summary>
        /// Starts race.
        /// </summary>
        public void Start()
        {
            for (int i = 0; i < Racers.Count; i++)
            {
                Racers[i].Start();
            }
            SetState(RaceState.Started);
        }

        /// <summary>
        /// Updates race.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (Data.State != RaceState.Started)
                return;
                
            Data.ElapsedTime += deltaTime;

            for (int i = 0; i < Racers.Count; i++)
            {
                Racers[i].Update(deltaTime);
            }
            
            Standings.Sort();
        }

        /// <summary>
        /// Finish race.
        /// </summary>
        public void Finish()
        {
            SetState(RaceState.Finished);
        }

        public void Reset()
        {
            Data = new RaceData();
        }
        
        /// <summary>
        /// Gets racer for given driver
        /// </summary>
        /// <returns></returns>
        public Racer GetRacer(AbstractDriver driver)
        {
            for (int i = 0; i < Racers.Count; i++)
            {
                if (Racers[i].Driver == driver)
                    return Racers[i];
            }
            
            Debug.LogWarning("Racer for Driver not found!");
            return null;
        }

        private void SetState(RaceState state)
        {
            var prevState = Data.State;
            Data.State = state;
            onStateChanged?.Invoke((prevState, state));
        }
    }
}
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
        public float Time;
    }

    public class Race
    {
        public readonly RaceSettings Settings;
        public readonly List<Racer> Racers;
        public readonly RaceStandings Standings;
        
        public RaceData Data { get; private set; }
        public RaceState State { get; private set; }

        public event Action onStart;
        public event Action onFinish;
        public event Action<(RaceState prevState, RaceState newState)> onStateChanged; 

        public Race(RaceSettings settings)
        {
            Settings = settings;
            State = RaceState.Initial;
            
            Racers = new List<Racer>();
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
            onStart?.Invoke();
        }

        /// <summary>
        /// Sets race total time.
        /// </summary>
        /// <param name="time"></param>
        public void SetRaceTime(float time)
        {
            Data.Time = time;
        }

        /// <summary>
        /// Returns race total time.
        /// </summary>
        /// <returns></returns>
        public float GetRaceTime()
        {
            return Data.Time;
        }

        /// <summary>
        /// Sorts racer standings.
        /// </summary>
        public void UpdateStandings()
        {
            Standings.Sort();
        }

        /// <summary>
        /// Finish race.
        /// </summary>
        public void Finish()
        {
            SetState(RaceState.Finished);
            onFinish?.Invoke();
        }

        public void Reset()
        {
            Data = new RaceData();
            for (int i = 0; i < Racers.Count; i++)
            {
                Racers[i].Reset();
            }
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
            var prevState = State;
            State = state;
            onStateChanged?.Invoke((prevState, state));
        }
    }
}
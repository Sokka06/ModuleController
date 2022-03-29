using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    /*public enum RacerState
    {
        Inactive,
        Active,
        DNF
    }

    public enum RaceState
    {
        Initial,
        Countdown,
        Started,
        Finished
    }*/

    [Serializable]
    public class RaceSettings
    {
        public int Laps = 1;
    }

    public class RaceData
    {
        public bool IsStarted = false;
        public bool IsFinished = false;
        public float ElapsedTime = 0f;
    }

    public class Race
    {
        public RaceSettings Settings;
        public List<Racer> Racers;
        public RaceStandings Standings;
        
        public RaceData Data { get; private set; }

        public Race(RaceSettings settings)
        {
            Settings = settings;
            Racers = new List<Racer>();
            Standings = new RaceStandings();
            
            Data = new RaceData();
        }

        public void AddRacer(AbstractDriver driver)
        {
            var racer = new Racer(driver);
            Racers.Add(racer);
            Standings.Add(racer);
        }

        public void Start()
        {
            Data.IsStarted = true;
        }

        public void Update(float deltaTime)
        {
            if (Data.IsStarted && !Data.IsFinished)
                Data.ElapsedTime += deltaTime;

            for (int i = 0; i < Racers.Count; i++)
            {
                var racer = Racers[i];
                var distance = Vector2.Distance(racer.PositionData.Checkpoint.Position, racer.PositionData.Position);
                racer.PositionData.DistanceToNext = distance;
            }
            
            Standings.Sort();
        }

        public void Finish()
        {
            Data.IsFinished = true;
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
    }
}
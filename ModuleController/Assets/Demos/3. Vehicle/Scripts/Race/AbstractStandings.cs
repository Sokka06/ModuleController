using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Vehicle
{
    /// <summary>
    /// Base class for handling standings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractStandings<T>
    {
        public List<T> Standings { get; protected set; }
    
        public event Action onChanged;
    
        protected AbstractStandings()
        {
            Standings = new List<T>();
        }
    
        /// <summary>
        /// Add item to standings.
        /// </summary>
        /// <param name="standing"></param>
        public virtual void Add(T standing)
        {
            Standings.Add(standing);
            onChanged?.Invoke();
        }
        
        /// <summary>
        /// Remove item from standings.
        /// </summary>
        /// <param name="standing"></param>
        public virtual void Remove(T standing)
        {
            Standings.Remove(standing);
            onChanged?.Invoke();
        }
        
        /// <summary>
        /// Updates standings order.
        /// </summary>
        public virtual void Sort()
        {
            Standings.Sort();
            onChanged?.Invoke();
        }
    
        /// <summary>
        /// Returns standing for given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual int GetStanding(T item)
        {
            return Standings.IndexOf(item) + 1;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class RaceStandings : AbstractStandings<Racer>
    {
        
    }
}

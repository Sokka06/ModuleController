using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatorCallbacks
{
    void AnimatorMove(float deltaTime);
    void AnimatorIK(float deltaTime, int layerIndex);
}

/// <summary>
/// 
/// </summary>
public class AnimatorListener : MonoBehaviour
{
    public Animator Animator;

    public List<IAnimatorCallbacks> Callbacks { get; private set; } = new List<IAnimatorCallbacks>();

    private void OnValidate()
    {
        if (Animator == null)
            Animator = GetComponent<Animator>();
    }

    public void Register(IAnimatorCallbacks callbacks)
    {
        Callbacks.Add(callbacks);
    }
    
    public void Unregister(IAnimatorCallbacks callbacks)
    {
        Callbacks.Remove(callbacks);
    }
    
    private void OnAnimatorMove()
    {
        for (int i = 0; i < Callbacks.Count; i++)
        {
            Callbacks[i].AnimatorMove(Time.deltaTime);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        for (int i = 0; i < Callbacks.Count; i++)
        {
            Callbacks[i].AnimatorIK(Time.deltaTime, layerIndex);
        }
    }
}

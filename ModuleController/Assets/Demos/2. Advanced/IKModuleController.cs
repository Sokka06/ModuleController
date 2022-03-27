using System;
using System.Collections;
using System.Collections.Generic;
using Demos;
using Sokka06.ModuleController;
using UnityEngine;

public class IKModuleController : ModuleControllerBehaviour<IKModuleController, AbstractIKModule>, IAnimatorCallbacks
{
    public CharacterModuleController Character;
    public AnimatorListener Listener;

    private void Start()
    {
        SetupModules();
        Listener.Register(this);
    }

    private void OnDestroy()
    {
        Listener.Unregister(this);
    }

    public void AnimatorMove(float deltaTime)
    {
        
    }

    public void AnimatorIK(float deltaTime, int layerIndex)
    {
        UpdateModules(Time.deltaTime);
    }
}

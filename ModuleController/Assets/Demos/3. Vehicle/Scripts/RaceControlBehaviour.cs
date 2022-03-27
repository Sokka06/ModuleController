using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Demos.Vehicle
{
    public class RaceControlBehaviour : PlayableBehaviour
    {
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Light light = playerData as Light;

            if (light != null)
            {
            
            }
        }
    }
}


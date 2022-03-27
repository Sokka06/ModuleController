using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Common
{
    public interface IInteracteable
    {
        void Interact(AbstractInteractor interactor);
    }
}
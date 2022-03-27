using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Common
{
    public abstract class AbstractManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        public List<T> Elements;
    }
}
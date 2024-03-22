using System.Linq;
using UnityEngine.UI;
using UnityEngine;

namespace Weapons.Crosshair
{
    /// Author: L1nkCC
    /// Created: 12/5/2023
    /// Last Edited: 12/5/2023
    /// 
    /// <summary>
    /// Crosshair Display containing all Component options
    /// </summary>
    [System.Serializable]
    public abstract class Display
    {
        public Component Dot;
        public Component Inner;
        public Component Expanding;

        //Extra variables to define Expanding constraints
        public float ShrinkSpeed = 2f;
        public float MaxScale = 2f;
    }

    [System.Serializable]
    public class HipDisplay : Display
    {
        public Component Reload;
    }

    [System.Serializable]
    public class ScopeDisplay : Display
    {
        public Component Scope;
    }
}

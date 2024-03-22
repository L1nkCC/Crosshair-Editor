using UnityEngine;
using UnityEngine.UI;

namespace Weapons.Crosshair
{
    /// Author: L1nkCC
    /// Created: 10/30/2023
    /// Last Edited: 12/8/2023
    /// 
    /// <summary>
    /// Data for Describing a Crosshair
    /// </summary>
    [UnityEngine.CreateAssetMenu(fileName = "Crosshair", menuName = "Weapon/Crosshair")]
    [System.Serializable]
    public class CrosshairData : UnityEngine.ScriptableObject
    {
        public DisplayGroup<HipDisplay> Hip = new();
        public bool CanScope = true;
        public DisplayGroup<ScopeDisplay> Scope = new();
    }
}

using System;

namespace Weapons.Crosshair
{
    /// Author: L1nkCC
    /// Created: 12/5/2023
    /// Last Edited: 12/5/2023
    /// <summary>
    /// Interface for interacting with Crosshair
    /// </summary>
    public interface ICrosshairUpdater
    {
        System.Action<CrosshairData> Swap { get; set; }
        System.Action ToggleScope { get; set; }
        System.Action<float> Reload { get; set; }
        System.Action<float> Expand { get; set; }
        System.Action<Target> UpdateTarget { get; set; }
    }
}

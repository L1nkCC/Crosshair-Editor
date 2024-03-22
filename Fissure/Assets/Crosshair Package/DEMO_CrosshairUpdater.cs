using System;
using UnityEngine;

namespace Weapons.Crosshair
{
    public class DEMO_CrosshairUpdater : MonoBehaviour, ICrosshairUpdater
    {
        [SerializeField]
        private CrosshairData[] Crosshairs;
        private int m_currentIndex = 0;
        public Action<CrosshairData> Swap { get; set; }
        public Action ToggleScope { get; set; }
        public Action<float> Reload { get; set; }
        public Action<float> Expand { get; set; }
        public Action<Target> UpdateTarget { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Input.InputManager.Singleton.Use.performed += (ctx) => {m_currentIndex++; if (m_currentIndex >= Crosshairs.Length) m_currentIndex = 0; Swap(Crosshairs[m_currentIndex]); };
            Input.InputManager.Singleton.ADS.performed += (ctx) => { ToggleScope();};
            Input.InputManager.Singleton.Reload.performed += (ctx) => { Reload(1f); };
            Input.InputManager.Singleton.Fire.performed += (ctx) => { Expand(.2f); };
            Input.InputManager.Singleton.SwitchWeapon1.performed += (ctx) => { UpdateTarget(Target.Standard); };
            Input.InputManager.Singleton.SwitchWeapon2.performed += (ctx) => { UpdateTarget(Target.Enemy); };
            Input.InputManager.Singleton.SwitchWeapon3.performed += (ctx) => { UpdateTarget(Target.Friendly); };
            Swap(Crosshairs[0]);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
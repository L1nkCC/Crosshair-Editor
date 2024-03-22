using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    /// Author: L1nkCC
    /// Created: 10/28/2023
    /// Last Edited: 10/28/2023
    /// 
    /// <summary>
    /// Focus All Input operations through Singleton of PlayerControls
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        PlayerControls playerControls;
        PlayerControls.GameplayActions gameplayActions;
        public static InputManager Singleton { get; private set; }

        #region Easy Access InputActions
        public InputAction Fire => gameplayActions.Fire;
        public InputAction ADS => gameplayActions.ADS;
        public InputAction HorizontalMovement => gameplayActions.HorizontalMovement;
        public InputAction Jump => gameplayActions.Jump;
        public InputAction MouseX => gameplayActions.MouseX;
        public InputAction MouseY => gameplayActions.MouseY;
        public InputAction Use => gameplayActions.Use;
        public InputAction Drop => gameplayActions.Drop;
        public InputAction Sprint => gameplayActions.Sprint;
        public InputAction Crouch => gameplayActions.Crouch;
        public InputAction Ability => gameplayActions.Ability;
        public InputAction Reload => gameplayActions.Reload;
        public InputAction Melee => gameplayActions.Melee;
        public InputAction ScrollWeapon => gameplayActions.ScrollWeapon;
        public InputAction SwitchWeapon1 => gameplayActions.SwitchWeapon1;
        public InputAction SwitchWeapon2 => gameplayActions.SwitchWeapon2;
        public InputAction SwitchWeapon3 => gameplayActions.SwitchWeapon3;
        public InputAction SwitchWeapon4 => gameplayActions.SwitchWeapon4;
        #endregion

        /// <summary>
        /// Set Necessary accessor variables
        /// </summary>
        void Awake()
        {
            SetSingleton();
            playerControls = new PlayerControls();
            gameplayActions = playerControls.Gameplay;
            
        }
        /// <summary>
        /// Set this as the Singleton or Destroy it
        /// </summary>
        private void SetSingleton()
        {
            if (Singleton == null)
                Singleton = this;
            else
            {
                Debug.Log("There are Multiple InputManagers in scene: \tSingleton set: " + Singleton.gameObject.name + " and \tDestroyed: " + this.gameObject.name);
                Destroy(this);
            }
        }
        /// <summary>
        /// Enable playerControls
        /// </summary>
        private void OnEnable()
        {
            playerControls.Enable();
        }
        /// <summary>
        /// Disable PlayerControls
        /// </summary>
        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}

using UnityEngine;
using Input;

namespace Player
{
    /// Author: L1nkCC
    /// Created: 10/28/2023
    /// Last Edited: 11/9/2023
    /// 
    /// <summary>
    /// Add functionality for looking with a camera. Will rotate character for horizontal change and camera for vertical
    /// </summary>
    public class CameraLook : MonoBehaviour, IInputHandler
    {
        [Min(0)] [SerializeField] float MouseXSensitivitySettingsMultiplier = 1f;
        [Min(0)] [SerializeField] float MouseYSensitivitySettingsMultiplier = 1f;
        [field: SerializeField] public Camera c_camera { get; private set; }
        [SerializeField] Vector2 VerticalRotationClamp = new Vector2(-90f, 90f);

        Vector2 m_targetViewRotationChange = Vector3.zero;

        #region Sensitivity
        float MouseXSensitivity => SENSITIVITY_EQUALIZATION_MULTIPLIER * MouseXSensitivitySettingsMultiplier;
        float MouseYSensitivity => SENSITIVITY_EQUALIZATION_MULTIPLIER * MouseYSensitivitySettingsMultiplier;
        const float SENSITIVITY_EQUALIZATION_MULTIPLIER = .05f;
        #endregion
        #region Inputs
        public bool InputsEnabled { get; private set; }
        float MouseX => InputsEnabled ? InputManager.Singleton.MouseX.ReadValue<float>() * MouseXSensitivity : 0f;
        float MouseY => InputsEnabled ? -InputManager.Singleton.MouseY.ReadValue<float>() * MouseYSensitivity : 0f;
        #endregion

        /// <summary>
        /// Set up
        /// </summary>
        private void Awake()
        {
            if (c_camera == false) c_camera = GetComponentInChildren<Camera>();
            (this as IInputHandler).EnableInputs();
        }
        /// <summary>
        /// Apply all necessary operations for looking with a camera
        /// </summary>
        private void Update()
        {
            UpdateTargetViewRotationChange();
            TurnView();
        }

        /// <summary>
        /// Apply recoil to the view
        /// </summary>
        /// <param name="amount">The recoil of the Weapon</param>
        public void Recoil(float amount)
        {
            float recoilXPos = (float)(((Random.value - .5f) / 2f) * amount);
            float recoilYPos = Mathf.Abs((float)(((Random.value - .5f) / 2f) * amount));


            m_targetViewRotationChange.x -= recoilXPos;
            m_targetViewRotationChange.y -= recoilYPos;
            TurnView();
        }

        /// <summary>
        /// Update target
        /// </summary>
        private void UpdateTargetViewRotationChange()
        {
            m_targetViewRotationChange.x += MouseX;
            m_targetViewRotationChange.y += MouseY;
        }
        /// <summary>
        /// Turn View to the target. This will rotate the player character for horizontal turns and rotate the camera for Vertical
        /// </summary>
        private void TurnView()
        {
            m_targetViewRotationChange.y = Mathf.Clamp(m_targetViewRotationChange.y, VerticalRotationClamp.x, VerticalRotationClamp.y);

            Vector3 playerTargetRotation = transform.rotation.eulerAngles;
            playerTargetRotation.y = m_targetViewRotationChange.x;
            transform.eulerAngles = playerTargetRotation;


            Vector3 cameratargetRotation = transform.rotation.eulerAngles;
            cameratargetRotation.x = m_targetViewRotationChange.y;
            c_camera.transform.eulerAngles = cameratargetRotation;
        }

        /// <summary>
        /// Lock Cursor to middle of Screen
        /// </summary>
        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        /// <summary>
        /// Unlock Cursor from middle of Screen
        /// </summary>
        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }
        /// <summary>
        /// Disable Inputs (including unlocking cursor)
        /// </summary>
        void IInputHandler.DisableInputs()
        {
            UnlockCursor();
            InputsEnabled = false;
        }
        /// <summary>
        /// Enable Inputs (including locking cursor)
        /// </summary>
        void IInputHandler.EnableInputs()
        {
            LockCursor();
            InputsEnabled = true;
        }

    }
}

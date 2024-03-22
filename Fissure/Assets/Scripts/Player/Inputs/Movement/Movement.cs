using Input;
using UnityEngine;

namespace Player
{
    /// Author: L1nkCC
    /// Created: 10/28/2023
    /// Last Edited: 10/29/2023
    /// 
    /// <summary>
    /// Handle Movement for a Player
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour, IInputHandler
    {
        private CharacterController c_controller;
        private Vector3 m_movementChange = new();

        #region Horizontal Movement
        [Min(0)] [SerializeField] float m_walkSpeed = 5;
        [Min(0)] [SerializeField] float m_sprintSpeed = 10;
        #endregion

        #region Vertical Movement
        #region GroundCheck
        [SerializeField] Transform c_groundCheck;
        [SerializeField] LayerMask m_groundMask;
        const float GROUND_CHECK_RADIUS = 0.4f;
        bool m_isGrounded = false;
        #endregion

        #region Jump
        [SerializeField] float m_jumpHeight = 3f;//Mathf.Sqrt(JUMP_MULTIPLIER * -2f * GRAVITY_MULTIPLIER);
        #endregion

        #region Gravity
        [SerializeField] float m_gravity = -15f;
        #endregion
        #endregion

        #region Crouch
        float m_standingHeight;
        float m_crouchHeight;
        bool m_wasCrouching = false;
        #endregion

        #region Inputs
        public bool InputsEnabled { get; private set; }
        Vector2 HorizontalMovementInput => InputsEnabled ? InputManager.Singleton.HorizontalMovement.ReadValue<Vector2>() : Vector2.zero;
        bool SprintInput => InputsEnabled ? InputManager.Singleton.Sprint.IsPressed() : false;
        bool JumpInput => InputsEnabled ? InputManager.Singleton.Jump.IsPressed() : false;
        bool CrouchInput => InputsEnabled ? InputManager.Singleton.Crouch.IsPressed() : false;
        bool ADSInput => InputsEnabled ? InputManager.Singleton.ADS.IsPressed() : false;
        #endregion

        #region Status
        public bool IsMoving { get; private set; }
        public bool IsGrounded => m_isGrounded;
        public bool IsSprinting { get; private set; }
        public bool IsCrouching { get; private set; }

        #endregion

        /// <summary>
        /// Get Setup
        /// </summary>
        private void Awake()
        {
            c_controller = GetComponent<CharacterController>();
            InputsEnabled = true;
            m_standingHeight = c_controller.height;
            m_crouchHeight = c_controller.height / 2;
        }

        /// <summary>
        /// Apply all necessary Physics Motions
        /// </summary>
        private void FixedUpdate()
        {
            Move();
        }
        /// <summary>
        /// Find all necessary information for Movement
        /// </summary>
        private void Update()
        {
            UpdateHorizontalChange();
            UpdateGround();
            HandleCrouch();
            HandleJump();
            ApplyGravity();
            UpdateStatus();
        }

        /// <summary>
        /// Update movementChange to have current horizontal movement
        /// </summary>
        private void UpdateHorizontalChange()
        {
            m_movementChange = (transform.right * HorizontalMovementInput.x + transform.forward * HorizontalMovementInput.y + transform.up * m_movementChange.y);
            m_movementChange.x = SprintInput && !ADSInput ? m_movementChange.x * m_sprintSpeed : m_movementChange.x * m_walkSpeed;
            m_movementChange.z = SprintInput && !ADSInput ? m_movementChange.z * m_sprintSpeed : m_movementChange.z * m_walkSpeed;
        }
        /// <summary>
        /// Set isGrounded and Set Vertical change if grounded is true
        /// </summary>
        private void UpdateGround()
        {
            m_isGrounded = Physics.CheckSphere(c_groundCheck.position, GROUND_CHECK_RADIUS, m_groundMask);
            if (m_isGrounded && m_movementChange.y < 0)
            {
                m_movementChange.y = -.5f;
            }
        }

        private void HandleCrouch()
        {
            if (IsCrouching && !m_wasCrouching)
            {
                c_controller.height = m_crouchHeight;
                m_wasCrouching = true;
            }
            if(!IsCrouching && m_wasCrouching)
            {
                c_controller.height = m_standingHeight;
                m_wasCrouching = false;
            }
        }

        /// <summary>
        /// Handle updating movement change On jump action
        /// </summary>
        private void HandleJump()
        {
            if (m_isGrounded && JumpInput)
            {
                m_movementChange.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
            }
        }
        /// <summary>
        /// Apply Gravity to movement change
        /// </summary>
        private void ApplyGravity()
        {
            m_movementChange.y += m_gravity * Time.deltaTime;
        }

        /// <summary>
        /// Apply movement to PlayerController
        /// </summary>
        private void Move()
        { 
            c_controller.Move(Time.deltaTime * m_movementChange);
        }

        /// <summary>
        /// Set the current status of Movement
        /// </summary>
        private void UpdateStatus()
        {
            IsMoving = m_movementChange == Vector3.zero;
            IsSprinting = IsMoving && SprintInput;
            IsCrouching = !IsSprinting && CrouchInput;
        }

        /// <summary>
        /// Disable Inputs
        /// </summary>
        void IInputHandler.DisableInputs()
        {
            InputsEnabled = false;
        }

        /// <summary>
        /// Enable Inputs
        /// </summary>
        void IInputHandler.EnableInputs()
        {
            InputsEnabled = true;
        }
    }
}

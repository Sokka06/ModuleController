using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos
{
    public enum CharacterMoveMode
    {
        Input,
        Forward
    }
    
    /// <summary>
    /// Moves the character in the direction given by Input module.
    /// </summary>
    public class CharacterGroundMoveModule : AbstractCharacterModule
    {
        [Header("Move"), Tooltip("Input = Moves towards input, Forward = Moves towards current forward direction.")] 
        public CharacterMoveMode MoveMode;
        
        [Space]
        public float Speed = 5f;
        public float Smoothness = 15f;

        [Space] 
        public float SprintMultiplier = 1.5f;

        private CharacterInputModule _inputModule;

        public override void SetupModule(CharacterModuleController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.GetModule<CharacterInputModule>();
        }

        public override void UpdateModule(float deltaTime)
        {
            if (!Enabled)
                return;

            GetMoveVector(MoveMode, out var moveVector);
            
            if (Controller.CharacterController.isGrounded)
            {
                var speed = Speed;
                if (_inputModule.Inputs.Sprint)
                    speed *= SprintMultiplier;
                
                // Calculate target velocity
                var moveRight = Vector3.Cross(moveVector, Controller.Transform.up);
                var reorientedMoveVector = Vector3.Cross(Controller.GroundData.Normal, moveRight).normalized;
                var targetVelocity = moveVector * speed * _inputModule.Inputs.Move.magnitude;

                // Smooth movement Velocity
                Controller.SetVelocity(Vector3.Lerp(Controller.Velocity, targetVelocity, Smoothness * deltaTime));
            }
        }
        
        private void GetMoveVector(CharacterMoveMode mode, out Vector3 moveVector)
        {
            moveVector = Vector3.zero;
            switch (mode)
            {
                case CharacterMoveMode.Input:
                    moveVector = new Vector3(_inputModule.Inputs.Move.x, 0f, _inputModule.Inputs.Move.y);
                    break;
                case CharacterMoveMode.Forward:
                    moveVector = Controller.Transform.forward;
                    break;
            }

            moveVector.y = 0f;
            moveVector.Normalize();
        }
    }
}
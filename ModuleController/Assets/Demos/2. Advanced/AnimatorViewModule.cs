using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demos.Demo2
{
    public class AnimatorViewModule : AbstractViewModule
    {
        public Animator Animator;

        private CharacterInputModule _inputModule;
        private CharacterJumpModule _jumpModule;

        private float _freeFallTimer;
        
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");

        public override void SetupModule(CharacterViewController controller)
        {
            base.SetupModule(controller);

            _inputModule = Controller.Character.GetModule<CharacterInputModule>();
            _jumpModule = Controller.Character.GetModule<CharacterJumpModule>();
            
            _jumpModule.onJump += OnJump;
        }

        private void OnDestroy()
        {
            _jumpModule.onJump -= OnJump;
        }

        private void OnJump()
        {
            Animator.SetBool(Jump, true);
        }

        public override void UpdateModule(float deltaTime)
        {
            base.UpdateModule(deltaTime);

            /*var forwardDir = Controller.Character.Transform.forward;
            if (Controller.Character.GroundData.HasGround)
            {
                forwardDir = -Vector3.Cross(Controller.Character.GroundData.Normal,
                    Controller.Character.Transform.right);
            }
            var forwardSpeed = Vector3.Dot(Controller.Character.CharacterController.velocity, forwardDir);*/
            
            Animator.SetFloat(Speed, Controller.Character.CharacterController.velocity.magnitude);
            Animator.SetFloat(MotionSpeed, _inputModule.Inputs.Move.magnitude);
            Animator.SetBool(Grounded, Controller.Character.CharacterController.isGrounded);

            if (Controller.Character.GroundData.HasGround)
            {
                Animator.SetBool(Jump, false);
                _freeFallTimer = 0f;
            }
            else
            {
                _freeFallTimer += deltaTime;
            }
            
            Animator.SetBool("FreeFall", _freeFallTimer > 0.1f);
        }
    }
}
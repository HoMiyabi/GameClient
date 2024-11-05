using UnityEngine;

namespace Movement
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;

        protected Vector2 movementInput;

        protected float baseSpeed = 5f;
        protected float speedModifier = 1f;

        public PlayerMovementState(PlayerMovementStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("状态 " + GetType().Name);
        }

        public virtual void Exit()
        {

        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {

        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }
        #endregion

        #region Main Methods
        private void ReadMovementInput()
        {
            movementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            // if (movementInput == Vector2.zero || speedModifier == 0f)
            // {
            //     return;
            // }

            var movementDirection = GetMovementDirection();

            float movementSpeed = GetMovementSpeed();

            var currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine.Player.Rigidbody.velocity = movementDirection * movementSpeed;
            // stateMachine.Player.Rigidbody.AddForce(movementDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }
        #endregion

        #region Reusable Methods

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            var velocity = stateMachine.Player.Rigidbody.velocity;
            velocity.y = 0f;
            return velocity;
        }

        protected float GetMovementSpeed()
        {
            return baseSpeed * speedModifier;
        }

        protected Vector3 GetMovementDirection()
        {
            return new Vector3(movementInput.x, 0, movementInput.y);
        }
        #endregion
    }
}
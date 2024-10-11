using UnityEngine;

namespace Movement
{
    public class PlayerMovementState : IState
    {
        public virtual void Enter()
        {
            Debug.Log("状态 " + GetType().Name);
        }

        public virtual void Exit()
        {

        }

        public virtual void HandleInput()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void PhysicsUpdate()
        {

        }
    }
}
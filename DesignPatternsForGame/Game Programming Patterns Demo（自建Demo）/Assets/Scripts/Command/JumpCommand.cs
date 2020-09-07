using UnityEngine;

namespace Command
{
    public class JumpCommand : BaseCommand
    {
        public override void Execute()
        {
            base.Execute();
            Jump();
        }

        private void Jump()
        {
            Debug.Log("Jump!");
        }
    }
}
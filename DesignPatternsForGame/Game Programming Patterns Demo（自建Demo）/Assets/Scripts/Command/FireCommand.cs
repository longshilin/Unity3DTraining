using UnityEngine;

namespace Command
{
    public class FireCommand : BaseCommand
    {
        public override void Execute()
        {
            base.Execute();
            Fire();
        }

        private void Fire()
        {
            Debug.Log("Fire!");
        }
    }
}
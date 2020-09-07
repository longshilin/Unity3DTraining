namespace GJ.Game.CameraState
{
    public class DeathState : BaseState
    {
        private float _deathTime;
        private float _timer;

        public void OnInit(CStateBox stateBox, float deathTime)
        {
            base.OnInit(stateBox);
            _deathTime = deathTime;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _timer = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _timer += deltaTime;
            if (_timer > _deathTime)
            {
                ChangeState<WaiteSpawnState>();
            }
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }
    }
}
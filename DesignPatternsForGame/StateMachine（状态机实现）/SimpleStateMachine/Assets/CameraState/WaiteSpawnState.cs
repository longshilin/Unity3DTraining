namespace GJ.Game.CameraState
{
    public class WaiteSpawnState : BaseState
    {
        private const float OnHookReviveTime = 1.0f;

        private float _waiteTime;
        private float _timer;
        private bool _autoSpawn;

        public void OnInit(CStateBox stateBox, float waitTime)
        {
            base.OnInit(stateBox);
            _waiteTime = waitTime;
            _autoSpawn = true;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _timer = 0;
            _waiteTime = OnHookReviveTime;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _timer += deltaTime;
            if (_timer > _waiteTime && _autoSpawn)
            {
                ChangeState<SpwanState>();
            }
        }

        public void SetAutoSpawn(bool value)
        {
            _autoSpawn = value;
        }
    }
}
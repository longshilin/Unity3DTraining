namespace GJ.Game.CameraState
{
    public class SpwanState : BaseState
    {
        public static readonly float EPSILON = 0.5f;

        private float _spwanTime;
        private float _timer;

        public void OnInit(CStateBox stateBox, float spwanTime)
        {
            base.OnInit(stateBox);
            _spwanTime = spwanTime;
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
            if (_timer > _spwanTime)
            {
                ChangeState<ActiveState>();
            }
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public void ForceEnd()
        {
            _timer = _spwanTime;
        }
    }
}
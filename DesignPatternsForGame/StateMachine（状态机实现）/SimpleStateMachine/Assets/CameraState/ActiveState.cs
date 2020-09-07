namespace GJ.Game.CameraState
{
    /// <summary>
    /// 普通状态
    /// </summary>
    public class ActiveState : BaseState
    {
        private float _pauseTime;
        private float _timer;
        private bool _isAfterBornPauseDone; // 出生后的暂停动作是否完成

        public void OnInit(CStateBox stateBox, float pauseTime)
        {
            base.OnInit(stateBox);
            _pauseTime = pauseTime;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (_pauseTime > 0)
            {
                _isAfterBornPauseDone = false;
            }
            else
            {
                _isAfterBornPauseDone = true;
            }

            _timer = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (!_isAfterBornPauseDone)
            {
                _timer += deltaTime;
                if (_timer > _pauseTime)
                {
                    _isAfterBornPauseDone = true;
                }
            }
        }
    }
}
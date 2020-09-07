namespace GJ.Game.CameraState
{
    public class BaseState : IBaseState
    {
        protected CStateBox StateBox;

        public virtual void OnInit(CStateBox stateBox)
        {
            StateBox = stateBox;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void OnLeave()
        {
        }

        public void ChangeState<TState>() where TState : IBaseState
        {
            StateBox.ChangeState(typeof(TState));
        }
    }
}
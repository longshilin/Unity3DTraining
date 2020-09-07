namespace GJ.Game.CameraState
{
    public interface IBaseState
    {
        void OnInit(CStateBox stateBox);

        void OnEnter();

        void OnUpdate(float deltaTime);

        void OnLeave();

        void ChangeState<TState>() where TState : IBaseState;
    }
}
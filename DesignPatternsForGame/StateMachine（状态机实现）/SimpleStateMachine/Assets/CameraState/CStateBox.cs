using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace GJ.Game.CameraState
{
    public class CStateBox
    {
        private readonly Dictionary<Type, IBaseState> m_States = new Dictionary<Type, IBaseState>();
        private IBaseState _currentState;
        private float _currentStateTime = 0;


        public void DoStart()
        {
            var spawnState = new SpwanState();
            m_States.Add(typeof(SpwanState), spawnState);

            var activeState = new ActiveState();
            m_States.Add(typeof(ActiveState), activeState);

            var deathState = new DeathState();
            m_States.Add(typeof(DeathState), deathState);

            var waiteSpawnState = new WaiteSpawnState();
            m_States.Add(typeof(WaiteSpawnState), waiteSpawnState);

            StartState<SpwanState>();
        }

        public void DoUpdate(float deltaTime)
        {
            _currentStateTime += deltaTime;
            _currentState.OnUpdate(deltaTime);
        }

        public void StartState(Type stateType)
        {
            IBaseState state = GetState(stateType);
            if (state == null)
            {
                throw new Exception($"can not state '{stateType.FullName}' which is not exist.");
            }

            _currentState = state;
            _currentStateTime = 0;
            _currentState.OnEnter();
        }

        public void StartState<TState>() where TState : IBaseState
        {
            StartState(typeof(TState));
        }

        public void ChangeState(Type stateType)
        {
            if (stateType == _currentState.GetType())
            {
                Debug.Log($"state '{stateType.FullName}' is running.");
                return;
            }

            IBaseState state = GetState(stateType);
            if (state == null)
            {
                throw new Exception($"can not state '{stateType.FullName}' which is not exist.");
            }

            _currentState.OnLeave();
            _currentState = state;
            _currentStateTime = 0;
            _currentState.OnEnter();
        }

        public void ChangeState<TState>() where TState : IBaseState
        {
            ChangeState(typeof(TState));
        }

        public IBaseState GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new Exception("State type is invalid.");
            }

            IBaseState state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        public TState GetState<TState>()
        {
            var state = GetState(typeof(TState));
            return (TState) state;
        }
    }
}
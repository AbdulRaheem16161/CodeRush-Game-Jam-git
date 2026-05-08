using UnityEngine;

namespace AbdulRaheem.Game.Core
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Tick(float deltaTime);
    }
}

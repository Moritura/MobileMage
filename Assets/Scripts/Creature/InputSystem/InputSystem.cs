using System;
using UnityEngine;

namespace Creature
{
    public abstract class InputSystem : MonoBehaviour
    {
        public event Action<Vector2> MoveEvent;

        protected void OnMoveEvent(Vector2 move) => this.MoveEvent?.Invoke(move);
    }
}
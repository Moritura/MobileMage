using UnityEngine;

namespace Creature
{
    public class InputWASD : InputSystem
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.W)
                || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.S)
                || Input.GetKey(KeyCode.D))
            {
                Move();
            }
        }

        private void Move()
        {
            Vector2 move = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                move += new Vector2(0f, 1f);
            }

            if (Input.GetKey(KeyCode.A))
            {
                move += new Vector2(-1f, 0f);
            }

            if (Input.GetKey(KeyCode.S))
            {
                move += new Vector2(0f, -1f);
            }


            if (Input.GetKey(KeyCode.D))
            {
                move += new Vector2(1f, 0f);
            }

            OnMoveEvent(move.normalized);
        }
    }
}
using UnityEngine;

namespace Creature
{
    public class BaseViewCreature : ViewCreature
    {
        public override void OnMove(Vector2 move)
        {
            transform.position += (Vector3)move * data.Speed * Time.deltaTime;
        }
    }
}
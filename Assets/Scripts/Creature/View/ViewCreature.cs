using UnityEngine;

namespace Creature
{
    public abstract class ViewCreature : MonoBehaviour
    {
        protected DataCreature data;

        public void Init(DataCreature data)
        {
            this.data = data;
        }

        public virtual void OnMove(Vector2 move) { }
    }
}
using System;
using UnityEngine;

namespace Creature
{
    public class DataCreature : MonoBehaviour
    {
        public event Action DieEvent;

        #region Params

        #region HP

        [SerializeField]
        private float maxHp;
        public float MaxHp
        {
            get => maxHp;
            protected set { maxHp = value; }
        }

        private float hp;
        public float Hp
        {
            get => hp;
            protected set
            {
                hp = value;

                if (value <= 0)
                {
                    DieEvent?.Invoke();
                }
            }
        }

        #endregion

        [SerializeField]
        private float speed;
        public float Speed
        {
            get => speed;
            protected set { speed = value; }
        }

        #endregion

        public void Init()
        {
            hp = MaxHp;
        }
    }
}
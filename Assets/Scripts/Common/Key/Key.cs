using System;
using UnityEngine;

namespace Mage.Common
{
    [Serializable]
    public class Key<T> : KeyBase where T : Enum
    {
        #region Declaration

        [field: SerializeField]
        public T Type { get; private set; }

        #endregion

        public Key(T type, int id)
            : base(id)
        {
            Type = type;
        }

        public override bool Equals(KeyBase keyBase)
        {
            return keyBase is Key<T> key && Type.Equals(key.Type) && base.Equals(keyBase);
        }
    }
}
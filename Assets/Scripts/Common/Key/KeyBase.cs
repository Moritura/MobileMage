using UnityEngine;

namespace Mage.Common
{
    public class KeyBase
    {
        #region Declaration

        [field: SerializeField]
        public int Id { get; private set; }

        #endregion

        public KeyBase(int id)
        {
            Id = id;
        }

        public virtual bool Equals(KeyBase other)
        {
            return Id == other.Id;
        }
    }
}
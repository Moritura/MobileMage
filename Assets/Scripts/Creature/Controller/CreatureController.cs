using UnityEngine;

namespace Creature
{
    public class CreatureController : MonoBehaviour
    {
        protected InputSystem inputSystem;
        protected DataCreature dataCreature;
        protected ViewCreature[] views;

        private void Awake()
        {
            inputSystem = GetComponent<InputSystem>();
            dataCreature = GetComponent<DataCreature>();
            dataCreature.Init();

            views = GetComponents<ViewCreature>();
            foreach (ViewCreature view in views)
            {
                view.Init(dataCreature);
                inputSystem.MoveEvent += view.OnMove;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace Game.GameMenu
{
    public class SelectorInvoke : MonoBehaviour
    {
        public UnityEvent eventAction;
        public void Execute() => eventAction.Invoke();
    }
}
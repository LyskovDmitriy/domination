using UnityEngine;


namespace Utils.Ui
{
    public abstract class UiScreen : UiUnit
    {
        [SerializeField] private bool isRoot = default; //root screens don't work as overlayers

        public abstract ScreenType Type { get; }
        public bool IsRoot => isRoot;
    }
}

using UnityEngine;

namespace Kirara
{
    public class ChoiceItem : MonoBehaviour
    {
        public ChoiceGroup choiceGroup;
        public bool chosen = false;
        public int index;

        public void Choose()
        {
            choiceGroup.Choose(index);
        }
    }
}
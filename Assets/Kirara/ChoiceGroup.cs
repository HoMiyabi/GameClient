using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kirara
{
    public class ChoiceGroup : MonoBehaviour
    {
        public int chosenIndex = -1;
        [SerializeField] private List<ChoiceItem> choiceItems;
        public UnityEvent<int> onChoose;
        public UnityEvent<int, int> onChoiceChange;

        public void Add(ChoiceItem choiceItem)
        {
            choiceItem.choiceGroup = this;
            choiceItem.index = choiceItems.Count;
            choiceItems.Add(choiceItem);
        }

        public void Choose(int index)
        {
            onChoose.Invoke(index);
            if (index != chosenIndex)
            {
                if (chosenIndex != -1)
                {
                    choiceItems[chosenIndex].chosen = false;
                }
                choiceItems[index].chosen = true;
                onChoiceChange.Invoke(index, chosenIndex);
                chosenIndex = index;
            }
        }

        private void Awake()
        {
            chosenIndex = -1;
            for (int i = 0; i < choiceItems.Count; i++)
            {
                choiceItems[i].choiceGroup = this;
                choiceItems[i].index = i;
            }
        }
    }
}
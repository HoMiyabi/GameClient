using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBinderSO : ScriptableObject
{
    public MonoBehaviour ui;
    public List<NameAndCom> nameAndComs;

    [Serializable]
    public class NameAndCom
    {
        public string name;
        public Component component;

        public void Deconstruct(out string name, out Component component)
        {
            name = this.name;
            component = this.component;
        }
    }
}
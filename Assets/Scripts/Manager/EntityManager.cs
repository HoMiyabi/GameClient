using System.Collections.Generic;
using Kirara;
using UnityEngine;

namespace Manager
{
    public class EntityManager : Singleton<EntityManager>
    {
        public Dictionary<int, GameObject> entityIdToGO = new();
    }
}
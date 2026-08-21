using CodeBase.Infrastructure.Logic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SceneComponentContainer : MonoBehaviour
    {
        [field: SerializeField] public PolygonCollider2D CameraConfinerCollider { get; private set; }
        [field: SerializeField] public GameObject Finish { get; private set; }
        [field: SerializeField] public List<SawSpawner> SawSpawners { get; private set; } = new List<SawSpawner>();

        public void CollectComponents()
        {
            SawSpawners.Clear();
            SawSpawners = Object.FindObjectsOfType<SawSpawner>().ToList();
        }
    }
}
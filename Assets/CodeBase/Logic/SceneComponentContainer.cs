using CodeBase.Infrastructure.Logic;
using CodeBase.Logic.Traps;
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
        [field: SerializeField] public List<DestroyBlockTimer> BlockTimers { get; private set; } = new List<DestroyBlockTimer>();

        public void CollectComponents()
        {
            SawSpawners.Clear();
            BlockTimers.Clear();
            BlockTimers = Object.FindObjectsOfType<DestroyBlockTimer>().ToList();
            SawSpawners = Object.FindObjectsOfType<SawSpawner>().ToList();
        }
    }
}
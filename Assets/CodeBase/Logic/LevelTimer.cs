using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTimer : MonoBehaviour
    {
        public float GetSeconds() =>
            Time.timeSinceLevelLoad;
    }
}
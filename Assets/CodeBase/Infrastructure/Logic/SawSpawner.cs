using System.Collections;
using CodeBase.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.Logic
{
    public class SawSpawner : MonoBehaviour
    {
        [SerializeField] private FlySawMover _activeSaw;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private float _waitCreate, _waitStartMove;
        private IGameFactory _factory;
        private Vector3 _scale;

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
            _scale = _activeSaw.transform.localScale;
        }

        public void StartSpawnTimer()
        {
            StartCoroutine(SpawnerTimer());
        }

        private IEnumerator SpawnerTimer()
        {
            var waitCreate = new WaitForSeconds(_waitCreate);
            var waitStartMove = new WaitForSeconds(_waitStartMove);
            while (true)
            {
                yield return waitStartMove;
                SawMove();
                yield return waitCreate;
                CreateSaw();
            }
        }

        private void SawMove() =>
            _activeSaw.StartMove(_direction);

        private void CreateSaw()
        {
            GameObject saw = _factory.CreateFlySaw(transform.position, _scale);
            _activeSaw = saw.GetComponent<FlySawMover>();
        }
    }
}
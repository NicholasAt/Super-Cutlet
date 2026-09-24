using CodeBase.Services.Factory;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
        private CancellationToken _ct;

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
            _scale = _activeSaw.transform.localScale;
        }
        private void Awake()
        {
            _ct = this.GetCancellationTokenOnDestroy();
        }

        public void StartSpawnTimer()
        {
            SpawnerTimer(_ct).Forget();
        }

        private async UniTask SpawnerTimer(CancellationToken ct)
        {
            try
            {
                while (ct.IsCancellationRequested == false)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_waitStartMove), cancellationToken: ct);
                    SawMove();
                    await UniTask.Delay(TimeSpan.FromSeconds(_waitCreate), cancellationToken: ct);
                    await CreateSaw(ct);
                }
            }
            catch (OperationCanceledException)
            {
                //ignore
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        private void SawMove() =>
            _activeSaw.StartMove(_direction);

        private async UniTask CreateSaw(CancellationToken ct)
        {
            GameObject saw = await _factory.CreateFlySaw(transform.position, _scale, ct);
            _activeSaw = saw.GetComponent<FlySawMover>();
        }
    }
}
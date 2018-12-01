using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class GameController : SingletoneBehaviour<GameController>
    {
        [SerializeField]
        private Transform _enemySpawnPoint;
        [SerializeField]
        private Enemy _enemyPrefab;
        [SerializeField]
        private Transform _projectileRoot;
        [SerializeField]
        private Bullet _bulletPrefab;

        public static Vector2 Size { get; private set; }

        private bool _started;

        private void Start()
        {
            Size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        }

        public void StartGame()
        {
            _started = true;
            StartCoroutine(SpawnRoutine());
        }

        public IEnumerator SpawnRoutine()
        {
            while (_started)
            {
                var enemy = Instantiate(_enemyPrefab, _enemySpawnPoint);
                enemy.transform.localPosition = Vector3.zero;
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void PlayerShoot(Vector3 position, Vector2 direction)
        {
            if (!_started)
                return;
            var bullet = Instantiate(_bulletPrefab, _projectileRoot);
            bullet.transform.position = position;
            bullet.Move(direction.normalized);
        }
    }
}

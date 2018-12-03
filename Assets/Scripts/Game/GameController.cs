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
        private float _enemySpawnDelay = 0.2f;
        [SerializeField]
        private Transform _enemySpawnPoint;
        [SerializeField]
        private Enemy _enemyPrefab;
        [SerializeField]
        private float _enemySpeed = 1f;
        [SerializeField]
        private float _enemyBoardingSpeed = 1f;
        [SerializeField]
        private float _enemyOnBoardSpeed = 1f;
        [SerializeField]
        private float _enemySpeedOffset = 1f;
        [SerializeField]
        private Transform _projectileRoot;
        [SerializeField]
        private Bullet _bulletPrefab;
        [SerializeField]
        private float _checkPointPath = 10f;
        [SerializeField]
        private Transform _platform;
        [SerializeField]
        private GameObject _shipBack;
        [SerializeField]
        private GameObject _platformBack;
        [SerializeField]
        private GameObject _shipTrigger;

        public static Vector2 Size { get; private set; }
        public static float TrainSpeed { get; private set; }
        public static float EnemySpeed => Instance._enemySpeed + (1 - TrainSpeed) * Instance._enemySpeedOffset;
        public static float EnemyBoardingSpeed => Instance._enemyBoardingSpeed - (1 - TrainSpeed) * Instance._enemySpeedOffset * 2;
        public static float EnemyOnBoardSpeed => Instance._enemyOnBoardSpeed;
        public static float CheckPointState => Instance._currentPath / Instance._checkPointPath;

        public static bool PlayerAllowFly { get; set; } = true;

        private bool _started;
        private float _currentPath;
        private Coroutine _spawnRoutine = null;

        private void Start()
        {
            Size = Camera.main.ScreenTo3DWorld(new Vector2(Screen.width, Screen.height)) * 1.1f;
        }

        public bool StartGame()
        {
            if (!PlayerAllowFly)
                return PlayerAllowFly;
            StartCoroutine(GameRoutine());
            return PlayerAllowFly;
        }

        private IEnumerator GameRoutine()
        {
            _started = true;
            _shipBack.SetActive(true);
            _platformBack.gameObject.SetActive(false);
            _shipTrigger.SetActive(false);
            _currentPath = 0f;
            while (TrainSpeed < 0.3f)
            {
                TrainSpeed += Time.deltaTime / 12f;
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            while (_platform.position.x > -23f)
            {
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }

            _platform.gameObject.SetActive(false);
            _spawnRoutine = StartCoroutine(SpawnRoutine());
            while (TrainSpeed < 1f)
            {
                TrainSpeed += Time.deltaTime / 8f;
                _currentPath += TrainSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            TrainSpeed = 1f;
            while (CheckPointState <= 1f)
            {
                _currentPath += TrainSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            StopCoroutine(_spawnRoutine);

            while (TrainSpeed > 0.3f)
            {
                TrainSpeed -= Time.deltaTime / 12f;
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            _platform.transform.position = Vector3.right * 23f;
            _platform.gameObject.SetActive(true);
            while (_platform.position.x > 5f)
            {
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            while (TrainSpeed > 0.05f)
            {
                TrainSpeed -= Time.deltaTime / 12f;
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            while (_platform.position.x > 0f)
            {
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            _platform.position = Vector3.zero;
            TrainSpeed = 0f;
            _shipBack.SetActive(false);
            _platformBack.gameObject.SetActive(true);
            _shipTrigger.SetActive(true);
        }

        public IEnumerator SpawnRoutine()
        {
            while (_started)
            {
                var enemy = Instantiate(_enemyPrefab, _enemySpawnPoint);
                enemy.Launch();
                yield return new WaitForSeconds(_enemySpawnDelay);
            }
        }

        public void PlayerShoot(Vector3 position, Vector2 direction)
        {
            if (!_started)
                return;
            var bullet = Instantiate(_bulletPrefab, _projectileRoot);
            bullet.transform.position = position;
            bullet.Direction = direction.normalized;
            bullet.Launch();
        }

        public void PlayerThrow(Item item, Vector3 direction)
        {
            item.transform.parent = _projectileRoot;
            item.Direction = direction.normalized;
            item.Launch();
        }

    }
}

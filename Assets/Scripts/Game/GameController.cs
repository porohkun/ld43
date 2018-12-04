using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Layers;
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
        private Enemy[] _enemyPrefabs;
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
        private WorkPlace[] _workPlaces;
        [SerializeField]
        private Collider2D[] _disbleCollidersOnFly;
        [SerializeField]
        private Collider2D[] _enableCollidersOnFly;
        [SerializeField]
        private UsableItem[] _startupItems;
        [SerializeField]
        private Animator _shipAnimator;

        public static Vector2 Size { get; private set; }
        public static float TrainSpeed { get; private set; }
        public static float EnemySpeed => Instance._enemySpeed + (1 - TrainSpeed) * Instance._enemySpeedOffset;
        public static float EnemyBoardingSpeed => Instance._enemyBoardingSpeed - (1 - TrainSpeed) * Instance._enemySpeedOffset * 2;
        public static float EnemyOnBoardSpeed => Instance._enemyOnBoardSpeed;
        public static float CheckPointState => Instance._currentPath / Instance._nextCheckPointPath;
        public static bool Flying => Instance._started;
        public static float FullPath { get; private set; }
        public static bool PlayerAllowFly { get; set; } = true;
        public static int ShipHealth { get; set; } = 10;

        private bool _started;
        private float _currentPath;
        private float _nextCheckPointPath;
        private float _nextSpawnDelay;
        private Coroutine _spawnRoutine = null;
        private Coroutine _gameRoutine = null;

        private void Start()
        {
            Size = Camera.main.ScreenTo3DWorld(new Vector2(Screen.width, Screen.height)) * 1.1f;
        }

        public void NewGame()
        {
            foreach (var en in _enemySpawnPoint.GetChilds<Enemy>().ToArray())
                en.Free();
            _nextCheckPointPath = _checkPointPath;
            _nextSpawnDelay = _enemySpawnDelay;
            ShipHealth = 10;
            _shipAnimator.SetBool("fly", false);
            _shipAnimator.SetBool("die", false);
        }

        public bool StartGame()
        {
            if (!PlayerAllowFly)
                return PlayerAllowFly;
            _gameRoutine = StartCoroutine(GameRoutine());
            return PlayerAllowFly;
        }

        private void Update()
        {
            if (ShipHealth <= 0)
            {
                _shipAnimator.SetBool("die", true);
                GameOver();
            }
        }

        public void GameOver()
        {
            StopCoroutine(_gameRoutine);
            StopCoroutine(_spawnRoutine);
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            while (TrainSpeed > 0f)
            {
                TrainSpeed -= Time.deltaTime / 12f;
                _currentPath += TrainSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            LayersManager.Instance.Push<GameOverLayer>();
        }

        private IEnumerator GameRoutine()
        {
            _nextCheckPointPath = _nextCheckPointPath * 1.2f;
            _nextSpawnDelay = _nextSpawnDelay / 1.1f;
            _started = true;
            foreach (var coll in _enableCollidersOnFly)
                coll.enabled = true;
            foreach (var coll in _disbleCollidersOnFly)
                coll.enabled = false;
            _currentPath = 0f;
            _shipAnimator.SetBool("fly", true);
            while (TrainSpeed < 0.4f)
            {
                TrainSpeed += Time.deltaTime / 9f;
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            while (_platform.position.x > -30f)
            {
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }

            _platform.gameObject.SetActive(false);
            _spawnRoutine = StartCoroutine(SpawnRoutine());
            //while (TrainSpeed < 1f)
            //{
            //    TrainSpeed += Time.deltaTime / 8f;
            //    _currentPath += TrainSpeed * Time.deltaTime;
            //    yield return new WaitForEndOfFrame();
            //}
            //TrainSpeed = 1f;
            while (CheckPointState <= 1f)
            {
                TrainSpeed += Time.deltaTime / 8f * Mathf.Sign(1f + _workPlaces.Sum(w => w.ShipSpeed) - TrainSpeed);
                _currentPath += TrainSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            StopCoroutine(_spawnRoutine);



            foreach (var item in _startupItems)
                item.Refresh();



            while (TrainSpeed > 0.4f)
            {
                TrainSpeed -= Time.deltaTime / 9f;
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            _platform.transform.position = Vector3.right * 30f;
            _platform.gameObject.SetActive(true);
            while (_platform.position.x > 5f)
            {
                _platform.position += Vector3.left * TrainSpeed * Time.deltaTime * 8f;
                yield return new WaitForEndOfFrame();
            }
            _shipAnimator.SetBool("fly", false);
            while (TrainSpeed > 0.05f)
            {
                TrainSpeed -= Time.deltaTime / 6f;
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
            _started = false;
            foreach (var coll in _enableCollidersOnFly)
                coll.enabled = false;
            foreach (var coll in _disbleCollidersOnFly)
                coll.enabled = true;
            FullPath += _currentPath;
        }

        public IEnumerator SpawnRoutine()
        {
            while (_started)
            {
                var enemy = Instantiate(_enemyPrefabs.GetRandom(), _enemySpawnPoint);
                enemy.Launch();
                yield return new WaitForSeconds(_nextSpawnDelay);
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

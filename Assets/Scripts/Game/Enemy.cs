using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Enemy : Item, ICacheable
    {
        enum State
        {
            Normal,
            Boarding,
            OnBoard,
            Death
        }

        [SerializeField]
        private Rigidbody2D _rigidBody;
        [SerializeField]
        private SpriteRenderer[] _sprites;
        [SerializeField]
        private int _maxHealth = 3;
        [SerializeField]
        private float _boardingJumpPower = 1f;
        [SerializeField]
        private float _deathJumpPower = 1f;
        [SerializeField]
        private float _deathRotateSpeed = 1f;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _offsetPoint;

        public float Speed
        {
            get
            {
                switch (_state)
                {
                    case State.Normal: return GameController.EnemySpeed;
                    case State.Boarding: return GameController.EnemyBoardingSpeed;
                    case State.OnBoard: return GameController.EnemyOnBoardSpeed;
                    case State.Death: return -GameController.EnemySpeed * 2f;
                    default: return 0f;
                }
            }
        }

        private State _state = State.Normal;
        private int _health;

        public void Launch()
        {
            _rigidBody.freezeRotation = true;
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
            var order = Random.Range(-200, 200) * 5;
            foreach (var sprite in _sprites)
                sprite.sortingOrder += order;
            _offsetPoint.transform.localPosition = Vector3.up * (-order / 2000f);
            _health = _maxHealth;
        }

        private void FixedUpdate()
        {
            _rigidBody.AddForce(new Vector2(Speed * Time.fixedDeltaTime, 0f), ForceMode2D.Force);
        }

        private void Update()
        {
            if (transform.position.y > GameController.Size.y + 1f ||
                transform.position.y < -GameController.Size.y - 1f ||
                transform.position.x > GameController.Size.x + 1f ||
                transform.position.x < -GameController.Size.x * 2.5f)
                Free();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                _health--;
                if (_health <= 0)
                    Death();
                bullet.Free();
            }

            var trigger = collision.GetComponent<Trigger>();
            if (trigger != null)
                switch (trigger.Message)
                {
                    case "boarding":
                        _state = State.Boarding;
                        _rigidBody.AddForce(_boardingJumpPower * Vector2.up, ForceMode2D.Impulse);
                        DigitalRuby.Tween.TweenFactory.Tween(this, _offsetPoint.transform.localPosition, Vector3.zero, 0.2f, DigitalRuby.Tween.TweenScaleFunctions.SineEaseInOut,
                            p => _offsetPoint.transform.localPosition = p.CurrentValue, null);
                        break;
                    case "onboard":
                        _state = State.OnBoard;
                        break;
                    case "kill":
                        Death();
                        break;
                }
        }

        private void Death()
        {
            _rigidBody.freezeRotation = false;
            _state = State.Death;
            gameObject.layer = LayerMask.NameToLayer("Default");
            _rigidBody.AddForce(_deathJumpPower * Vector2.up + _deathJumpPower * 0.4f * Random.insideUnitCircle, ForceMode2D.Impulse);
            _rigidBody.AddTorque(Random.Range(-_deathRotateSpeed, _deathRotateSpeed));
        }

        public void Free()
        {
            Destroy(gameObject);
        }
    }
}

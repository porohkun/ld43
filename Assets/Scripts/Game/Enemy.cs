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
            Death
        }

        [SerializeField]
        private Rigidbody2D _rigidBody;
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private float _boardingJumpPower = 1f;
        [SerializeField]
        private float _onBoardSpeed = 1f;
        [SerializeField]
        private float _deathJumpPower = 1f;
        [SerializeField]
        private float _deathRotateSpeed = 1f;

        public float Speed
        {
            get
            {
                switch (_state)
                {
                    case State.Normal: return _speed;
                    case State.Boarding: return _onBoardSpeed;
                    case State.Death: return -_speed * 2f;
                    default: return 0f;
                }
            }
        }

        private State _state = State.Normal;

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
                Death();
                bullet.Free();
            }
            var trigger = collision.GetComponent<Trigger>();
            if (trigger != null && trigger.Message == "boarding")
            {
                _state = State.Boarding;
                _rigidBody.AddForce(_boardingJumpPower * Vector2.up, ForceMode2D.Impulse);
            }
        }

        private void Death()
        {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class ThrowableItem : Item
    {
        [SerializeField]
        private Rigidbody2D _rigidBody;
        [SerializeField]
        private Collider2D[] _throwEnableColliders;
        [SerializeField]
        private Collider2D[] _throwDisableColliders;
        [SerializeField]
        private float _throwPower = 1f;
        [SerializeField]
        private float _throwSpeed = 1f;

        public bool IsThrowing { get; private set; }

        public override void Launch()
        {
            foreach (var collider in _throwEnableColliders)
                collider.enabled = true;
            foreach (var collider in _throwDisableColliders)
                collider.enabled = false;
            _rigidBody.simulated = true;
            IsThrowing = true;
            _rigidBody.AddForce(Direction * _throwPower, ForceMode2D.Impulse);
        }

        private void Update()
        {
            if (IsThrowing &&
                transform.position.y > GameController.Size.y ||
                transform.position.y < -GameController.Size.y ||
                transform.position.x > GameController.Size.x ||
                transform.position.x < -GameController.Size.x * 2f)
                Free();
        }

        private void FixedUpdate()
        {
            if (IsThrowing)
                _rigidBody.AddForce(new Vector2(-_throwSpeed * Time.fixedDeltaTime, 0f), ForceMode2D.Force);
        }

        public void Free()
        {
            Destroy(gameObject);
        }
    }
}

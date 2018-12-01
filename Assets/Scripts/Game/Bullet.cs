using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour, ICacheable
    {
        [SerializeField]
        private float _speed;

        private Vector3 _direction;

        public void Move(Vector2 direction)
        {
            _direction = direction;
            transform.LookAt(transform.position + Vector3.forward, direction);
        }

        private void Update()
        {
            transform.position += _direction * _speed * Time.deltaTime;
            if (transform.position.y > GameController.Size.y || transform.position.y < -GameController.Size.y || transform.position.x > GameController.Size.x || transform.position.x < -GameController.Size.x * 2f)
                Free();
        }

        public void Free()
        {
            Destroy(gameObject);
        }
    }
}

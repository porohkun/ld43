using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Bullet : Item, ICacheable
    {
        [SerializeField]
        private float _speed;

        public override void Launch()
        {
            transform.LookAt(transform.position + Vector3.forward, Direction);
        }

        private void Update()
        {
            transform.position += Direction * _speed * Time.deltaTime;
            if (transform.position.y > GameController.Size.y || transform.position.y < -GameController.Size.y || transform.position.x > GameController.Size.x || transform.position.x < -GameController.Size.x * 2f)
                Free();
        }

        public void Free()
        {
            Destroy(gameObject);
        }
    }
}

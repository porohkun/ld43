using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Player : Item
    {
        [SerializeField]
        private Rigidbody2D _rigidBody;
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private Transform _gun;
        [SerializeField]
        private Transform _gunEndPoint;

        private Vector3 _gunDirection;

        private void FixedUpdate()
        {
            var dir = Input.GetAxis("Horizontal");
            _rigidBody.AddForce(new Vector2(_speed * dir * Time.fixedDeltaTime, 0f), ForceMode2D.Force);
        }

        private void Update()
        {
            _gunDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _gun.position;
            _gun.LookAt(_gun.position + Vector3.forward, _gunDirection);

            if (Input.GetMouseButtonDown(0))
                GameController.Instance.PlayerShoot(_gunEndPoint.position, _gunDirection);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var trigger = collision.GetComponent<Trigger>();
            if (trigger != null)
                switch (trigger.Message)
                {
                    case "ship":
                        GameController.PlayerAllowFly = true;
                        break;
                }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var trigger = collision.GetComponent<Trigger>();
            if (trigger != null)
                switch (trigger.Message)
                {
                    case "ship":
                        GameController.PlayerAllowFly = false;
                        break;
                }
        }
    }
}

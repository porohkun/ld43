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
        private Transform _gunEndPoint;
        [SerializeField]
        private Transform _carryPoint;
        [SerializeField]
        private Animator _torsoAnimator;
        [SerializeField]
        private Animator _handAnimator;
        [SerializeField]
        private Transform _body;
        [SerializeField]
        private LadderSensor _ladderSensor;

        public bool IsCarrying => CarryingItem != null;

        private Vector3 _gunDirection;
        private UsableItem _currentUsableItem;
        public Item CarryingItem { get; private set; }

        private void Start()
        {
            _handAnimator.speed = 0.00001f;
        }

        private void Update()
        {
            _gunDirection = Camera.main.ScreenTo3DWorld(Input.mousePosition) - transform.position;
            _body.localScale = new Vector3(Mathf.Sign(_gunDirection.x), 1f, 1f);

            var horDir = Input.GetAxis("Horizontal");
            _torsoAnimator.SetBool("walk", !_rigidBody.velocity.x.IsZero());
            if (!horDir.IsZero())
                _rigidBody.velocity = new Vector2(horDir * 6f, _rigidBody.velocity.y);

            if (_ladderSensor.IsLadder)
            {
                var verDir = Input.GetAxis("Vertical");
                if (!verDir.IsZero())
                {
                    if (horDir.IsZero())
                        transform.position = transform.position.SetX(_ladderSensor.LadderX);
                    _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, verDir * 6f);
                    _ladderSensor.LinkedObject.SetActive(false);
                }
                else
                    _ladderSensor.LinkedObject.SetActive(true);
            }

            if (IsCarrying)
            {
                _handAnimator.speed = 1f;
                _handAnimator.Play("cptn_top_carry");
            }
            else
            {
                _handAnimator.speed = 0.00001f;
                _handAnimator.Play("cptn_top_gun", 0, Vector3.Angle(Vector3.down, _gunDirection) / 180f);
            }

            if (Input.GetMouseButtonDown(0))
                if (CarryingItem == null)
                    GameController.Instance.PlayerShoot(_gunEndPoint.position, _gunDirection);
                else
                {
                    GameController.Instance.PlayerThrow(CarryingItem, _gunDirection);
                    CarryingItem = null;
                }
            if (_currentUsableItem != null && Input.GetButtonDown("Use"))
                _currentUsableItem.Use(this);
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
                    case "depth":
                        transform.position = transform.position.SetZ(trigger.ForceZ);
                        break;
                }
            var usableItem = collision.GetComponent<UsableItem>();
            if (usableItem != null)
                _currentUsableItem = usableItem;
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
            var usableItem = collision.GetComponent<UsableItem>();
            if (usableItem == _currentUsableItem)
                _currentUsableItem = null;
        }

        public void Carry(Item item)
        {
            CarryingItem = item;
            if (item != null)
            {
                item.transform.parent = _carryPoint;
                item.gameObject.SetActive(true);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.ToFront();
            }
        }

    }
}

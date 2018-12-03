using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WorkPlace : UsableItem
    {
        [SerializeField]
        private Transform _anchor;
        [SerializeField]
        private Vector3 _playerScale = Vector3.one;
        [SerializeField]
        private Image _workImage;
        [SerializeField]
        private float _workUpSpeed;
        [SerializeField]
        private float _workDownSpeed;
        [SerializeField]
        private float _shipSpeed;

        public float ShipSpeed => _shipSpeed * (_work - 1f);

        private Player _player;
        private Member _member;
        private float _work;

        public override void Use(Player player)
        {
            if (player.IsCarrying)
            {
                var member = player.CarryingItem as Member;
                if (member != null && _member == null)
                {
                    member.transform.parent = _anchor;
                    member.transform.localPosition = Vector3.zero;
                    member.transform.localRotation = Quaternion.identity;
                    member.transform.localScale = _playerScale;
                    member.ToBack();
                    _member = member;
                    player.Carry(null);
                    _member.Work(true);
                }
            }
            else if (_member != null)
            {
                _member.transform.localScale = Vector3.one;
                player.Carry(_member);
                _member.Work(false);
                _member = null;
            }
            else
            {
                player.Bisy = !player.Bisy;
                _player = player.Bisy ? player : null;
            }
        }

        private void FixedUpdate()
        {
            if (_player != null && _player.Bisy)
            {
                _player.transform.position = _anchor.position;
                _player.Body.localScale = _playerScale;
            }
        }

        private void Update()
        {
            if (_player != null || _member != null)
                _work += _workUpSpeed * Time.deltaTime * (_member != null ? 0.5f : 1f);
            else
                _work -= _workDownSpeed * Time.deltaTime;
            if (_work < 0f)
                _work = 0f;
            if (_work > 1f)
                _work = 1f;
            _workImage.fillAmount = _work;
        }
    }
}

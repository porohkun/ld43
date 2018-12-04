using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class MemberPlace : UsableItem
    {
        [SerializeField]
        private Transform _memberPlace;
        [SerializeField]
        private Member[] _memberPrefabs;

        private Member _member;

        private void Awake()
        {
            _member = Instantiate(_memberPrefabs.GetRandom(), _memberPlace);
            _member.transform.localPosition = Vector3.zero;
        }

        public override void Use(Player player)
        {
            if (player.IsCarrying)
            {
                var member = player.CarryingItem as Member;
                if (member != null && _member == null)
                {
                    member.transform.parent = _memberPlace;
                    member.transform.localPosition = Vector3.zero;
                    member.transform.localRotation = Quaternion.identity;
                    member.ToBack();
                    _member = member;
                    player.Carry(null);
                }
            }
            else if (_member != null)
            {
                player.Carry(_member);
                _member = null;
            }
        }

        public override void Refresh()
        {
            Destroy(_member.gameObject);
            _member = null;
            if (Random.value < 0.2f)
            {
                _member = Instantiate(_memberPrefabs.GetRandom(), _memberPlace);
                _member.transform.localPosition = Vector3.zero;
            }
        }

        public override void Initial()
        {
            Destroy(_member.gameObject);
            _member = null;
            {
                _member = Instantiate(_memberPrefabs.GetRandom(), _memberPlace);
                _member.transform.localPosition = Vector3.zero;
            }
        }
    }
}

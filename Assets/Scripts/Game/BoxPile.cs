using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class BoxPile : UsableItem
    {
        [SerializeField]
        private Transform[] _boxPlaces;
        [SerializeField]
        private Box _boxPrefab;

        private List<Box> _boxes = new List<Box>();

        private void Awake()
        {
            foreach (var boxPlace in _boxPlaces)
                _boxes.AddRange(boxPlace.GetChilds<Box>());
        }

        public override void Use(Player player)
        {
            if (player.IsCarrying)
            {
                var box = player.CarryingItem as Box;
                if (box != null && _boxes.Count < _boxPlaces.Length)
                {
                    box.transform.parent = _boxPlaces[_boxes.Count];
                    box.transform.localPosition = Vector3.zero;
                    box.transform.localRotation = Quaternion.identity;
                    box.ToBack();
                    _boxes.Add(box);
                    player.Carry(null);
                }
            }
            else if (_boxes.Count > 0)
            {
                var box = _boxes.RemoveAtAndReturn(_boxes.Count - 1);
                player.Carry(box);
            }
        }
    }
}

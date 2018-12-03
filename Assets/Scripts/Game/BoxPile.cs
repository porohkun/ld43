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
        private GameObject[] _boxPlaces;
        [SerializeField]
        private Box _boxPrefab;

        private List<Box> _boxes = new List<Box>();

        private void Awake()
        {
            _boxes.AddRange(transform.GetChilds<Box>());
            for (int i = 0; i < _boxPlaces.Length; i++)
                _boxPlaces[i].SetActive(i < _boxes.Count);
        }

        public override void Use(Player player)
        {
            if (player.IsCarrying)
            {
                var box = player.CarryingItem as Box;
                if (box != null && _boxes.Count < _boxPlaces.Length)
                {
                    box.transform.parent = transform;
                    box.gameObject.SetActive(false);
                    box.transform.localPosition = Vector3.zero;
                    box.transform.localRotation = Quaternion.identity;
                    box.ToBack();
                    _boxes.Add(box);
                    for (int i = 0; i < _boxPlaces.Length; i++)
                        _boxPlaces[i].SetActive(i < _boxes.Count);
                    player.Carry(null);
                }
            }
            else if (_boxes.Count > 0)
            {
                var box = _boxes.RemoveAtAndReturn(_boxes.Count - 1);
                for (int i = 0; i < _boxPlaces.Length; i++)
                    _boxPlaces[i].SetActive(i < _boxes.Count);
                player.Carry(box);
            }
        }
    }
}

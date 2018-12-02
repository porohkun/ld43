using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Box : Item
    {
        [SerializeField]
        private SpriteRenderer _sprite;

        public override void ToFront()
        {
            _sprite.sortingLayerName = "Player";
        }

        public override void ToBack()
        {
            _sprite.sortingLayerName = "Train";
        }
    }
}

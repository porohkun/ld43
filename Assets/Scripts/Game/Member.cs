using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Member : ThrowableItem
    {
        [SerializeField]
        private SpriteRenderer[] _sprites;
        [SerializeField]
        private string _sortingLayer;
        [SerializeField]
        private Animator _animator;

        public override void Launch()
        {
            base.Launch();
            gameObject.layer = LayerMask.NameToLayer("Projectile");
        }

        public override void ToFront()
        {
            foreach (var sprite in _sprites)
                sprite.sortingLayerName = _sortingLayer;
        }

        public override void ToBack()
        {
            foreach (var sprite in _sprites)
                sprite.sortingLayerName = "Train";
        }

        public void Work(bool work)
        {
            _animator.SetBool("work", work);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Jet : MonoBehaviour
    {
        [SerializeField]
        private float _width;
        [SerializeField]
        private float _speed;

        private List<SpriteRenderer> _sequence = new List<SpriteRenderer>();
        private float _fullDist;

        private void Start()
        {
            _sequence.AddRange(transform.GetChilds<SpriteRenderer>());
        }

        private void Update()
        {
            _fullDist = _sequence.Count * _width;
            foreach (var item in _sequence)
            {
                item.transform.localPosition += Vector3.down * _speed * Time.deltaTime;
                item.color = item.color.SetA((item.transform.localPosition.y / _fullDist));
            }
            var item0 = _sequence[0];
            if (item0.transform.localPosition.y < -_width)
            {
                _sequence.RemoveAt(0);
                item0.transform.localPosition = _sequence.Last().transform.localPosition + Vector3.up * _width;
                _sequence.Add(item0);
            }
        }
    }
}

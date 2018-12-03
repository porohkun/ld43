using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Factory : MonoBehaviour
    {
        [SerializeField]
        private float _width;
        [SerializeField]
        private float _speed;

        private List<Transform> _sequence = new List<Transform>();

        private void Start()
        {
            _sequence.AddRange(transform.GetChilds());
        }

        private void Update()
        {
            foreach (var item in _sequence)
                item.localPosition += Vector3.left * _speed * GameController.TrainSpeed * Time.deltaTime;
            var item0 = _sequence[0];
            if (item0.localPosition.x < -_width)
            {
                _sequence.RemoveAt(0);
                item0.localPosition = _sequence.Last().localPosition + Vector3.right * _width;
                _sequence.Add(item0);
            }
        }
    }
}

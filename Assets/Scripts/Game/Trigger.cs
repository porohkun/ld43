using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField]
        private string _message;
        [SerializeField]
        private GameObject _linkedObject;
        [SerializeField]
        private float _forceZ;

        public string Message => _message;
        public GameObject LinkedObject => _linkedObject;
        public float ForceZ => _forceZ;
    }
}

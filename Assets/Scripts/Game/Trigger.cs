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

        public string Message => _message;
    }
}

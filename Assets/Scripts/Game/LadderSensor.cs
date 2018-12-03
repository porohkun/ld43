using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class LadderSensor:MonoBehaviour
    {
        public bool IsLadder { get; private set; }
        public float LadderX { get; private set; }
        public GameObject LinkedObject { get; private set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var trigger = collision.GetComponent<Trigger>();
            if (trigger != null)
                switch (trigger.Message)
                {
                    case "ladder":
                        IsLadder = true;
                        LadderX = trigger.transform.position.x;
                        LinkedObject = trigger.LinkedObject;
                        break;
                }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var trigger = collision.GetComponent<Trigger>();
            if (trigger != null)
                switch (trigger.Message)
                {
                    case "ladder":
                        IsLadder = false;
                        LinkedObject.SetActive(true);
                        LinkedObject = null;
                        break;
                }
        }

    }
}

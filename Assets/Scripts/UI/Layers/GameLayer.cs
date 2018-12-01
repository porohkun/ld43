using UnityEngine;
using Game;

namespace UI.Layers
{
    public class GameLayer : LayerBase
    {
        public void Start()
        {
            GameController.Instance.StartGame(); 
        }
    }
}

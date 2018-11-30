using UnityEngine;

namespace UI.Layers
{
    public class StartLayer : LayerBase
    {
        private void Start()
        {
            LayersManager.Instance.Push<MainMenuLayer>();

            LayersManager.Instance.FadeIn(2.5f, null);
        }
    }
}

using UnityEngine;

namespace UI.Layers
{
    public class GameOverLayer : LayerBase
    {

        public void ToMenuClick()
        {
            LayersManager.Instance.FadeOut(0.5f, () =>
            {
                LayersManager.Instance.PopTill<MainMenuLayer>();
                LayersManager.Instance.FadeIn(0.5f, null);
            });
        }
    }
}

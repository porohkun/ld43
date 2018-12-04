using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TextPro = TMPro.TextMeshProUGUI;

namespace UI.Layers
{
    public class MainMenuLayer : LayerBase
    {
        protected override void OnFloatUp()
        {
            base.OnFloatUp();
            Game.GameController.Instance.NewGame();
        }

        public void StartButtonClick()
        {
            LayersManager.Instance.FadeOut(0.5f, () =>
            {
                LayersManager.Instance.Push<GameLayer>();
                LayersManager.Instance.FadeIn(0.5f, null);
            });
        }
    }
}

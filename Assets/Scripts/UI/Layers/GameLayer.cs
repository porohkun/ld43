using UnityEngine;
using UnityEngine.UI;
using Game;

namespace UI.Layers
{
    public class GameLayer : LayerBase
    {
        [SerializeField]
        private RectTransform _speedArrow;
        [SerializeField]
        private Image _progressLine;
        [SerializeField]
        private RectTransform _progressSprite;

        public void Start()
        {
            GameController.Instance.StartGame();
        }

        private void Update()
        {
            _speedArrow.localRotation = Quaternion.Euler(0f, 0f, (1f - GameController.TrainSpeed) * 90f);
            _progressLine.fillAmount = GameController.CheckPointState;
            _progressSprite.anchorMin = new Vector2(GameController.CheckPointState, 0f);
            _progressSprite.anchorMax = new Vector2(GameController.CheckPointState, 1f);
        }
    }
}

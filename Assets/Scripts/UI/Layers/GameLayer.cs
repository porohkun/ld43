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
        private Image _antiProgressLine;
        [SerializeField]
        private RectTransform _progressSprite;
        [SerializeField]
        private GameObject _startButton;

        public void Start()
        {
            //GameController.Instance.StartGame();
        }

        public void FlyClick()
        {
            GameController.Instance.StartGame();
        }

        private void Update()
        {
            _startButton.SetActive(!GameController.Flying);
            _speedArrow.localRotation = Quaternion.Euler(0f, 0f, 10f + (1f - GameController.TrainSpeed) * 65f);
            _progressLine.fillAmount = GameController.CheckPointState;
            _antiProgressLine.fillAmount = 1f - GameController.CheckPointState;
            _progressSprite.anchorMin = new Vector2(GameController.CheckPointState, 0f);
            _progressSprite.anchorMax = new Vector2(GameController.CheckPointState, 0f);
        }
    }
}

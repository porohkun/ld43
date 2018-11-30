using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Layers
{
    public class LayersManager : MonoBehaviour
    {
        public static LayersManager Instance { get; private set; }

        private Dictionary<string, LayerBase> _instances = new Dictionary<string, LayerBase>();
        private List<LayerBase> _stack = new List<LayerBase>();

        [SerializeField]
        private bool _isStatic = false;
        [SerializeField]
        private Canvas _uiCanvas;
        [SerializeField]
        private RectTransform _container;
        [SerializeField]
        private CanvasGroup _fader;
        [SerializeField]
        private bool _showLayersInGui = false;
        [SerializeField]
        private bool _dontDestroyOnLoad = true;

        void Awake()
        {
            if (_fader != null)
                _fader.alpha = 1f;
            if (_isStatic)
                Instance = this;
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(this.gameObject);
            if (_uiCanvas == null) _uiCanvas = GetComponent<Canvas>();
            if (_container == null) _container = _uiCanvas.transform as RectTransform;

            var preloadedLayers = new List<LayerBase>();
            for (int i = 0; i < _container.childCount; i++)
                preloadedLayers.Add(_container.GetChild(i).GetComponent<LayerBase>());
            foreach (var layer in preloadedLayers)
            {
                _instances.Add(layer.GetType().Name, layer);
                layer.Instantiate(this);
                layer.gameObject.SetActive(false);
            }
            if (preloadedLayers.Count > 0)
                Push(preloadedLayers[0]);
        }

        #region Fading

        public void FadeIn(float fadeTime, Action afterFade)
        {
            StartCoroutine(FadeRoutine(1f, 0f, fadeTime, afterFade));
        }

        public void FadeOut(float fadeTime, Action afterFade)
        {
            StartCoroutine(FadeRoutine(0f, 1f, fadeTime, afterFade));
        }

        private IEnumerator FadeRoutine(float from, float to, float duration, Action finalAction)
        {
            float time = Time.realtimeSinceStartup;
            if (_fader != null)
            {
                _fader.blocksRaycasts = true;
                _fader.alpha = from;
            }
            while (Time.realtimeSinceStartup < time + duration)
            {
                _fader.alpha = Mathf.Lerp(from, to, (Time.realtimeSinceStartup - time) / duration);
                yield return new WaitForEndOfFrame();
            }
            if (_fader != null)
            {
                _fader.alpha = to;
                _fader.blocksRaycasts = to != 0f;
            }
            if (finalAction != null)
                finalAction();
        }

        #endregion

        public TLayer GetLayer<TLayer>() where TLayer : LayerBase
        {
            var layerName = typeof(TLayer).Name;
            if (_instances.ContainsKey(layerName))
                return _instances[layerName] as TLayer;

            return InstantiateFromPrefab((TLayer)Resources.Load(string.Format("Layers/{0}", layerName), typeof(TLayer))) as TLayer;
        }

        public LayerBase InstantiateFromPrefab(LayerBase prefab)
        {
            var layerName = prefab.GetType().Name;
            var instance = Instantiate(prefab);
            if (!instance.DestroyOnLoad)
                DontDestroyOnLoad(instance);
            instance.name = layerName;
            var rt = instance.transform as RectTransform;
            rt.SetParent(_container);
            rt.localScale = Vector3.one;
            instance.transform.position = Vector3.zero;
            rt.anchoredPosition = Vector3.zero;
            rt.sizeDelta = Vector3.zero;

            instance.Enabled = false;
            instance.Instantiate(this);
            if (_instances.ContainsKey(layerName))
                _instances.Remove(layerName);
            _instances.Add(layerName, instance);
            return instance;
        }

        public bool HaveLayer<TLayer>()
        {
            var layerName = typeof(TLayer).Name;
            return _instances.ContainsKey(layerName);
        }

        public void DestroyLayer(LayerBase layer)
        {
            var layerName = layer.GetType().Name;
            if (_instances.ContainsKey(layerName))
            {
                Pop(layer);
                _instances.Remove(layerName);
            }
            GameObject.Destroy(layer.gameObject);
        }

        public LayerBase TopLayer { get { if (_stack.Count > 0) return _stack.Last(); else return null; } }

        private void Push(LayerBase layer, bool withSwitch)
        {
            Pop(layer);
            _stack.Add(layer);
            layer.transform.SetAsLastSibling();
            if (withSwitch)
                SwitchLayersActivity();
        }

        public void Push(LayerBase layer)
        {
            Push(layer, true);
        }

        public TLayer Push<TLayer>() where TLayer : LayerBase
        {
            var layer = GetLayer<TLayer>();
            Push(layer);
            return layer;
        }

        private LayerBase Pop(bool withSwitch)
        {
            var count = _stack.Count;
            LayerBase result = null;
            if (count > 1)
            {
                result = _stack[count - 1];
                result.Enabled = false;
                _stack.RemoveAt(count - 1);
            }
            if (withSwitch)
                SwitchLayersActivity();
            return result;
        }

        public LayerBase Pop()
        {
            return Pop(true);
        }

        public void Hide(bool val)
        {
            gameObject.SetActive(!val);
        }

        public void Pop(LayerBase layer)
        {
            if (_stack != null && _stack.Contains(layer))
            {
                layer.Enabled = false;
                _stack.Remove(layer);
                SwitchLayersActivity();
            }
        }

        public void Pop<TLayer>() where TLayer : LayerBase
        {
            LayerBase layerForRemove = null;
            if (_stack == null) return;
            foreach (var layer in _stack)
            {
                if (layer.GetType() == typeof(TLayer))
                {
                    layerForRemove = layer;
                    break;
                }
            }
            if (layerForRemove != null)
                Pop(layerForRemove);
        }

        public void PopTill(LayerBase layer, bool destroy = false, bool include = false)
        {
            while (_stack.Count > 1)
            {
                var pLayer = Pop(false);
                if (pLayer != null)
                {
                    if (pLayer == layer)
                    {
                        if (!include)
                            Push(pLayer, false);
                        else
                            DestroyLayer(pLayer);
                        break;
                    }
                    else
                        DestroyLayer(pLayer);
                }
            }
            SwitchLayersActivity();
        }

        public void PopTill<TLayer>(bool destroy = false, bool include = false) where TLayer : LayerBase
        {
            while (_stack.Count > 1)
            {
                var layer = Pop(false);
                if (layer != null)
                {
                    if (layer.GetType() == typeof(TLayer))
                    {
                        if (!include)
                            Push(layer, false);
                        else
                            DestroyLayer(layer);
                        break;
                    }
                    else
                        DestroyLayer(layer);
                }
            }
            SwitchLayersActivity();
        }

        public void Top(LayerBase layer)
        {
            if (_stack.Contains(layer))
            {
                _stack.Remove(layer);
                Push(layer);
            }
        }

        public void Top<TLayer>() where TLayer : LayerBase
        {
            LayerBase layerForTop = null;
            foreach (var layer in _stack)
            {
                if (layer.GetType() == typeof(TLayer))
                {
                    layerForTop = layer;
                    break;
                }
            }
            if (layerForTop != null)
                Top(layerForTop);
        }


        public bool Contains<TLayer>() where TLayer : LayerBase
        {
            foreach (var layer in _stack)
                if (layer.GetType() == typeof(TLayer))
                    return true;
            return false;
        }

        private void SwitchLayersActivity()
        {
            int hiding = -1;

            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                var layer = _stack[i];
                if (layer.IsTopLayer)
                    layer.Enabled = true;
                else
                    layer.Enabled = (int)layer.Visibility > hiding;
                hiding = Math.Max(hiding, (int)layer.Hiding);
            }
            if (TopLayer != null)
            {
                TopLayer.FloatUp();
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (_showLayersInGui)
                for (int i = 0; i < _stack.Count; i++)
                {
                    UnityEngine.GUI.Label(new Rect(Screen.width - 150, Screen.height - 25 * (_stack.Count - i), 150, 25), _stack[i].GetType().Name);
                }
        }
#endif
    }
}
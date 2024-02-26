using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Web Image", 11)]
    public class UIWebImage : RawImage
    {
        [SerializeField]
        private string m_ImageUrl;
        public string ImageUrl
        {
            get
            {
                return m_ImageUrl;
            }
            set
            {
                if (m_ImageUrl != value)
                {
                    m_ImageUrl = value;

                    StartCoroutine(LoadWebImage());
                }
            }
        }

        [SerializeField]
        private int m_Timeout = 3;
        public int RequestTimeout
        {
            get
            {
                return m_Timeout;
            }
            set
            {
                if (value <= 0)
                {
                    value = 3;
                }
                m_Timeout = value;
            }
        }

        [SerializeField]
        private bool m_IsSetNativeSize = true;
        public bool IsSetNativeSize
        {
            get
            {
                return m_IsSetNativeSize;
            }
            set
            {
                if (m_IsSetNativeSize != value)
                {
                    m_IsSetNativeSize = value;
                    if (texture != null)
                    {
                        SetNativeSize();
                    }
                }
            }
        }

        [SerializeField]
        private UIWebImageFinishedAction m_FinishedAction = new UIWebImageFinishedAction();
        public UIWebImageFinishedAction FinishedAction
        {
            get { return m_FinishedAction; }
            set { m_FinishedAction = value; }
        }


        protected override void Awake()
        {
            base.Awake();
            if (!string.IsNullOrEmpty(ImageUrl) && Application.isPlaying)
            {
                StartCoroutine(LoadWebImage());
            }
        }

        private UnityWebRequest webRequest = null;
        private IEnumerator LoadWebImage()
        {
            yield return new WaitForEndOfFrame();

            if (webRequest != null)
            {
                webRequest.Abort();
                webRequest.Dispose();
                webRequest = null;
            }

            if (string.IsNullOrEmpty(ImageUrl))
            {
                yield break;
            }

            webRequest = UnityWebRequestTexture.GetTexture(ImageUrl);
            webRequest.timeout = RequestTimeout;
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                FinishedAction?.Invoke(false);
            }
            else
            {
                Texture texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                if (texture != null)
                {
                    texture.hideFlags = HideFlags.DontSave;

                    base.texture = texture;
                    if (IsSetNativeSize)
                    {
                        SetNativeSize();
                    }
                    FinishedAction?.Invoke(true);
                }
                else
                {
                    FinishedAction?.Invoke(false);
                }
            }

        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (texture == null)
            {
                toFill.Clear();
            }
            else
            {
                base.OnPopulateMesh(toFill);
            }
        }

        protected override void OnDestroy()
        {
            if (webRequest != null)
            {
                webRequest.Abort();
                webRequest.Dispose();
                webRequest = null;
            }

            base.OnDestroy();
        }
    }

    [Serializable]
    public class UIWebImageFinishedAction : UnityEvent<bool>
    {
        public UIWebImageFinishedAction() { }
    }
}


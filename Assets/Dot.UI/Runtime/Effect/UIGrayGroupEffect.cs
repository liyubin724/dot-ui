using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    [AddComponentMenu("UI/Effects/Gray Group")]
    public class UIGrayGroupEffect : MonoBehaviour
    {
        [SerializeField]
        private Material m_GrayMat;
        [SerializeField]
        private UIGrayAnimationType m_AnimationType = UIGrayAnimationType.None;
        [SerializeField]
        private bool m_IsGray = false;
        [SerializeField]
        private float m_FromGray = 0f;
        [SerializeField]
        private float m_ToGray = 1f;
        [SerializeField]
        private float m_AnimTime = 0.2f;

        [SerializeField]
        private bool m_IsAllGraphic = true;
        [SerializeField]
        private Graphic[] m_Graphics;

        private Material m_Material;

        private float m_EleapseTime = 0f;
        private bool m_IsRunning = false;

        private void Awake()
        {
            if (m_GrayMat != null)
            {
                m_Material = new Material(m_GrayMat);
            }

            if (m_IsAllGraphic)
            {
                m_Graphics = GetComponentsInChildren<Graphic>(true);
            }

            ApplyEffect();
        }

        private void ApplyEffect()
        {
            if (m_Material == null || m_Graphics == null || m_Graphics.Length == 0) return;

            if (m_AnimationType == UIGrayAnimationType.None)
            {
                foreach (var graphic in m_Graphics)
                {
                    if (m_IsGray)
                    {
                        graphic.material = m_Material;
                    }
                    else
                    {
                        graphic.material = null;
                    }
                }

                m_EleapseTime = 0f;
                m_IsRunning = false;
            }
            else if (m_AnimationType == UIGrayAnimationType.Lerp)
            {
                m_Material.SetFloat("_Gray", m_FromGray);
                foreach (var graphic in m_Graphics)
                {
                    graphic.material = m_Material;
                }

                m_EleapseTime = 0f;
                m_IsRunning = true;
            }
        }

        public void ApplyWithLerp(float from, float to, float animTime)
        {
            m_AnimationType = UIGrayAnimationType.Lerp;
            m_FromGray = from;
            m_ToGray = to;
            m_AnimTime = animTime;

            ApplyEffect();
        }

        public void Apply(bool isGray)
        {
            m_AnimationType = UIGrayAnimationType.None;
            m_IsGray = isGray;

            ApplyEffect();
        }

        private void Update()
        {
            if (!m_IsRunning)
            {
                return;
            }

            if (m_AnimationType == UIGrayAnimationType.Lerp)
            {
                m_EleapseTime += Time.deltaTime;
                if (m_EleapseTime >= m_AnimTime)
                {
                    m_IsRunning = false;
                    if (m_ToGray > 0f)
                    {
                        m_IsGray = true;
                    }
                    else
                    {
                        m_IsGray = false;
                        foreach (var graphic in m_Graphics)
                        {
                            graphic.material = null;
                        }
                    }
                }
                else
                {
                    var gray = Mathf.Lerp(m_FromGray, m_ToGray, m_EleapseTime / m_AnimTime);
                    m_Material.SetFloat("_Gray", Mathf.Max(gray, 0f));
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Graphics))]
    public class LongClickBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float longTime = 0.1f;

        public UnityEvent clickEvent = new UnityEvent();
        public UnityEvent longClickEvent = new UnityEvent();

        private bool m_IsPressed = false;
        private float m_ElapsedTime = 0.0f;
        private bool m_IsLongClick = false;

        void Update()
        {
            if (m_IsLongClick)
            {
                return;
            }

            if (m_IsPressed)
            {
                m_ElapsedTime += Time.deltaTime;
                if (m_ElapsedTime > longTime)
                {
                    m_IsLongClick = true;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_IsPressed = true;
            m_ElapsedTime = 0.0f;
            m_IsLongClick = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!m_IsPressed)
            {
                return;
            }

            if (m_IsLongClick)
            {
                longClickEvent.Invoke();
            }
            else
            {
                clickEvent.Invoke();
            }
        }
    }
}

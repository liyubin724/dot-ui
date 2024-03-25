using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Graphics))]
    public class RepeatPressBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float triggerInterval = 0.3f;

        public UnityEvent startEvent = new UnityEvent();
        public UnityEvent intervalEvent = new UnityEvent();
        public UnityEvent endEvent = new UnityEvent();

        private bool m_IsPressed = false;
        private float m_ElapsedTime = 0.0f;

        void Update()
        {
            if (!m_IsPressed)
            {
                return;
            }

            m_ElapsedTime += Time.deltaTime;
            if (m_ElapsedTime >= triggerInterval)
            {
                intervalEvent.Invoke();
                m_ElapsedTime -= triggerInterval;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_IsPressed = true;
            m_ElapsedTime = 0.0f;
            startEvent.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!m_IsPressed)
            {
                return;
            }

            m_IsPressed = false;
            endEvent.Invoke();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class Follower : MonoBehaviour
    {
        public GameObject Panel;
        public CanvasGroup CanvasGroup;

        protected IFollowed followed;

        private RectTransform rectTransform;
        private UnityEngine.Camera mainCamera;
        private Vector2 Position;

        private bool shown = true;

        public void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            mainCamera = UnityEngine.Camera.main;
            Position = rectTransform.position;
        }

        public void FixedUpdate()
        {
            rectTransform.anchoredPosition = Position;// Vector2.Lerp(rectTransform.anchoredPosition, Position,1/*10f*UnityEngine.Time.fixeddeltaTime*/);
            if (followed != null && Panel.activeInHierarchy)
            {
                var mainCanvas = mainCamera.GetComponentInChildren<Canvas>();
                Position = mainCamera.WorldToScreenPoint(followed.FollowedPosition());
            }
            if (Vector3.Distance(mainCamera.transform.position, followed.FollowedPosition()) > 40000|| Vector3.Angle(followed.FollowedPosition(), mainCamera.transform.position) >= 90f)
            {
                if (shown)
                {
                    Hide();
                }
                return;
            }
            if (!shown&&Vector3.Angle(followed.FollowedPosition(), mainCamera.transform.position)<90f)
            {
                Show();
            }
        }

        public void UpdateFollowed(IFollowed _followed)
        {
            followed = _followed;
        }

        private void Show()
        {
            shown = true;
            Panel.SetActive(true);
            LeanTween.alphaCanvas(CanvasGroup, 1, 0.2f).setEaseInOutSine();
        }

        private void Hide()
        {
            shown = false;
            LeanTween.alphaCanvas(CanvasGroup, 0, 0.2f).setEaseInOutSine().setOnComplete(() => HideEnd());
        }

        private void HideEnd()
        {
            Panel.SetActive(false);
        }

        public void Destroy()
        {
            LeanTween.alphaCanvas(CanvasGroup, 0, 0.2f).setEaseInOutSine().setOnComplete(() => Remove());
        }

        private void Remove()
        {
            GameObject.Destroy(gameObject);
        }
    }
}

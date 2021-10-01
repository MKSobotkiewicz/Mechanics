using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Utility
{
    public class MouseRaycasting : MonoBehaviour
    {
        public Player.Player Player;
        public Canvas MainCanvas;
        public RectTransform SelectionSquare;
        public bool beganSelecting;
        private new UnityEngine.Camera camera;

        private bool beganOrdering;
        private readonly Vector3[] corners = new Vector3[2];

        void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            if (camera == null)
            {
                Debug.LogError(name + " missing camera");
            }
            beganSelecting = false;
            beganOrdering = false;
        }

        void Update()
        {
            if (Input.GetAxis("Select") > 0)
            {
                SelectTrue();
            }
            if (Input.GetAxis("Select") <= 0)
            {
                SelectFalse();
            }
            if (Input.GetAxis("Order") > 0)
            {
                OrderTrue();
            }
            if (Input.GetAxis("Order") <= 0)
            {
                OrderFalse();
            }
        }
        
        private void SelectTrue()
        {
            if (!beganSelecting)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50000))
                {
                    var clickable = hit.transform.gameObject.GetComponent<Clickable>();
                    if (clickable != null)
                    {
                        clickable.Click();
                    }
                }
                beganSelecting = true;
                corners[0] = Input.mousePosition;
            }
            else
            {
                SelectionSquare.gameObject.SetActive(true);
                corners[1] = Input.mousePosition;
                SelectionSquare.anchoredPosition = new Vector2(Mathf.Min(corners[0].x, corners[1].x), Mathf.Min(corners[0].y, corners[1].y));
                SelectionSquare.sizeDelta = new Vector2(Mathf.Max(corners[0].x, corners[1].x) - Mathf.Min(corners[0].x, corners[1].x),
                                                      Mathf.Max(corners[0].y, corners[1].y) - Mathf.Min(corners[0].y, corners[1].y));
            }
        }

        private void SelectFalse()
        {
            if (beganSelecting)
            {
                beganSelecting = false;
                SelectionSquare.gameObject.SetActive(false);
                Player.UnselectAllUnits();

                var minX = Mathf.Min((int)corners[0].x, (int)corners[1].x);
                var maxX = Mathf.Max((int)corners[0].x, (int)corners[1].x);
                var minY = Mathf.Min((int)corners[0].y, (int)corners[1].y);
                var maxY = Mathf.Max((int)corners[0].y, (int)corners[1].y);
                foreach (var unit in Units.Unit.AllUnits)
                {
                    var position=UnityEngine.Camera.main.WorldToScreenPoint(unit.transform.position);
                    if (position.x> minX&& position.x< maxX&& position.y > minY && position.y < maxY/*&&Equals(unit.Organization, Player.Organization)*/)
                    {
                        Player.SelectUnit(unit);
                    }
                }
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50000))
                {
                    var clickable = hit.transform.gameObject.GetComponent<Clickable>();
                    if (clickable != null)
                    {
                        clickable.Unclick();
                    }
                }
            }
        }

        private void OrderTrue()
        {
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50000))
            {
                var clickable = hit.transform.gameObject.GetComponent<Clickable>();
                if (clickable != null)
                {
                    clickable.Order();
                }
            }
        }

        private void OrderFalse()
        {
            if (beganOrdering == true)
            {
                beganOrdering = false;
            }
        }
    }
}
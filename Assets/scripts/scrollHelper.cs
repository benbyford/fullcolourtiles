using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class scrollHelper : MonoBehaviour {

	public GameObject menu;
	[SerializeField] ScrollRect sr;
	float y;
	float h;
	RectTransform menuRect;

	Image pointImg;

    [SerializeField] UiController uiCon;

	bool usingMouse;

    void Start()
	{
		menuRect = menu.GetComponent<RectTransform>();

        pointImg = uiCon.pointer.GetComponent<Image>();
    }

	void Update()
	{
		y = menuRect.anchoredPosition.y;
		//Debug.Log(y);
    }
    public void SnapTo(GameObject current)
    {
		if (!usingMouse)
		{
			pointImg.enabled = true;


			RectTransform currentRect = current.GetComponent<RectTransform>();

			Canvas.ForceUpdateCanvases();

			var contentPos = (Vector2)sr.transform.InverseTransformPoint(sr.content.position);
			var childPos = (Vector2)sr.transform.InverseTransformPoint(currentRect.position);
			Vector2 endPos = contentPos - childPos;

			// If no horizontal scroll, then don't change contentPos.x
			if (!sr.horizontal) endPos.x = contentPos.x;

			// If no vertical scroll, then don't change contentPos.y
			if (!sr.vertical) endPos.y = contentPos.y;

			sr.content.position = sr.transform.TransformPoint(endPos);

			Vector2 goalPos = sr.normalizedPosition;

			goalPos.x = Mathf.Clamp01(goalPos.x);
			goalPos.y = Mathf.Clamp01(goalPos.y);

			sr.normalizedPosition = goalPos;
		}
    }
}

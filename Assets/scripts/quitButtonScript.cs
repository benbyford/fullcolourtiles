using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class quitButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public GameObject QuitText;

	// Use this for initialization
	void Start () {

		// set event listener for clickevent
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(Clicked);
	}

	void Clicked(){
		Application.Quit();
	}

	public void OnPointerEnter(PointerEventData eventData){
		QuitText.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData){
		QuitText.SetActive(false);
	}
}

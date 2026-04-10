using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Linq.Expressions;
using System.Transactions;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiController : MonoBehaviour//, IPointerDownHandler
{
    
    [SerializeField] EventSystem eventSystem;
    [SerializeField] PlayerInput input;
    [SerializeField] grid gameController;
    public GameObject pointer;
    [SerializeField] GameObject currentlySelected;
    [SerializeField] GameState currentState;
    public bool controllerUsed = false;
    [SerializeField] scrollHelper menuScrollHelper;
    [SerializeField] LongHoldButton menuButtons;
    [SerializeField] LongHoldButton reloadButtons;
    [SerializeField] homeButton homeButton;
    [SerializeField] float cursorTimerOut = 100f;
    [SerializeField] Camera cam;

    [SerializeField] InputAction holdAction;

    [Header("buttons")]
    [SerializeField] GameObject btnMenu;
    [SerializeField] GameObject btnEndOfLevel;
    [SerializeField] GameObject btnCurrentLevel;
    [SerializeField] GameObject btnLevelMenu;
    [SerializeField] GameObject btnMainMenuPlay;
    [SerializeField] GameObject btnEndOfGame;
    [SerializeField] GameObject btnScore;
    [SerializeField] GameObject btnData;
    [SerializeField] GameObject btnLang;
    [SerializeField] GameObject btnCredits;
    [SerializeField] GameObject btnQuit;

    bool isHeldPressed;
    float heldPressLength;

    public bool scrolling;

    List<RaycastResult> rayResults = new();

    public void stateChange(GameState state)
    {
        currentState = state;

        // buttons to change to on state change in game
        switch (state)
        {
            case GameState.LEVEL:
                currentlySelected = null;

                break;
            case GameState.ENDOFLEVEL:
                currentlySelected = btnEndOfLevel;
        
                break;
            case GameState.ENDOFGAME:
                currentlySelected = btnEndOfGame;
                
                break;
            case GameState.LEVELMENU:
                currentlySelected = btnLevelMenu;
                if(btnCurrentLevel) currentlySelected = btnCurrentLevel;

                break;
            case GameState.MAINMENU:
                currentlySelected = btnMainMenuPlay;
                
                break;
            case GameState.CREDITSMENU:
                currentlySelected = btnCredits;
                
                break;
            case GameState.DATAMENU:
                currentlySelected = btnData;
                
                break;
            case GameState.LANGMENU:
                currentlySelected = btnLang;
                
                break;
            case GameState.SCOREMENU:
                currentlySelected = btnScore;
                
                break;

        }
        StartCoroutine(ChangeCurrentBtn(currentlySelected));
    }

    const float SCALECHANGE = 0.15f;
    Vector3 scale = new Vector3(SCALECHANGE,SCALECHANGE,SCALECHANGE);
    bool throbBigger = true;
    bool moving = true;
    float timer = 0;

    private void Update()
    {
        // constant throb
        if (!moving && controllerUsed){
            if(throbBigger && currentlySelected){
                if (currentlySelected.CompareTag("menu") || currentlySelected.CompareTag("longbutton")) // Attach menu tag to any button that needs a smaller circle, but doesn't need to be held by controller
                {
                    if (pointer.transform.localScale.magnitude > 1.4)
                    {
                        throbBigger = false;
                    }
                    pointer.transform.localScale += scale * Time.deltaTime;
                } else if (currentlySelected.CompareTag("bigButton"))
                {
                    if (pointer.transform.localScale.magnitude > 2.4)
                    {
                        throbBigger = false;
                    }
                    pointer.transform.localScale += scale * Time.deltaTime;
                } else
                {
                    if (pointer.transform.localScale.magnitude > 1.8)
                    {
                        throbBigger = false;
                    }
                    pointer.transform.localScale += scale * Time.deltaTime;
                }
            }else if (currentlySelected){
                if (currentlySelected.CompareTag("menu") || currentlySelected.CompareTag("longbutton"))
                {
                    if (pointer.transform.localScale.magnitude < 1)
                    {
                        throbBigger = true;
                    }
                    pointer.transform.localScale -= scale * Time.deltaTime;
                }
                else if (currentlySelected.CompareTag("bigButton"))
                {
                    if (pointer.transform.localScale.magnitude < 1.9)
                    {
                        throbBigger = true;
                    }
                    pointer.transform.localScale -= scale * Time.deltaTime;
                } else
                {
                    if (pointer.transform.localScale.magnitude < 1.5)
                    {
                        throbBigger = true;
                    }
                    pointer.transform.localScale -= scale * Time.deltaTime;
                }
            }
        } else if (!controllerUsed)
        {
            Image pointImg = pointer.GetComponent<Image>();
            pointImg.enabled = false;
        }

        // time out cursor
        if(timer > cursorTimerOut){
            pointer.SetActive(false);
        }
        else{
            timer += Time.deltaTime;
        }
    }

    public void SetCurrentLevelBtn(GameObject btn)
    {
        btnCurrentLevel = btn;
    }

    void OnCancel()
    {
        //if (currentState == GameState.LEVEL) menuButtons.BackToMenu();
        Escape();
    }

    void OnSubmit() => Clicked();

    //void OnPointer() => Clicked();

    //void OnEscape() => Escape();
    void OnRestart() => Restart();

    //public void OnPointerDown(PointerEventData eventData)
    void OnClick() // Allows mouse control for clicking tiles
    {
        //Debug.Log("Pointer Down " + eventData);
#if UNITY_EDITOR
        Debug.Log("Clicked");
#endif

        if (gameController.inputAllowed)
        {
            Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());

#if UNITY_EDITOR
            Debug.Log("Raycast hit on click: " + ray);
#endif

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("tile"))
                {
#if unity_editor
                    Debug.Log("Setting current selection to: " + hit.transform.gameObject + " From: " + currentlySelected);
#endif
                    if (currentlySelected) // Hides the highlighting and pointer used for controller/keyboard controls
                    {
                        controllerUsed = false;
                        currentlySelected.GetComponent<tile_data>().Deselect();
                    }

                    //StartCoroutine(ChangeCurrentBtn(hit.transform.gameObject));

                    Image pointImg = pointer.GetComponent<Image>();
#if UNITY_EDITOR
                    Debug.Log("Hiding Poiner. Is it valid? " + pointImg);
#endif

                    pointImg.enabled = false;

                    gameController.TitleHit( // Communicates which tile has been clicked
                        hit.collider.gameObject,
                        hit.collider.gameObject.transform.position,
                        false
                    );
                }
            }
        }
    }

    void OnNavigate(InputValue value)
    {
        if (eventSystem.currentSelectedGameObject == null) stateChange(currentState);

        pointer.SetActive(true);

        pointer.GetComponent<Image>().enabled = true;

        if (currentlySelected)
        {
            if (currentlySelected.CompareTag("levelTile"))
            {
                if (!scrolling)
                {
                    //menuScrollHelper.ScrollSelected(value.Get<Vector2>().y);
                    RectTransform currentRect = currentlySelected.GetComponent<RectTransform>();
                }
            }
        }

        if (!controllerUsed)
        {
            controllerUsed = true;
        }

        // move pointer
        timer = 0f;

        // deselect sprites
        if (currentlySelected)
        {
            if (currentlySelected.CompareTag("tile"))
            {
                currentlySelected.GetComponent<tile_data>().Deselect();
            }
        }
#if UNITY_EDITOR
        Debug.Log("Currently Selected Game Object pre-set: " + currentlySelected);
#endif
        currentlySelected = eventSystem.currentSelectedGameObject;
#if UNITY_EDITOR
        Debug.Log("Currently selected game object post-set: " + currentlySelected);
#endif

        if (currentlySelected && gameController.inputAllowed)
            if (currentlySelected.CompareTag("tile"))
                currentlySelected.GetComponent<tile_data>().Select();


        moving = true;

        pointer.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.1f);

#if UNITY_EDITOR
        Debug.Log("Currently Selected in OnNavigate: " + currentlySelected);
#endif
        if (currentlySelected)
        {
            pointer.transform.DOMove(
                currentlySelected.transform.position, // currentlySelected occassionally goes null despite the code above setting it if it's null
                0.25f
                ).OnComplete(() =>
                {
                    pointer.transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.1f).OnComplete(() =>
                    {
                        moving = false;
                    });
                }
            );
        } else
        {
            stateChange(currentState); // hopefully this'll fix the random null errors
        }
    }

    void Clicked()
    {
        if (eventSystem.currentSelectedGameObject == null) stateChange(currentState);

        // only do click if tile in level
        if (eventSystem.currentSelectedGameObject.CompareTag("tile") && gameController.inputAllowed)
        {
            gameController.TitleHit(
                eventSystem.currentSelectedGameObject,
                eventSystem.currentSelectedGameObject.transform.position,
                false
            );
        }

    }
    void Restart()
    {
        if(currentState == GameState.LEVEL) reloadButtons.Reload();
    }

    void Escape()
    {
        switch (currentState)
        {
            case GameState.LEVEL:
                menuButtons.BackToMenu();
                break;
            case GameState.LEVELMENU:
                homeButton.Clicked();
                break;
            case GameState.MAINMENU:
#if UNITY_STANDALONE || UNITY_EDITOR
                StartCoroutine(ChangeCurrentBtn(btnQuit));
#endif
                break;
        }
    }

    public void UpdateCurrentSelected(GameObject obj){
        eventSystem.SetSelectedGameObject(obj);
    }

    public IEnumerator ChangeCurrentBtn(GameObject btn) {
		
        yield return new WaitForSeconds(0.3f);

        // if nothing provided to select then find a tile (hopefully in the level)
        if (btn == null)
        {

            GameObject[] btns = GameObject.FindGameObjectsWithTag("tile"); // need a fallback on any other screen other than a level

            for (int i = 0; i < btns.Length; i++)
            {
                if (btns[i].GetComponent<SpriteRenderer>().sprite.name == "octogon") // octogons shouldn't be selectable.
                {
                  // do nothing and loop again
                } else
                {
                    btn = btns[i];
                    break;
                }
            }

        }
        // update currently seleted
        this.UpdateCurrentSelected(btn);

        if (controllerUsed)
        {
            if (!pointer)
                pointer.SetActive(true);

            Image pointImg = pointer.GetComponent<Image>();
            pointImg.enabled = true;

            pointer.transform.position = btn.transform.position;
        }
    }
}

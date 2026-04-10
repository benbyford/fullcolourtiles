using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class reloadButtonClick : MonoBehaviour {

    public grid grid;

    [SerializeField] Image progressBar;
    [SerializeField] float progressBarTime;
    [SerializeField] float holdTime;
    [SerializeField] float maxTime;

    [SerializeField] bool holding = false;
    [SerializeField] bool depressing = false;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] EventSystem eventSystem;

    InputAction pressGamepad;

    // I think this might break if someone tries to quickly keep swapping between m/kb/gamepad constantly while interacting with the buttons. 

    void Start()
    {

        // Find the input action so it can be used
        pressGamepad = playerInput.actions.FindAction("GamepadPress");

        // Make sure the progress bar is empty
        progressBar.fillAmount = 0f;
    }

    void Update()
    {
        // Allows gamepad and keyboard controls
        if (eventSystem.currentSelectedGameObject)
        {
            if (pressGamepad.WasPressedThisFrame() && eventSystem.currentSelectedGameObject.CompareTag("longbutton"))
                PressButton();
            if (pressGamepad.WasReleasedThisFrame() && eventSystem.currentSelectedGameObject.CompareTag("longbutton"))
                StopButton();
        }

        // Start and stop timers
        if (holding && !depressing)
            holdTime += Time.deltaTime;
        else if (depressing && !holding && holdTime >= 0f) // This has an extra check as it needs to stop at 0 as the game's state isn't changing
            holdTime -= Time.deltaTime;
        else
        { // Reset timers and bar just incase
            holdTime = 0f;
            progressBarTime = 0f;
            progressBar.fillAmount = 0f;
            depressing = false;
        }
    }

    // Mouse and touch input events
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down");
        PressButton();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer up");
        StopButton();
    }

    // These are seperate to make sure all input methods can call the same code without too much duplication
    void PressButton()
    {
        StartCoroutine(HoldButton());
        StopCoroutine(DepressButton());
    }
    void StopButton()
    {
        StartCoroutine(DepressButton());
        StopCoroutine(HoldButton());
    }

    public void Clicked()
    {

        // Reset everything
        holdTime = 0f;
        progressBarTime = 0f;
        progressBar.fillAmount = 0f;
        depressing = false;
        holding = false;


        #if (UNITY_EDITOR)
        Debug.Log("reload level");
        #endif

        // only allow click if input allowed
        if (grid.inputAllowed)
        {

            grid.restartLevel();
            gameObject.GetComponent<Animator>().Play("rotate360");

            // level start sound
            grid.audioPlayer.playAudioClip(2);
        }
    }

    // Coroutines to handle progression bar changing to stop infinite while loops.
    IEnumerator HoldButton()
    {
        // Start counting up in delta time, progress progress bar by held time x2, trigger Clicked() to progress
        holding = true;
        depressing = false;

        while (holdTime <= maxTime)
        {
            if (!holding) // Break out of the loop if player stops holding the button
                break;
            progressBarTime = holdTime * 2;
            progressBar.fillAmount = progressBarTime;
            yield return new WaitForEndOfFrame();
        }

        if (holdTime >= maxTime) // If the button has been held for long enough, progress
            Clicked();
    }

    IEnumerator DepressButton()
    {
        // Take current progress bar and decrease by same amount to make it go back down.
        holding = false;
        depressing = true;

        while (holdTime >= 0)
        {
            if (holding) // Break out of the loop if player stops holding the button
                break;
            progressBarTime = (holdTime * 2) / 2; // Does a slight jump downwards when the player stops holding the button, might need different maths?
            progressBar.fillAmount = progressBarTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRKeys;

public class NoteTakingTool : MonoBehaviour
{
    public GameObject keyboardPrefab;
    public MenuManager menu;

    public TMP_InputField inputField;
    public Transform keyboardSpawnTransform;

    public List<GameObject> pageList = new List<GameObject>();
    private int activePageIndex;
    public TextMeshProUGUI pageNumberText;

    public GameObject saveAcknowledgement;

    private GameObject keyboard;
    private GrabbableKeyboard grabbableKeyboard;

    private GameObject leftHand;
    private GameObject rightHand;


    private void Start()
    {
        leftHand = menu.inputManager.controllerLeft.controller;
        rightHand = menu.inputManager.controllerRight.controller;

        // initialize keyboard
        keyboard = Instantiate(keyboardPrefab, keyboardSpawnTransform.position, keyboardSpawnTransform.rotation);
        Keyboard keyboardScript = keyboard.GetComponent<Keyboard>();
        grabbableKeyboard = keyboard.GetComponentInChildren<GrabbableKeyboard>(true);
        keyboardScript.displayText = inputField;
        keyboardScript.leftHand = leftHand;
        keyboardScript.rightHand = rightHand;
        inputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return keyboard.GetComponent<Keyboard>().MyValidate(addedChar); };
        keyboard.transform.parent = gameObject.transform;

        pageList[0].GetComponent<NoteTakingToolPage>().inputField.text = PlayerPrefs.GetString("SavedPage1", "");
        pageList[1].GetComponent<NoteTakingToolPage>().inputField.text = PlayerPrefs.GetString("SavedPage2", "");
        pageList[2].GetComponent<NoteTakingToolPage>().inputField.text = PlayerPrefs.GetString("SavedPage3", "");
        pageList[3].GetComponent<NoteTakingToolPage>().inputField.text = PlayerPrefs.GetString("SavedPage4", "");
    }

    private void OnEnable()
    {
        if (keyboard != null)
        {
            keyboard.SetActive(true);
            grabbableKeyboard.ReattatchKeyboard();
        }
    }
    private void OnDisable()
    {
        if (keyboard != null)
        {
            keyboard.SetActive(false);
        }
    }

    public void SaveButton()
    {
        PlayerPrefs.SetString("SavedPage1", pageList[0].GetComponent<NoteTakingToolPage>().inputField.text);
        PlayerPrefs.SetString("SavedPage2", pageList[1].GetComponent<NoteTakingToolPage>().inputField.text);
        PlayerPrefs.SetString("SavedPage3", pageList[2].GetComponent<NoteTakingToolPage>().inputField.text);
        PlayerPrefs.SetString("SavedPage4", pageList[3].GetComponent<NoteTakingToolPage>().inputField.text);
        saveAcknowledgement.SetActive(true);
        Invoke("DisableSavedAck", 1);
    }

    private void DisableSavedAck()
    {
        saveAcknowledgement.SetActive(false);
    }

    public void PageUp()
    {
        if (activePageIndex < pageList.Count - 1)
        {
            pageList[activePageIndex].SetActive(false);

            activePageIndex++;
            pageList[activePageIndex].SetActive(true);

            pageNumberText.text = "Page " + (activePageIndex + 1) + "/4";

            pageList[activePageIndex].GetComponent<NoteTakingToolPage>().inputField.ForceLabelUpdate();
            keyboard.GetComponent<Keyboard>().displayText = pageList[activePageIndex].GetComponent<NoteTakingToolPage>().inputField;
        }
    }

    public void PageDown()
    {
        if (activePageIndex > 0)
        {
            pageList[activePageIndex].SetActive(false);

            activePageIndex--;
            pageList[activePageIndex].SetActive(true);

            pageNumberText.text = "Page " + (activePageIndex + 1) + "/4";

            pageList[activePageIndex].GetComponent<NoteTakingToolPage>().inputField.ForceLabelUpdate();
            keyboard.GetComponent<Keyboard>().displayText = pageList[activePageIndex].GetComponent<NoteTakingToolPage>().inputField;
        }
    }

    public void ResetKeyboardTransform()
    {
        keyboard.transform.position = keyboardSpawnTransform.position;
        keyboard.transform.rotation = keyboardSpawnTransform.rotation;
    }
}

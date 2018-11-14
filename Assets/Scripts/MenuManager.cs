using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public delegate void MenuButtonEvent();
    public event MenuButtonEvent OnBackButtonPressed;
    public event MenuButtonEvent OnCloseButtonPressed;
    public event MenuButtonEvent OnScrollLeftPressed;
    public event MenuButtonEvent OnScrollRightPressed;

    public List<Transform> menuButtonTransforms; //list of transforms attached to menu in correct location for buttons to be

    public GameObject imageButtonPrefab;
    public List<GameObject> imageButtonPool = new List<GameObject>();
    public GameObject imageButtonParentPage;
    public Transform exampleButtonRotation;

    public GameObject scrollButtonLeft;
    public GameObject scrollButtonRight;

    public AvatarArtAndCharacterData avatarArt;
    public InputManager inputManager;
    public SelectionTool selectionTool;    

    public GameObject statText;
    public GameObject statTextPanel;

    public int storedCharacterIndex;
    public string storedCharacterUsername;  //used for Roll functionality as well as Character Stats
    public int storedAbilityIndex;

    public GameObject modMenuButtons;  //DM only menu objects
    public GameObject npcStatButton;
    public RollTool rollTool;

    public EnvironmentMenuPage environmentMenuPage;


    void Start()
    {
        CreateImageButtonPool();

        LoadCharacterDataFromJson();

        InitializeMenuComponents();
    }

    private void CreateImageButtonPool()
    {
        for (int i = 0; i < menuButtonTransforms.Count; i++)
        {
            GameObject button = Instantiate(imageButtonPrefab, menuButtonTransforms[i].position, exampleButtonRotation.rotation);
            button.transform.SetParent(imageButtonParentPage.transform);
            imageButtonPool.Add(button);
            button.SetActive(false);
        }
    }

    private void LoadCharacterDataFromJson()
    {
        foreach (GameObject characterPrefab in avatarArt.characterPrefabsList)
        {
            characterPrefab.GetComponent<Character>().LoadCharacterData();
        }
    }

    private void InitializeMenuComponents()
    {
        IInitializable[] initializables = GetComponentsInChildren<IInitializable>(true);
        foreach (IInitializable initializable in initializables)
        {
            initializable.Initialize();
        }
    }

    public void InitializeMenuForHost()
    {
        modMenuButtons.SetActive(true);
        
        rollTool.dmRollPermissionButton.SetActive(true);  
        rollTool.dmRollPermissionButtonIsOn = true;

        rollTool.SetButtonColor(rollTool.dmRollPermissionButton.GetComponent<Image>(), rollTool.rollButtonsSelectColor);

        npcStatButton.SetActive(true);

    }

    public void BackButton()
    {
        if (OnBackButtonPressed != null)
        {
            OnBackButtonPressed();
        }
    }

    public void CloseMenu()
    {
        if (OnCloseButtonPressed != null)
        {
            OnCloseButtonPressed();
        }
        gameObject.SetActive(false);
    }

    public void ScrollRight()
    {
        if (OnScrollRightPressed != null)
        {
            OnScrollRightPressed();
        }
    }

    public void ScrollLeft()
    {
        if (OnScrollLeftPressed != null)
        {
            OnScrollLeftPressed();
        }
    }

    public void SetScrollButtonsActive(bool isActive)
    {
        scrollButtonLeft.SetActive(isActive);
        scrollButtonRight.SetActive(isActive);
    }
}
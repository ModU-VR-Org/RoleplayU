using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPageBase : MonoBehaviour
{
    public MenuManager menuManager;
    public GameObject parentMenuPage;

    [System.Serializable]
    public class ChildButtonActivationLink
    {
        public Button button;
        public GameObject menuPage;
    }
    [SerializeField]
    public List<ChildButtonActivationLink> childMenuPages;

    //Multi-page buttons are located in MenuManager
   

    //Connect buttons to what they should activate OnClick
    public virtual void Start()
    {
        foreach (ChildButtonActivationLink childPage in childMenuPages)
        {
            childPage.button.onClick.AddListener(delegate { ActivateChild(childPage.menuPage); });
        }
    }


    public virtual void ActivateChild(GameObject child)
    {
        child.SetActive(true);
        gameObject.SetActive(false);
    }

    public virtual void OnEnable()
    {
        menuManager.OnBackButtonPressed += BackButton;
        menuManager.OnScrollLeftPressed += ScrollLeft;
        menuManager.OnScrollRightPressed += ScrollRight;
        menuManager.OnCloseButtonPressed += CloseButton;
    }
    public virtual void OnDisable()
    {
        menuManager.OnBackButtonPressed -= BackButton;
        menuManager.OnScrollLeftPressed -= ScrollLeft;
        menuManager.OnScrollRightPressed -= ScrollRight;
        menuManager.OnCloseButtonPressed -= CloseButton;
    }

    public virtual void BackButton()
    {
        if(parentMenuPage != null)
        {
            parentMenuPage.SetActive(true);
            gameObject.SetActive(false);
        }   
    }

    public virtual void ScrollLeft()
    {
    }

    public virtual void ScrollRight()
    {
    }

    public virtual void CloseButton()
    {
    }
}

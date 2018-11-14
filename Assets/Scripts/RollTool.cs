using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RollTool : MonoBehaviour, IInitializable
{
    private RollMenuPage rollMenuPage;
    
    public GameObject rollToolButton;
    public GameObject rollPage;
    public TextMeshProUGUI rollsCacheText;
    private string[] rollsCacheStrings = new string[9];
    private int maxNumOfDice = 15;

    private GameObject selectedDice; //runtime stored button
    private int selectedRollValue = 20;
    private int numOfDice = 1;
    public TextMeshProUGUI numOfDiceText;

    public Color rollButtonsSelectColor;
    public Color rollButtonUnselectedColor;

    private GameObject selectedModNum; //runtime stored button
    private int selectedModValue = 0;
    private int modNumSignage = 1;

    public GameObject modNumPositiveButton;
    public GameObject modNumNegativeButton;
    public GameObject dmRollPermissionButton;

    public bool dmRollPermissionButtonIsOn;

    public void Initialize()
    {
        rollMenuPage = GetComponent<RollMenuPage>();
    }

    public void SubmitRoll()
    {
        int rollAmount = 0;
        for (int i = 0; i < numOfDice; i++)
        {
            int randomDie = Random.Range(1, selectedRollValue + 1); //+1 because max is exclusive
            rollAmount += randomDie;
        }

        rollAmount += (selectedModValue * modNumSignage);

        //append roll value to string, remove oldest roll
        for (int i = rollsCacheStrings.Length - 1; i > 0; i--)
        {
            rollsCacheStrings[i] = rollsCacheStrings[i - 1]; 
        }

        rollsCacheStrings[0] = rollAmount.ToString();

        rollsCacheText.text = System.String.Join(" | ", rollsCacheStrings); // if efficieny needed use: text = rollsCacheStrings[0] + " | " + rollsCacheStrings[1] + ...

        bool d20Selected = false;
        if(selectedRollValue == 20)
        {
            d20Selected = true;
        }
        CmdShowRoll(rollMenuPage.menuManager.storedCharacterUsername, rollAmount, selectedModValue, d20Selected);
    }

    public void CmdShowRoll(string selectedPlayerUsername, int value, int modValue, bool d20Selected)
    {
        if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["rollPermission"] || Utilities_NET.LocalPlayerIsDM)
        {
            GameObject targetAvatar = Utilities_NET.GetPlayerAvatar(selectedPlayerUsername);
            targetAvatar.GetComponent<PhotonView>().RPC("RpcShowRoll", RpcTarget.All, value, modValue, d20Selected);
        }
    }

    public void ChangeSelectedRoll(int dice, GameObject button) //activated by pressing 
    {
        if (selectedDice != null)
        {
            SetButtonColor(selectedDice.GetComponent<Image>(), Color.white);
        }

        selectedDice = button;
        selectedRollValue = dice;

        SetButtonColor(selectedDice.GetComponent<Image>(), rollButtonsSelectColor);
    }

    public void ChangeSelectedModNum(int modNum, GameObject button)
    {
        //Make previous selection white
        if (selectedModNum != null) 
        {
            SetButtonColor(selectedModNum.GetComponent<Image>(), Color.white);
        }

        //Make new selection rollButtonsSelectColor
        SetButtonColor(button.GetComponent<Image>(), rollButtonsSelectColor);

        //Set new selection
        selectedModNum = button;
        selectedModValue = modNum;
    }

    public void ChangeModNumSignage(bool positive)
    {
        if (positive)
        {
            modNumSignage = 1;
            SetButtonColor(modNumPositiveButton.GetComponent<Image>(), rollButtonsSelectColor);
            SetButtonColor(modNumNegativeButton.GetComponent<Image>(), Color.white);
        }
        else
        {
            modNumSignage = -1;
            SetButtonColor(modNumPositiveButton.GetComponent<Image>(), Color.white);
            SetButtonColor(modNumNegativeButton.GetComponent<Image>(), rollButtonsSelectColor);
        }
    }

    public void ChangeDiceNum(bool increase)
    {
        if (increase)
        {
            numOfDice++;
            if (numOfDice > maxNumOfDice)
            {
                numOfDice = maxNumOfDice;
            }
        }
        else
        {
            numOfDice--;
            if (numOfDice < 1)
            {
                numOfDice = 1;
            }
        }
        numOfDiceText.text = numOfDice.ToString();
    }

    public void SetButtonColor(Image buttonImage, Color color)
    {
        var newButtonColor = buttonImage.color;
        newButtonColor = color;
        buttonImage.color = newButtonColor;
    }

    public void InitializeRollButtonColors()
    {
        var newColor = modNumPositiveButton.GetComponent<Image>().color;
        newColor = rollButtonsSelectColor;
        modNumPositiveButton.GetComponent<Image>().color = newColor;
    }
    public void DMRollButton()
    {
        CmdPlayerRollsPermissionChange();

        if (dmRollPermissionButtonIsOn)
        {
            dmRollPermissionButtonIsOn = false;
            SetButtonColor(dmRollPermissionButton.GetComponent<Image>(), Color.white);
        }
        else
        {
            dmRollPermissionButtonIsOn = true;
            SetButtonColor(dmRollPermissionButton.GetComponent<Image>(), rollButtonsSelectColor);
        }
    }

    public void CmdPlayerRollsPermissionChange()
    {
        if (Utilities_NET.LocalPlayerIsDM) 
        {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["rollPermission"])
            {
                Hashtable hash = new Hashtable();
                hash.Add("rollPermission", false);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash, null, null);
            }
            else
            {
                Hashtable hash = new Hashtable();
                hash.Add("rollPermission", true);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash, null, null);
            }
        }
    }
}

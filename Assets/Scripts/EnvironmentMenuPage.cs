using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class EnvironmentMenuPage : MenuPageBase 
{
    public List<Enviro> environments;
    public GameObject currentEnvironment;

    public override void OnEnable()
    {
        base.OnEnable();
        ShowEnvironmentButtons();
    }

   void ShowEnvironmentButtons()
    {
        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            if(i >= environments.Count)
            {
                return;
            }
            menuManager.imageButtonPool[i].SetActive(true);
            menuManager.imageButtonPool[i].transform.SetParent(transform);

            menuManager.imageButtonPool[i].GetComponentInChildren<TextMeshProUGUI>().text = environments[i].environmentName;
            menuManager.imageButtonPool[i].GetComponent<ImageButton>().lowerText.text = "";
            menuManager.imageButtonPool[i].GetComponent<ImageButton>().image.sprite = null;

            int index = i;
            menuManager.imageButtonPool[i].GetComponent<Button>().onClick.RemoveAllListeners();
            menuManager.imageButtonPool[i].GetComponent<Button>().onClick.AddListener(delegate { CmdChangeEnvironment(index); });
        }
    }

    public void CmdChangeEnvironment(int index)
    {
        GameObject localPlayerAvatar = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
        localPlayerAvatar.GetComponent<PhotonView>().RPC("RpcChangeEnvironment", RpcTarget.All, index);
    }

    public void ChangeEnvironment(int index)
    {
        Debug.Log(index);
        //TODO: cut to black
        Destroy(currentEnvironment);
        currentEnvironment = Instantiate(environments[index].environment, environments[index].environment.transform.position, Quaternion.identity);      
    }

}

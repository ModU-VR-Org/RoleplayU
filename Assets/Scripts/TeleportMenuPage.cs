using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TeleportMenuPage : MenuPageBase
{
    public GameObject teleportPlayerButtonsPrefab; // prefab for the menu item that has player name and buttons for DM to select movement options
    public List<Transform> teleportPlayerMenuTransforms; //list of transforms attached to menu in correct location for buttons to be
    private List<GameObject> teleportMenuButtonPool = new List<GameObject>();

    public override void OnEnable()
    {
        base.OnEnable();
        InitializeTeleportMenu();
        TeleportToolActivate();
    }

    public void InitializeTeleportMenu()
    {
        // Teleport buttons Object Pool
        for (int i = 0; i < teleportPlayerMenuTransforms.Count; i++)
        {
            GameObject teleportPlayerMenuObject = Instantiate(teleportPlayerButtonsPrefab, teleportPlayerMenuTransforms[i].position, transform.rotation);
            teleportPlayerMenuObject.transform.SetParent(gameObject.transform);
            teleportMenuButtonPool.Add(teleportPlayerMenuObject);
        }
    }

    public void TeleportToolActivate()
    {
        menuManager.SetScrollButtonsActive(false);
        //TODO: CURRENTLY DOES NOT HANDLE MORE THAN 4 PLAYERS, DOES NOT REFRESH (list of players) WITHOUT BACKING OUT OF MENU.
        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < teleportMenuButtonPool.Count; i++)
        {
            if (i < playerList.Length)
            {
                int actorNumber = playerList[i].ActorNumber;
                string username = (string)playerList[i].CustomProperties["username"];
                teleportMenuButtonPool[i].GetComponent<TeleportMenuButtons>().InitializeTeleportMenuButtons(actorNumber, username);
                teleportMenuButtonPool[i].SetActive(true);
            }
            else
            {
                teleportMenuButtonPool[i].SetActive(false);
            }
        }
    }

    public void UpdateTeleportMenu(int actorId)
    {
        foreach (var menuItem in teleportMenuButtonPool)
        {
            if (menuItem.GetComponent<TeleportMenuButtons>().actorId == actorId)
            {
                menuItem.GetComponent<TeleportMenuButtons>().DeactivateSingleTeleportToggle();
            }
        }
    }
}
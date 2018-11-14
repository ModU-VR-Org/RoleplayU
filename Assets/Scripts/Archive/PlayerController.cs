using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//Handles player controller input locally, triggers functionality on the local Avatar

public class PlayerController : MonoBehaviourPunCallbacks
{
    //public GameObject controllerL;
    //public GameObject controllerR;
    //public Transform headTransform;
    //public Transform handTransformL;
    //public Transform handTransformR;

    //public GameObject avatarPrefab;

    //private GameObject localAvatar;
    //private Avatar_NET localAvatarController;
    //private GameObject targetAvatar; //used for CmdShowRoll

    ////Menu Variables
    //public bool insideMenuSpawnCollider = false;
    //public GameObject menuPrefab;

    //public Transform menuOffset;
    //private bool menuIsHeld = false;
    //public bool handCollidingWithMenu = false;
    //public bool insideKeyboardCollider = false;
    //private bool keyboardIsHeld = false;

    //public NoteTakingTool noteTakingTool;
    //public GameObject menu; //REFACT: remove this later to identify where depencies for this are
    //private MenuManager menuManager;

    ////Moving spawned Objects
    //public bool spawnedObjectHeld = false;
    //public GameObject selectedObject;
    //public int spawnedModelIndex;  

    ////make private later
    //public GameObject grippedGameObjectL;
    //public bool objectIsGrippedL = false;
    //public GameObject grippedGameObjectR;
    //public bool objectIsGrippedR = false;
    //public bool RPCtest = false;

    //public MeasuringTool measuringTool;
    //public SteamVR_LaserPointer laserPointerScript;

    //public GameObject spawnedMeasureLine;
    //private LineRenderer measureLineRenderer;
    //private MeasureLine measureLine;

    ////Teleport
    ////public int playerConnectionId;
    //public GameObject teleportVisual;
    //private GameObject lastPlayerTeleported;
    //private bool playerTeleportStatus;   
    
    
    //void Start()
    //{
    //    InitializeMenu();

    //    SubscribeToInputDelegates();

    //    InitializeAvatar();
    //}    


    //private void InitializeMenu()
    //{
    //    //Menu Setup
    //    GameObject obj = Instantiate(menuPrefab);
    //    menu = obj;
    //    menu.SetActive(false);

    //    menuManager = menu.GetComponent<MenuManager>();
    //    //TODOTEST menuManager.menuCollisionScript.playerController = this;
    //    menuManager.playerController = this;

    //    //noteTakingTool = menuManager.noteTakingTool;
    //}

    //private void SubscribeToInputDelegates()
    //{
    //    ////Input Delegate Subscribing
    //    controllerR.GetComponent<SteamVR_TrackedController>().TriggerClicked += OnTriggerClicked;
    //    controllerR.GetComponent<SteamVR_TrackedController>().TriggerUnclicked += OnTriggerUnClicked;
    //    controllerL.GetComponent<SteamVR_TrackedController>().Gripped += OnGripClickedL;
    //    controllerL.GetComponent<SteamVR_TrackedController>().Ungripped += OnGripUnClickedL;
    //    controllerR.GetComponent<SteamVR_TrackedController>().Gripped += OnGripClickedR;
    //    controllerR.GetComponent<SteamVR_TrackedController>().Ungripped += OnGripUnClickedR;
    //}

    //private void InitializeAvatar()
    //{
    //    localAvatar = PhotonNetwork.Instantiate(avatarPrefab.name, Vector3.zero, Quaternion.identity, 0);
    //    localAvatarController = localAvatar.GetComponent<Avatar_NET>();
    //    localAvatarController.AssignToLocalPlayer(gameObject, headTransform, handTransformL, handTransformR);
    //}

    ////Menu Functions ***

    ////Left Controller
    //void OnGripClickedL(object sender, ClickedEventArgs args)
    //{
    //    GrabLeft();
    //}

    //private void GrabLeft()
    //{
    //    if (insideMenuSpawnCollider) //script on spawn collider changes this bool
    //    {
    //        ActivateAndPositionMenu();
    //        GrabMenu();
    //    }
    //    else if (handCollidingWithMenu) //script menu collider changes this bool
    //    {
    //        GrabMenu();

    //        //if(menuManager.currentPage == MenuManager.ToolPage.noteTakingTool)
    //        //{
    //        //    DetachKeyboard();
    //        //}
    //    }
    //    else if (insideKeyboardCollider)
    //    {
    //        GrabKeyboard();
    //    }
    //    else if (selectedObject != null)
    //    {
    //        GrabObjectL();
    //    }
    //}
   
    //private void ActivateAndPositionMenu()
    //{
    //    menu.SetActive(true);
    //    menu.transform.position = menuOffset.position;
    //    menu.transform.rotation = menuOffset.rotation;
    //}

    //private void GrabMenu()
    //{
    //    menuIsHeld = true;
    //    menu.transform.SetParent(controllerL.transform);
    //}

    //private void DetachKeyboard()
    //{
    //    //Detaches Keyboard, move this functionality long term
    //    if (noteTakingTool != null)
    //    {
    //        noteTakingTool.keyboard.transform.SetParent(null);
    //    }
    //}

    //private void GrabKeyboard()
    //{
    //    keyboardIsHeld = true;
    //    if (noteTakingTool.keyboard != null)
    //    {
    //        noteTakingTool.keyboard.transform.SetParent(controllerL.transform);
    //    }
    //}

    //public void GrabObjectL()
    //{
    //    if (selectedObject.GetComponent<PhotonView>().IsMine)
    //    {
    //        // RPC called from ModelHeldState_NET script
    //        selectedObject.GetComponent<PhotonView>().RPC("SyncHeldState", RpcTarget.All, true);
    //        grippedGameObjectL = selectedObject;
    //        grippedGameObjectL.transform.SetParent(controllerL.transform);
    //        objectIsGrippedL = true;
    //    }
    //}

    //void OnGripUnClickedL(object sender, ClickedEventArgs args)
    //{
    //    UnGrabLeft();
    //}

    //private void UnGrabLeft()
    //{
    //    if (menuIsHeld)
    //    {
    //        UnGrabMenu();
    //    }

    //    if (keyboardIsHeld)
    //    {
    //        UnGrabKeyboard();
    //    }

    //    UnGrabObjectL();
    //}

    //private void UnGrabKeyboard()
    //{
    //    keyboardIsHeld = false;
    //    noteTakingTool.keyboard.transform.SetParent(noteTakingTool.gameObject.transform);
    //}

    //private void UnGrabMenu()
    //{
    //    menuIsHeld = false;
    //    menu.transform.SetParent(null);
    //    //if (menuManager.currentPage == MenuManager.ToolPage.noteTakingTool)
    //    //{
    //    //    noteTakingTool.keyboard.transform.SetParent(noteTakingTool.gameObject.transform);
    //    //}
    //}

    //private void UnGrabObjectL()
    //{
    //    if (objectIsGrippedL)
    //    {
    //        // RPC called from ModelHeldState_NET script
    //        selectedObject.GetComponent<PhotonView>().RPC("SyncHeldState", RpcTarget.All, false);
    //        grippedGameObjectL.transform.SetParent(null);
    //        grippedGameObjectL = null;
    //        objectIsGrippedL = false;
    //    }
    //}

    ////Right Controller
    //private void OnGripClickedR(object sender, ClickedEventArgs args)
    //{
    //    GrabRight();
    //}

    //private void GrabRight()
    //{
    //    if (selectedObject != null)
    //    {
    //        GrabObjectR();
    //    }
    //}

    //private void GrabObjectR()
    //{
    //    if (selectedObject.GetComponent<PhotonView>().IsMine)
    //    {
    //        // RPC called from ModelHeldState_NET script
    //        selectedObject.GetComponent<PhotonView>().RPC("SyncHeldState", RpcTarget.All, true);
    //        objectIsGrippedR = true;
    //        grippedGameObjectR = selectedObject;
    //        grippedGameObjectR.transform.SetParent(controllerR.transform);
    //    }
    //}

    //void OnGripUnClickedR(object sender, ClickedEventArgs args)
    //{
    //    UnGrabRight();
    //}

    //private void UnGrabRight()
    //{
    //    UnGrabObjectR();
    //}

    //public void UnGrabObjectR()
    //{
    //    if (objectIsGrippedR)
    //    {
    //        // RPC called from ModelHeldState_NET script
    //        selectedObject.GetComponent<PhotonView>().RPC("SyncHeldState", RpcTarget.All, false);
    //        grippedGameObjectR.transform.SetParent(null);
    //        grippedGameObjectR = null;
    //        objectIsGrippedR = false;
    //    }
    //}

    //private void OnTriggerClicked(object sender, ClickedEventArgs args)
    //{
    //    //selectedObject set in ClickUIElements script
    //}
    //private void OnTriggerUnClicked(object sender, ClickedEventArgs args)
    //{
    //    //TriggerSpawnUnGrab();
    //}

    ////TODO: delete once this is properly in SpawnTool
    ////public void SpawnLocalPreview(int model, Vector3 pos)
    ////{
    ////    DeselectModel();

    ////    //TODO: change the spawn rotation to a pre-set transform attached to the controllerR
    ////    GameObject previewObject = Instantiate(menuManager.spawnModelPrefabsList[model], pos, controllerR.transform.rotation);
    ////    previewObject.transform.SetParent(controllerR.transform); //when object spawned, it moves with hand until trigger unheld
    ////    selectedObject = previewObject;
    ////    spawnedObjectHeld = true;
    ////    objectIsGrippedR = true;
    ////    spawnedModelIndex = model;
    ////}      
    
    //public void CmdDeleteModel()
    //{
    //    //GetComponent<SaveLoadManager>().modelsSpawned.Remove(selectedObject);
    //    PhotonNetwork.Destroy(selectedObject);
    //}
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasInputManager : BaseInputModule
{
    public bool GeometryBlocksLaser = true;
    public LayerMask LayersThatBlockLaser = Physics.AllLayers;

    public Sprite CursorSprite;
    public Material CursorMaterial;
    public float NormalCursorScale = 0.05f;

    public bool LaserEnabled = true;
    public Color LaserColor = Color.blue;
    public float LaserStartWidth = 0.02f;
    public float LaserEndWidth = 0.001f;

    public bool OnCanvas;
    public bool CanvasUsed;

    private RectTransform[] Cursors;

    private LineRenderer[] Lasers;

    private GameObject[] CurrentPoint;
    private GameObject[] CurrentPressed;
    private GameObject[] CurrentDragging;

    private PointerEventData[] PointEvents;

    private bool Initialized = false;
    private bool DelayedInitialized = false;

    public Camera ControllerCamera;

    public GameObject rightController;

    public Transform pointerDirection;

    private InputManager inputManager;
    private bool buttonDown;
    private bool buttonUp;


    protected override void Start()
    {
        base.Start();

        if (Initialized == false)
        {
            Cursors = new RectTransform[1];
            Lasers = new LineRenderer[Cursors.Length];

            ControllerCamera = new GameObject("Controller UI Camera").AddComponent<Camera>();
            ControllerCamera.transform.parent = this.transform;

            for (int index = 0; index < Cursors.Length; index++)
            {
                GameObject cursor = new GameObject("Cursor for a hand.");
                cursor.transform.parent = this.transform;
                cursor.transform.localPosition = Vector3.zero;
                cursor.transform.localRotation = Quaternion.identity;

                Canvas canvas = cursor.AddComponent<Canvas>();
                cursor.AddComponent<CanvasRenderer>();
                cursor.AddComponent<CanvasScaler>();
                cursor.AddComponent<IgnoreRaycast>();
                cursor.AddComponent<GraphicRaycaster>();

                canvas.renderMode = RenderMode.WorldSpace;
                canvas.sortingOrder = 1000; //set to be on top of everything

                Image image = cursor.AddComponent<Image>();
                image.sprite = CursorSprite;
                image.material = CursorMaterial;

                if (LaserEnabled == true)
                {
                    Lasers[index] = cursor.AddComponent<LineRenderer>();
                    Lasers[index].material = new Material(Shader.Find("Standard"));
                    Lasers[index].material.color = LaserColor;
                    Lasers[index].useWorldSpace = true;
                    Lasers[index].enabled = false;
                }

                if (CursorSprite == null)
                {
                    //Debug.LogError("Set CursorSprite on " + this.gameObject.name + " to the sprite you want to use as your cursor.", this.gameObject);

                }

                Cursors[index] = cursor.GetComponent<RectTransform>();
            }

            CurrentPoint = new GameObject[Cursors.Length];
            CurrentPressed = new GameObject[Cursors.Length];
            CurrentDragging = new GameObject[Cursors.Length];
            PointEvents = new PointerEventData[Cursors.Length];

            inputManager = GetComponentInParent<InputManager>();
            inputManager.controllerRight.OnTriggerClicked += TriggerClicked;
            inputManager.controllerRight.OnTriggerUnClicked += TriggerUnClicked;

            Initialized = true;
        }
    }

    //this is broken up into two steps because of a unity bug. https://issuetracker.unity3d.com/issues/gl-dot-end-error-is-thrown-if-a-cameras-clear-flags-is-set-to-depth-only
    public void DelayedCameraInit()
    {
        ControllerCamera.clearFlags = CameraClearFlags.Nothing;
        ControllerCamera.cullingMask = 0; 
        ControllerCamera.stereoTargetEye = StereoTargetEyeMask.None;
        ControllerCamera.nearClipPlane = 0.01f;

        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.worldCamera = ControllerCamera;
        }

        DelayedInitialized = true;
    }

    // use screen midpoint as locked pointer location, enabling look location to be the "mouse"
    private bool GetLookPointerEventData(int index)
    {
        if (PointEvents[index] == null)
        {
            PointEvents[index] = new PointerEventData(base.eventSystem);
        }
        else
        {
            PointEvents[index].Reset();
        }

        PointEvents[index].delta = Vector2.zero;
        PointEvents[index].position = new Vector2(ControllerCamera.pixelWidth * 0.5f, ControllerCamera.pixelHeight * 0.5f);
        PointEvents[index].scrollDelta = Vector2.zero;

        base.eventSystem.RaycastAll(PointEvents[index], m_RaycastResultCache);
        PointEvents[index].pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        if (PointEvents[index].pointerCurrentRaycast.gameObject != null)
        {
            OnCanvas = true; //gets set to false at the beginning of the process event
            m_RaycastResultCache.Clear();
            return true;
        }
        else
        {
            m_RaycastResultCache.Clear();
            return false;
        }
    }

    // update the cursor location and whether it is enabled
    // this code is based on Unity's DragMe.cs code provided in the UI drag and drop example
    private bool UpdateCursor(int index, PointerEventData pointData)
    {
        bool cursorState = false;

        if (PointEvents[index].pointerCurrentRaycast.gameObject != null && pointData.pointerEnter != null)
        {
            RectTransform draggingPlane = pointData.pointerEnter.GetComponent<RectTransform>();
            Vector3 globalLookPos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, pointData.position, pointData.enterEventCamera, out globalLookPos))
            {
                //do real physics raycast.
                //Vector3 origin = rightController.transform.position;
                Vector3 origin = pointerDirection.position;
                //Vector3 direction = rightController.transform.forward;
                Vector3 direction = pointerDirection.forward;
                Vector3 endPoint = globalLookPos;
                float distance = Vector3.Distance(origin, endPoint);

                bool blockedByGeometry = false;

                if (GeometryBlocksLaser == true)
                {
                    blockedByGeometry = Physics.Raycast(origin, direction, distance, LayersThatBlockLaser);
                }

                if (blockedByGeometry == false)
                {
                    cursorState = true;

                    Cursors[index].position = globalLookPos;
                    Cursors[index].rotation = draggingPlane.rotation;
                    Cursors[index].localScale = Vector3.one * (NormalCursorScale / 100);

                    if (LaserEnabled == true)
                    {
                        Lasers[index].enabled = true;
                        Lasers[index].SetPositions(new Vector3[] { origin, endPoint });
                    }
                }
            }
        }

        Cursors[index].gameObject.SetActive(cursorState);
        return cursorState;
    }

    public void ClearSelection()
    {
        if (base.eventSystem.currentSelectedGameObject)
        {
            base.eventSystem.SetSelectedGameObject(null);
        }
    }

    private void Select(GameObject go)
    {
        ClearSelection();

        if (ExecuteEvents.GetEventHandler<ISelectHandler>(go))
        {
            base.eventSystem.SetSelectedGameObject(go);
        }
    }

    private bool SendUpdateEventToSelectedObject()
    {
        if (base.eventSystem.currentSelectedGameObject == null)
            return false;

        BaseEventData data = GetBaseEventData();

        ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);

        return data.used;
    }

    private void UpdateCameraPosition(int index)
    {
        //ControllerCamera.transform.position = rightController.transform.position;
        ControllerCamera.transform.position = pointerDirection.position;
        //ControllerCamera.transform.forward = rightController.transform.forward;
        ControllerCamera.transform.forward = pointerDirection.forward;
    }

    // Process is called by UI system to process events
    public override void Process() { } 
    private void Update()
    {
        //if (rightControllerDevice == null)
        //{
        //    rightControllerDevice = rightController.GetComponent<DeviceInput>().device;
        //    return;
        //}

        OnCanvas = false;
        CanvasUsed = false;

        if (Initialized == false)
            return;
        if (DelayedInitialized == false)
        {
            DelayedCameraInit();
        }

        // send update events if there is a selected object - this is important for InputField to receive keyboard events
        SendUpdateEventToSelectedObject();

        // see if there is a UI element that is currently being looked at
        for (int index = 0; index < 1; index++)
        {
            if (rightController.gameObject.activeInHierarchy == false )
            {
                if (Cursors[index].gameObject.activeInHierarchy == true)
                {
                    Cursors[index].gameObject.SetActive(false);
                }
                continue;
            }

            UpdateCameraPosition(index);

            bool hit = GetLookPointerEventData(index);

            CurrentPoint[index] = PointEvents[index].pointerCurrentRaycast.gameObject;

            // handle enter and exit events (highlight)
            base.HandlePointerExitAndEnter(PointEvents[index], CurrentPoint[index]);

            // update cursor
            bool cursorActive = UpdateCursor(index, PointEvents[index]);

            if (rightController != null)
            {
                if (buttonDown)
                {
                    ClearSelection();

                    PointEvents[index].pressPosition = PointEvents[index].position;
                    PointEvents[index].pointerPressRaycast = PointEvents[index].pointerCurrentRaycast;
                    PointEvents[index].pointerPress = null;

                    if (CurrentPoint[index] != null)
                    {
                        CurrentPressed[index] = CurrentPoint[index];

                        GameObject newPressed = ExecuteEvents.ExecuteHierarchy(CurrentPressed[index], PointEvents[index], ExecuteEvents.pointerDownHandler);

                        if (newPressed == null)
                        {
                            // some UI elements might only have click handler and not pointer down handler
                            newPressed = ExecuteEvents.ExecuteHierarchy(CurrentPressed[index], PointEvents[index], ExecuteEvents.pointerClickHandler);
                            if (newPressed != null)
                            {
                                CurrentPressed[index] = newPressed;
                            }
                        }
                        else
                        {
                            CurrentPressed[index] = newPressed;
                        }

                        if (newPressed != null)
                        {
                            PointEvents[index].pointerPress = newPressed;
                            CurrentPressed[index] = newPressed;
                            Select(CurrentPressed[index]);
                            CanvasUsed = true;
                        }

                        ExecuteEvents.Execute(CurrentPressed[index], PointEvents[index], ExecuteEvents.beginDragHandler);
                        PointEvents[index].pointerDrag = CurrentPressed[index];
                        CurrentDragging[index] = CurrentPressed[index];
                    }

                    if (CurrentPressed[index])
                    {
                        //todo maybe mouse up and click should go in different places / times?
                        ExecuteEvents.Execute(CurrentPressed[index], PointEvents[index], ExecuteEvents.pointerClickHandler);
                        ExecuteEvents.Execute(CurrentPressed[index], PointEvents[index], ExecuteEvents.pointerUpHandler);
                        PointEvents[index].rawPointerPress = null;
                        PointEvents[index].pointerPress = null;
                        CurrentPressed[index] = null;
                    }

                    buttonDown = false;
                }

                if (buttonUp)
                {
                    if (CurrentDragging[index])
                    {
                        ExecuteEvents.Execute(CurrentDragging[index], PointEvents[index], ExecuteEvents.endDragHandler);
                        if (CurrentPoint[index] != null)
                        {
                            ExecuteEvents.ExecuteHierarchy(CurrentPoint[index], PointEvents[index], ExecuteEvents.dropHandler);
                        }
                        PointEvents[index].pointerDrag = null;
                        CurrentDragging[index] = null;
                    }

                    buttonUp = false;
                }

                // drag handling
                if (CurrentDragging[index] != null)
                {
                    ExecuteEvents.Execute(CurrentDragging[index], PointEvents[index], ExecuteEvents.dragHandler);
                }
            }
        }
    }

    private void TriggerClicked()
    {
        buttonDown = true;
    }

    private void TriggerUnClicked()
    {
        buttonUp = true;
    }

    //private bool ButtonDown(int index)
    //{
    //    return rightControllerDevice.GetHairTriggerDown();
    //}

    //private bool ButtonUp(int index)
    //{
    //    return rightControllerDevice.GetHairTriggerUp();
    //}
}
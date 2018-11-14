using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Teleporter : MonoBehaviour
{
    public TeleporterParabolicPointer Pointer;
    public Transform OriginTransform;
    public Transform HeadTransform;

    public float TeleportFadeDuration = 0.2f;
    public float HapticClickAngleStep = 10;

    [SerializeField]
    private Material FadeMaterial;
    private Material FadeMaterialInstance;
    private int MaterialFadeID;

    public TeleportState CurrentTeleportState { get; private set; }

    public GameObject teleportEffectPrefab;

    public Transform leftControllerPointerTransform;
    public Transform rightControllerPointerTransform;

    private Vector3 startingPoint;
    private Vector3 endingPoint;

    private Vector3 LastClickAngle = Vector3.zero;
    private bool IsClicking = false;

    private bool FadingIn = false;
    private float TeleportTimeMarker = -1;

    private Mesh PlaneMesh;

    private Transform ActiveControllerPointer;
    private InputManager inputManager;
    private bool shouldTeleport;

    public delegate void TeleportationEvent();
    public event TeleportationEvent OnSingleTeleport;

    

    void Start()
    {
        // Disable the pointer graphic (until the user holds down on the touchpad)
        Pointer.enabled = false;

        // Ensure we mark the player as not teleporting
        CurrentTeleportState = TeleportState.None;

        // Standard plane mesh used for "fade out" graphic when you teleport
        // This way you don't need to supply a simple plane mesh in the inspector
        PlaneMesh = new Mesh();
        Vector3[] verts = new Vector3[]
        {
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0)
        };
        int[] elts = new int[] { 0, 1, 2, 0, 2, 3 };
        PlaneMesh.vertices = verts;
        PlaneMesh.triangles = elts;
        PlaneMesh.RecalculateBounds();

        if (FadeMaterial != null)
            FadeMaterialInstance = new Material(FadeMaterial);

        MaterialFadeID = Shader.PropertyToID("_Fade");

        inputManager = GetComponentInParent<InputManager>();

        inputManager.controllerLeft.OnPadClicked += PadClickedLeft;
        inputManager.controllerLeft.OnPadUnClicked += PadUnClickedLeft;

        inputManager.controllerRight.OnPadClicked += PadClickedRight;
        inputManager.controllerRight.OnPadUnClicked += PadUnClickedRight;
    }

    void OnPostRender()
    {
        if (CurrentTeleportState == TeleportState.Teleporting)
        {
            // Perform the fading in/fading out animation, if we are teleporting.  This is essentially a triangle wave
            // in/out, and the user teleports when it is fully black.
            float alpha = Mathf.Clamp01((Time.time - TeleportTimeMarker) / (TeleportFadeDuration / 2));
            if (FadingIn)
            {
                alpha = 1 - alpha;
            }

            Matrix4x4 local = Matrix4x4.TRS(Vector3.forward * 0.3f, Quaternion.identity, Vector3.one);
            FadeMaterialInstance.SetPass(0);
            FadeMaterialInstance.SetFloat(MaterialFadeID, alpha);
            Graphics.DrawMeshNow(PlaneMesh, transform.localToWorldMatrix * local);
        }
    }

    void Update()
    {
        // If we are currently teleporting (ie handling the fade in/out transition)...
        if (CurrentTeleportState == TeleportState.Teleporting)
        {
            if (Pointer.singleTeleport == true)
            {
                Pointer.singleTeleport = false;

                if (OnSingleTeleport != null)
                {
                    OnSingleTeleport();
                }
            }

            // Wait until half of the teleport time has passed before the next event (note: both the switch from fade
            // out to fade in and the switch from fade in to stop the animation is half of the fade duration)
            if (Time.time - TeleportTimeMarker >= TeleportFadeDuration / 2)
            {
                if (FadingIn)
                {
                    // We have finished fading in
                    CurrentTeleportState = TeleportState.None;
                }
                else
                {
                    startingPoint = HeadTransform.position;

                    // We have finished fading out - time to teleport!
                    Vector3 offset = OriginTransform.position - HeadTransform.position;
                    offset.y = 0;
                    OriginTransform.position = Pointer.SelectedPoint + offset;

                    endingPoint = HeadTransform.position;

                    StartCoroutine(TeleportEffect());
                }

                TeleportTimeMarker = Time.time;
                FadingIn = !FadingIn;
            }
        }
        // At this point, we are NOT actively teleporting.  So now we care about controller input.
        else if (CurrentTeleportState == TeleportState.Selecting)
        {
            Debug.Assert(ActiveControllerPointer != null);

            // Here, there is an active controller - that is, the user is holding down on the trackpad.
            // Poll controller for pertinent button data
            if (shouldTeleport) // || shouldCancel)
            {
                // If the user has decided to teleport (ie lets go of touchpad) then remove all visual indicators
                // related to selecting things and actually teleport
                // If the user has decided to cancel (ie squeezes grip button) then remove visual indicators and do nothing
                if (shouldTeleport && Pointer.PointOnNavMesh)
                {
                    // Begin teleport sequence
                    CurrentTeleportState = TeleportState.Teleporting;
                    TeleportTimeMarker = Time.time;
                }
                else
                    CurrentTeleportState = TeleportState.None;

                // Reset active controller, disable pointer, disable visual indicators
                ActiveControllerPointer = null;
                Pointer.enabled = false;

                Pointer.transform.parent = null;
                Pointer.transform.position = Vector3.zero;
                Pointer.transform.rotation = Quaternion.identity;
                Pointer.transform.localScale = Vector3.one;
                shouldTeleport = false;
            }
            else
            {
                // The user is still deciding where to teleport and has the touchpad held down.
                // Note: rendering of the parabolic pointer / marker is done in ParabolicPointer
                Vector3 offset = HeadTransform.position - OriginTransform.position;
                offset.y = 0;

                // Haptic feedback click every [HaptickClickAngleStep] degrees
                if (Pointer.CurrentParabolaAngleY >= 45) // Don't click when at max degrees
                    LastClickAngle = Pointer.CurrentPointVector;

                float angleClickDiff = Vector3.Angle(LastClickAngle, Pointer.CurrentPointVector);
                if (IsClicking && Mathf.Abs(angleClickDiff) > HapticClickAngleStep)
                {
                    LastClickAngle = Pointer.CurrentPointVector;
                    
                }

                // Trigger a stronger haptic pulse when "entering" a teleportable surface
                if (Pointer.PointOnNavMesh && !IsClicking)
                {
                    IsClicking = true;
                    LastClickAngle = Pointer.CurrentPointVector;
                }
                else if (!Pointer.PointOnNavMesh && IsClicking)
                    IsClicking = false;
            }
        }
    }

    private void PadClickedRight()
    {
        if (ActiveControllerPointer == null)
        {
            ActiveControllerPointer = rightControllerPointerTransform;
            PadClicked();
        }
    }

    private void PadUnClickedRight()
    {
        if (ActiveControllerPointer == rightControllerPointerTransform)
        {
            PadUnClicked();
        }
    }

    private void PadClickedLeft()
    {
        if (ActiveControllerPointer == null)
        {
            ActiveControllerPointer = leftControllerPointerTransform;
            PadClicked();
        }
    }

    private void PadUnClickedLeft()
    {
        if (ActiveControllerPointer == leftControllerPointerTransform)
        {
            PadUnClicked();
        }
    }

    private void PadClicked()
    {
        Pointer.transform.parent = ActiveControllerPointer;
        Pointer.transform.localPosition = Vector3.zero;
        Pointer.transform.localRotation = Quaternion.identity;
        Pointer.transform.localScale = Vector3.one;
        Pointer.enabled = true;

        CurrentTeleportState = TeleportState.Selecting;

        Pointer.ForceUpdateCurrentAngle();
        LastClickAngle = Pointer.CurrentPointVector;
        IsClicking = Pointer.PointOnNavMesh;
    }

    private void PadUnClicked()
    {
        shouldTeleport = true;
    }

    IEnumerator TeleportEffect()
    {
        GameObject obj = Instantiate(teleportEffectPrefab);
        obj.GetComponent<LineRenderer>().SetPosition(0, startingPoint);
        obj.GetComponent<LineRenderer>().SetPosition(1, endingPoint);

        yield return new WaitForSeconds(1);

        Destroy(obj);
    }
}

public enum TeleportState
{
    /// The player is not using teleportation right now
    None,
    /// The player is currently selecting a teleport destination (holding down on touchpad)
    Selecting,
    /// The player has selected a teleport destination and is currently teleporting now (fading in/out)
    Teleporting
}
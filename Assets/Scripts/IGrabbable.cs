using UnityEngine;

public interface IGrabbable
{
    bool Grab(GrabbingTool.GrabbingHand hand);
    void UnGrab();
}
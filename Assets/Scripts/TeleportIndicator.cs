using System.Collections;
using UnityEngine;

public class TeleportIndicator : MonoBehaviour
{
    public Transform playerPlaySpace;
    public Transform playerHead;
    public float yAxisOffset;
    public GameObject trailEffect;
    public float timeToMove = 0.9f;

    private Vector3 positionCurrent;
    private Vector3 positionLast;

    private void OnEnable()
    {
        positionCurrent = new Vector3(playerHead.position.x, playerPlaySpace.position.y + yAxisOffset, playerHead.position.z);
        positionLast = positionCurrent;
        trailEffect.transform.position = positionCurrent;
    }

    void Update () 
    {
        positionCurrent = new Vector3(playerHead.position.x, playerPlaySpace.position.y + yAxisOffset, playerHead.position.z);
        transform.position = positionCurrent;

        if (Vector3.Distance(positionLast, positionCurrent) > 2)
        {
            trailEffect.transform.position = positionLast;
            StartCoroutine(MoveToPosition(trailEffect.transform, positionCurrent, timeToMove));
        }
        else
        {
            trailEffect.transform.position = positionCurrent;
        }

        positionLast = positionCurrent;
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
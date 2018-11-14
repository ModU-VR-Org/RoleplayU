using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RollTool_NET : MonoBehaviourPun
{
    public GameObject rollBackboard;
    public TextMeshProUGUI[] rollText;
    public GameObject rollBackboardLocal;
    public TextMeshProUGUI[] rollTextLocal;
    public Transform rollBackboardLocalSpawn;
    public AudioSource roll1Sound;
    public AudioSource roll20Sound;
    private WaitForSeconds rollShowLength = new WaitForSeconds(4f);

    [PunRPC]
    public void RpcShowRoll(int value, int modValue, bool d20Selected)
    {
        //Show roll value in front of player if localAuthority (enable text, text value to value)
        if (photonView.IsMine)
        {
            rollBackboardLocal.SetActive(true);
            rollBackboardLocal.transform.position = rollBackboardLocalSpawn.position;
            rollBackboardLocal.transform.rotation = rollBackboardLocalSpawn.rotation;
            rollTextLocal[0].text = value.ToString();
            rollTextLocal[1].text = value.ToString();
            rollTextLocal[2].text = value.ToString();
            rollTextLocal[3].text = value.ToString();
        }
        else
        {
            rollBackboard.SetActive(true);
            rollText[0].text = value.ToString();
            rollText[1].text = value.ToString();
            rollText[2].text = value.ToString();
            rollText[3].text = value.ToString();
        }
        if (d20Selected)
        {
            if (value - modValue == 1)
            {
                roll1Sound.Play();
            }
            else if (value - modValue == 20)
            {
                roll20Sound.Play();
            }
        }       
        StopCoroutine("DisableRollResultText");
        StartCoroutine("DisableRollResultText");
    }
    IEnumerator DisableRollResultText()
    {
        yield return rollShowLength;
        rollBackboard.SetActive(false);
        rollBackboardLocal.SetActive(false);
    }
}

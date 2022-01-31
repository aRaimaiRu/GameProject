using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;




public class Impostor : Role
{
    [SerializeField] private float _range = 10.0f;
    private Killable _target;

    public override void Start()
    {
        base.Start();
        Debug.Log("Impostor start");
        if (photonView.IsMine)
        {
            UIControl.Instance.IsImpostor = true;

        }
        StartCoroutine(SearchForKillable());


    }
    private IEnumerator SearchForKillable()
    {
        while (true)
        {
            Killable newTarget = null;
            float minDist = Mathf.Infinity;
            Killable[] killList = FindObjectsOfType<Killable>();

            foreach (Killable kill in killList)
            {
                if (kill.GetComponent<Impostor>() != null) { continue; }
                // if (kill == this) { continue; }
                float distance = Vector3.Distance(transform.position, kill.transform.position);
                if (distance > _range) { continue; }
                if (distance < minDist)
                {

                    newTarget = kill;
                    minDist = distance;

                    // kill
                    UIControl.Instance.HasTarget = _target != null;
                    // break;
                }



            }
            if (minDist > _range || minDist == Mathf.Infinity)
            {
                UIControl.Instance.HasTarget = false;

            }
            _target = newTarget;
            yield return new WaitForSeconds(0.25f);
        }
    }

    // public virtual void MeetingAction()
    // {

    // }
    public override void GamePlayAction()
    {
        UIControl.Instance._killBtn.onClick.RemoveAllListeners();
        UIControl.Instance._killBtn.onClick.AddListener(delegate { this._target.Kill(); });
    }





}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{

    public bool gameStart;
    public bool camArrive;
    public bool camCrash;
    private int crashCount;
    public float crashTargetPos;
    public float originPos;
    public Transform firstPos;
    public Transform shipPos;
    public Transform lookPos;
    public float defFov;
    public float targetFov;
    public float moveSpeed;
    public float startDist;
    public float dist;
    public MainSystem mainSys;
    private void Start()
    {
        transform.position = firstPos.position;
        startDist = Vector3.Distance(transform.position, shipPos.position);
    }

    private void Update()
    {
        if (gameStart)
        {
            
            CamMoving();
            dist = Vector3.Distance(transform.position, shipPos.position);
            if (dist < 0.08f)
            {
                camArrive = true;
            }
        }
    }

    private void CamMoving()
    {
        if (camCrash)
        {
            crashCount++;
            if (crashCount == 1)
            {
                StartCoroutine(CamCrashPlay());
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(shipPos.position.x+crashTargetPos,shipPos.position.y, shipPos.position.z), moveSpeed+mainSys.buffSpeed);
            Vector3 dir = lookPos.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.1f);
        }
        else
        {
            crashCount = 0;
            transform.position = Vector3.MoveTowards(transform.position, shipPos.position, moveSpeed + mainSys.buffSpeed);
            Vector3 dir =  lookPos.position- transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.1f);
        }
    }

    IEnumerator CamCrashPlay()
    {
        crashTargetPos = -0.2f;
        yield return new WaitForSeconds(0.1f);
        crashTargetPos = 0.2f;
        yield return new WaitForSeconds(0.1f);
        crashTargetPos = -0.1f;
        yield return new WaitForSeconds(0.1f);
        crashTargetPos = 0.1f;
        yield return new WaitForSeconds(0.1f);
        camCrash = false;
    }

}

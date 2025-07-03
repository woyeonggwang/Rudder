using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSystem : MonoBehaviour
{

    public CameraMoving camMoving;

    [Header("joystick")]
    public float joystickVal;
    public float remapVal;
    public float minVal;
    public float maxVal;

    [Header("ship")]
    public Transform mainShip;
    public Transform shipModel;
    public float targetPosX;
    public float mainPosX;
    public float targetRot;
    public float mainRot;
    public float moveSpeed;
    public float minX;
    public float maxX;
    public float buffSpeed;
    public bool moving;
    public float enemyStartDist;
    public ParticleSystem explosion;
    public MeshRenderer[] flagMesh;
    public SkinnedMeshRenderer[] flagSkin;
    private float targetHeight;
    public ParticleSystem speedLine;
    [Header("EnemyShip")]
    public Transform enemyShip;
    public float enemySpeed;
    public Transform spawner;
    public Transform[] enemyModel;
    public bool enemyDown;
    public GameObject[] enemyParticle;

    [Header("UI")]
    public GameObject titleUi;
    public GameObject tutoUi;
    public GameObject[] readyUi;
    public GameObject playUi;
    public GameObject endUi;
    public AnimationClip tutoAnimClip;
    public AnimationClip[] readyAnimClip;
    public Text timer;
    private float sec;
    public Text endTimer;
    public Image gauge;
    public Transform anchor;
    public float guageAmount;
    public float anchorPos;
    public GameObject leftShip;
    public Transform shipImage;
    public Image fade;

    [Header("system")]
    public bool gameStart;
    public bool gameEnd;
    public int targetIndex; //시작하는 타겟인덱스
    private int startCount;
    private int gameEndCount;
    public bool result;
    public GameObject[] succesObj;
    public GameObject[] failObj;
    public AudioClip introBGM;
    public AudioClip gameBGM;
    public AudioClip victoryBGM;
    public AudioClip failBGM;
    private int gameStartCount;
    public AudioClip tutoNar;
    public GameObject[] line;
    private void Start()
    {
        line[0].SetActive(false);
        line[1].SetActive(false);
        speedLine.gameObject.SetActive(false);
        sec = 60;
        flagMesh = shipModel.GetComponentsInChildren<MeshRenderer>();
        flagSkin = shipModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        targetHeight = 50;
        enemyStartDist=Vector3.Distance(mainShip.position, spawner.GetChild(0).position);
        DisActive();
        Active();
        StartCoroutine(FadeOut());
        ProjectManager.instance.audioManager.ChangeBGM(introBGM);
        ProjectManager.instance.audioManager.drumStop = true;
    }

    private void Update()
    {

        Joystick();
        if (gameStart)
        {
            gameStartCount++;
            if (gameStartCount == 1)
            {
                line[0].SetActive(true);
                line[1].SetActive(true);
                ProjectManager.instance.audioManager.ChangeBGM(gameBGM);
            }
            if (gameEnd)
            {
                line[0].SetActive(false);
                line[1].SetActive(false);
                ProjectManager.instance.audioManager.dotSound.Stop();
                if (result)
                {
                    ProjectManager.instance.audioManager.drumFaster = true;
                    if (enemyDown)
                    {
                        for (int i = 0; i < enemyModel.Length; i++)
                        {
                            enemyModel[i].position += Vector3.down;
                        }
                    }
                }
                gameEndCount++;
                if (gameEndCount == 1)
                {
                    ProjectManager.instance.audioManager.dotSound.Stop();
                    StartCoroutine(EndPlay());
                }
            }
            else
            {
                ProjectManager.instance.audioManager.drumStop = false;
                ShipMoving();
                EnemyMove();
                Timer();
            }
        }
        else
        {
            CheckStart();
        }
        SetTargetHeight();
    }

    private void Joystick()
    {
        joystickVal = Input.GetAxisRaw("1P_axis");
        remapVal = remap(joystickVal, -1, 1, minVal, maxVal);
        shipImage.localPosition = new Vector3(remapVal, shipImage.localPosition.y, shipImage.localPosition.z);
    }

    private void DisActive()
    {
        endUi.SetActive(false);
        tutoUi.SetActive(false);
        playUi.SetActive(false);
        for (int i = 0; i < readyUi.Length; i++)
        {
            readyUi[i].SetActive(false);
        }
    }

    private void SetTargetHeight()
    {
        for (int i = 0; i < flagMesh.Length; i++)
        {
            flagMesh[i].material.SetFloat("_Dither_Height", targetHeight);
        }
        for (int i = 0; i < flagSkin.Length; i++)
        {
            flagSkin[i].material.SetFloat("_Dither_Height", targetHeight);
        }
    }

    private void EnemyMove()
    {
        enemyShip.position += Vector3.forward * enemySpeed;
    }


    private void ShipMoving()
    {
        targetPosX = remap(joystickVal, 1, -1, minX, maxX);
        float distToEnd = Vector3.Distance(mainShip.position, spawner.GetChild(0).position);
        guageAmount = remap(distToEnd, enemyStartDist, 10, 0, 1);
        anchorPos = remap(distToEnd, enemyStartDist, 10, -330, 330);
        if (anchorPos < -330)
        {
            anchorPos = -330;
        }
        gauge.fillAmount = guageAmount;
        anchor.localPosition = new Vector3(anchorPos, anchor.localPosition.y, anchor.localPosition.z);
        if (distToEnd < 2f)
        {
            gameEnd = true;
        }
        if (buffSpeed < 0.18f)
        {
            ProjectManager.instance.audioManager.drumFaster = false;
            buffSpeed += Time.deltaTime * 0.05f;
            speedLine.gameObject.SetActive(false);
        }
        else
        {
            ProjectManager.instance.audioManager.drumFaster = true;
            speedLine.gameObject.SetActive(true);
        }
        mainShip.position += Vector3.forward * (moveSpeed + buffSpeed);
        shipModel.localPosition = new Vector3(mainPosX, shipModel.localPosition.y, shipModel.localPosition.z);
        shipModel.rotation = Quaternion.Euler(0, mainRot, 0);
        if (mainPosX < targetPosX - 0.1f)
        {
            mainPosX += Time.deltaTime * 15;
            moving = true;
        }
        else if (mainPosX > targetPosX + 0.1f)
        {
            mainPosX -= Time.deltaTime * 15;
            moving = true;
        }
        else
        {
            mainPosX = targetPosX;
            moving = false;
        }
        if (mainRot > targetRot + 0.1f)
        {
            mainRot -= Time.deltaTime * 40;
        }
        else if (mainRot < targetRot + -0.1f)
        {
            mainRot += Time.deltaTime * 40;
        }
        else
        {
            mainRot = targetRot;
        }
        if (moving)
        {
            if (mainPosX < targetPosX - 2f)
            {
                targetRot = 25;
            }
            else if (mainPosX > targetPosX + 2f)
            {
                targetRot = -25;
            }
            else
            {
                targetRot = 0;
            }
        }
        else
        {
            targetRot = 0;
        }


    }

    private void Active()
    {
        titleUi.SetActive(true);
    }

    private void CheckStart()
    {
        switch (targetIndex)
        {
            case 0:
                if (remapVal < minVal + 3)
                {
                    ProjectManager.instance.audioManager.drumSound.Play();
                    leftShip.SetActive(false);
                    targetIndex = 1;
                }
                break;
            case 1:
                if (remapVal > maxVal - 3)
                {
                    ProjectManager.instance.audioManager.drumSound1.Play();
                    targetIndex = 2;
                }
                break;
            case 2:
                camMoving.gameStart = true;
                titleUi.SetActive(false);
                if (camMoving.camArrive)
                {

                    startCount++;
                    if (startCount == 1)
                    {
                        StartCoroutine(TutoPlay());
                    }
                }
                else
                {
                    targetHeight = remap(camMoving.dist, camMoving.startDist, 0.1f, 50, 30);
                }
                break;
        }
    }


    public void Explose()
    {
        buffSpeed =0.05f;
        camMoving.camCrash = true;
        ProjectManager.instance.audioManager.explosionSound.Play();
    }

    private void Timer()
    {
        if (!gameEnd)
        {

            if (sec > 0)
            {
                sec -= Time.deltaTime;
                int secVal = (int)sec;
                if (sec < 10)
                {
                    timer.text = "0" + secVal;
                }
                else
                {
                    timer.text = secVal + "";
                }
            }
            else
            {
                result = false;
                gameEnd = true;
                sec = 0;
            }
        }
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i > -0.1f; i -= 0.01f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeIn()
    {
        for (float i = 0; i < 1.1f; i += 0.01f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator TutoPlay()
    {
        tutoUi.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        ProjectManager.instance.audioManager.TutoNarPlay(tutoNar);
        yield return new WaitForSeconds(tutoAnimClip.length);
        tutoUi.SetActive(false);
        StartCoroutine(ReadyPlay());
    }

    IEnumerator ReadyPlay()
    {

        readyUi[0].SetActive(true);
        yield return new WaitForSeconds(readyAnimClip[0].length);
        readyUi[0].SetActive(false);
        readyUi[1].SetActive(true);
        yield return new WaitForSeconds(readyAnimClip[1].length);
        readyUi[1].SetActive(false);
        ProjectManager.instance.audioManager.GeneralNarrPlay();
        playUi.SetActive(true);
        gameStart = true;
        ProjectManager.instance.audioManager.dotSound.Play();
    }

    IEnumerator EndPlay()
    {
        endTimer.text = timer.text + " 초";
        if (result)
        {

            for (int i = 0; i < succesObj.Length; i++)
            {
                succesObj[i].SetActive(true);
            }
            for (int i = 0; i < failObj.Length; i++)
            {
                failObj[i].SetActive(false);
            }
            playUi.SetActive(false);
            for (int i = 0; i < enemyModel.Length; i++)
            {
                enemyParticle[i].SetActive(true);
            }
            yield return new WaitForSeconds(2f);
            ProjectManager.instance.audioManager.ChangeBGM(victoryBGM);
            ProjectManager.instance.audioManager.oarSound.Play();
            enemyDown = true;
            endUi.SetActive(true);

        }
        else
        {
            ProjectManager.instance.audioManager.drumStop = true;
            for (int i = 0; i < succesObj.Length; i++)
            {
                succesObj[i].SetActive(false);
            }
            for (int i = 0; i < failObj.Length; i++)
            {
                failObj[i].SetActive(true);
            }
            playUi.SetActive(false);
            
            
            yield return new WaitForSeconds(1f);
            ProjectManager.instance.audioManager.ChangeBGM(failBGM);
            endUi.SetActive(true);
        }
        yield return new WaitForSeconds(10);
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
        
    }

    public static float remap(float val, float in1, float in2, float out1, float out2)  //리맵하는 함수
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public MainSystem mainSys;
    public Transform bombParent;
    public GameObject bomb;
    public float[] xPos;
    public float xPos1;
    public float xPos2;
    public int firstSpawnCount;
    public float spawnDelay;
    public int spawnIndex;
    private void Start()
    {
        xPos = new float[3];
    }
    private void Update()
    {

        if (mainSys.gameEnd)
        {
            bombParent.gameObject.SetActive(false);
        }
        if (mainSys.gameStart)
        {
            firstSpawnCount++;
            if (firstSpawnCount == 1)
            {
                StartCoroutine(FirstSpawnDelay());
            }
        }
        spawnDelay = remap(mainSys.buffSpeed, 0, 0.18f, 4f, 2.8f);
    }

    IEnumerator FirstSpawnDelay()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnPlay());
    }

    IEnumerator SpawnPlay()
    {
        GameObject temp = Instantiate(bomb);
        temp.transform.parent = bombParent;
        xPos[0] = Random.Range(-18,-10);
        xPos[1] = Random.Range(-10,10);
        xPos[2] = Random.Range(10, 18);
        temp.transform.position = new Vector3(xPos[spawnIndex], transform.position.y, transform.position.z);
        temp.SetActive(true);
        yield return new WaitForSeconds(spawnDelay);
        spawnIndex = Random.Range(0, 3);
        StartCoroutine(SpawnPlay());
    }
    public static float remap(float val, float in1, float in2, float out1, float out2)  //리맵하는 함수
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }
}

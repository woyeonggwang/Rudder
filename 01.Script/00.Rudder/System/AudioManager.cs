using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public XmlLoader xmlLoader;
    public AudioSource mainBGM;
    public AudioSource narSound;
    public AudioSource waterSound;
    public AudioSource explosionSound;
    public AudioSource windSound;
    public AudioSource systemSound;
    public AudioSource dotSound;
    public AudioSource waterBeginSound;
    public AudioSource drumSound1;
    public AudioSource drumSound;
    public AudioSource oarSound;
    public float maxVolume;
    public int sceneCount0;
    public int sceneCount1;
    public AudioClip[] mainBGMClip;
    public AudioClip[] generalClip;
    public AudioClip soldierClip;
    private float targetVolume;
    private bool raise;
    public float windTargetVolume;
    public bool windChange;
    public bool drumStop;
    public bool drumFaster;
    public int drumCount;
    public bool bgmChange;


    private void Start()
    {
        xmlLoader = ProjectManager.instance.xmlLoader;
    }
    private void Update()
    {
        Drum();
        //mainBGM.volume = xmlLoader.mainBGM;
        
        maxVolume = xmlLoader.mainBGM;
        narSound.volume = xmlLoader.narSound;
        waterSound.volume = xmlLoader.waterSound;
        explosionSound.volume = xmlLoader.explosionSound;
        systemSound.volume = xmlLoader.systemSound;
        dotSound.volume = xmlLoader.dotSound;
        waterBeginSound.volume = xmlLoader.waterSound;
        drumSound.volume = xmlLoader.drumSound;
        drumSound1.volume = xmlLoader.drumSound;
        oarSound.volume = xmlLoader.drumSound;
    }
    
    public void GeneralNarrPlay()
    {
        StartCoroutine(GeneralPlay());
    }
    public void TutoNarPlay(AudioClip clip)
    {
        StartCoroutine(NarPlay(clip));
    }
    public void SysSound(AudioClip targetClip)
    {
        systemSound.PlayOneShot(targetClip);
    }
    public void Drum()
    {
        if (drumStop)
        {
            drumSound.volume = 0;
            drumCount = 0;
        }
        else
        {
            drumCount++;
            drumSound.volume = xmlLoader.drumSound;
            if (drumCount == 1)
            {
                StartCoroutine(DrumPlay());
            }
        }
    }
    IEnumerator DrumPlay()
    {
        if (drumFaster)
        {
            drumSound.Play();
            yield return new WaitForSeconds(0.6f);
            drumCount = 0;
        }
        else
        {
            drumSound.Play();
            yield return new WaitForSeconds(1.5f);
            drumCount = 0;
        }
    }
    
    IEnumerator GeneralPlay()
    {
        mainBGM.volume = 0.3f;
        narSound.PlayOneShot(generalClip[0]);
        yield return new WaitForSeconds(generalClip[0].length);
        narSound.PlayOneShot(generalClip[1]);
        yield return new WaitForSeconds(generalClip[1].length);
        mainBGM.volume = xmlLoader.mainBGM;
    }

    IEnumerator NarPlay(AudioClip clip)
    {
        mainBGM.volume = 0.1f;
        narSound.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        mainBGM.volume = xmlLoader.mainBGM;
    }

    public void ChangeBGM(AudioClip targetClip)
    {
        StartCoroutine(MainBGMChanger(targetClip));
    }

    IEnumerator MainBGMChanger(AudioClip targetClip)
    {
        for (float i = xmlLoader.mainBGM; i > -0.1f; i -= 0.01f)
        {
            mainBGM.volume = i;
            yield return new WaitForSeconds(0.01f);
        }
        mainBGM.Stop();
        mainBGM.clip = targetClip;
        yield return new WaitForSeconds(0.5f);
        mainBGM.Play();
        for (float i = 0; i < xmlLoader.mainBGM; i += 0.01f)
        {
            mainBGM.volume = i;
            yield return new WaitForSeconds(0.01f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public MainSystem mainSys;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bomb")
        {
            print(0);
            GameObject temp = Instantiate(mainSys.explosion.gameObject);
            temp.transform.position = other.transform.position;
            temp.SetActive(true);
            mainSys.Explose();
            Destroy(other.gameObject);
        }
        if (other.tag == "EndPoint")
        {
            ProjectManager.instance.audioManager.explosionSound.Play();
            mainSys.result = true;
            mainSys.gameEnd = true;
        }
        
    }

}

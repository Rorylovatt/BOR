using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoints : MonoBehaviour
{
    Racemanager racemanager;

    // Start is called before the first frame update
    void Start()
    {
        racemanager = FindObjectOfType<Racemanager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(this.gameObject.name == "CheckPoint1Collider")
        {
            racemanager.checkPointHit[0] = true;
        }
        if (this.gameObject.name == "CheckPoint2Collider" && racemanager.checkPointHit[0] == true)
        {
            racemanager.checkPointHit[1] = true;
        }
        if (this.gameObject.name == "CheckPoint3Collider" && racemanager.checkPointHit[1] == true)
        {
            racemanager.checkPointHit[2] = true;
        }
        if (this.gameObject.name == "CheckPointFinishCollider" && racemanager.checkPointHit[2] == true)
        {
            racemanager.checkPointHit[3] = true;
        }
    }
}

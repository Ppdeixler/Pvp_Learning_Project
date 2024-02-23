using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdentifier : MonoBehaviour
{


    public bool onEnemy = false;

    public GameObject enemy;




    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onEnemy = true;
            enemy = other.gameObject;
        }
        if (other == null)
        {
            enemy = null;
            onEnemy = false;    
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enemy = null;
            onEnemy = false;
        }
    }


}

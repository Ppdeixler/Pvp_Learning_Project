using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    //HP Variables
    public float hp;
    public float maxHP;
    public float minHP;

    //UI Variables
    [SerializeField] private Image img;
    [SerializeField] private Slider sld;

    void Update()
    {
        HP();   
    }

    void HP()
    {
        hp = Mathf.Clamp(hp, minHP, maxHP);

        sld.maxValue = maxHP;

        sld.value = hp;
    }

    public void TakeDamage(float damage, GameObject enemy)
    {

        hp -= damage;
    }



}

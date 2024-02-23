using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    //Attack Variables

    public bool attacking;
    private bool canAttack = true;

    public float attackCd;

    [SerializeField] private GameObject attackCollider;
    private Collider collider;

    //Stun Variables

    public bool stunned;
    public float parryDuration;
    public float damageStunDuration;


    //Parry Variables

    private bool parrying;
    private bool canParry = true;

    public float parryCd;

    public float parryStunDuration;

    //Block Variables

    public bool isBlocking;

    private float blockingReduce;
    private float endurance;

    //Enemy Variables

    private EnemyIdentifier EI;

    private GameObject enemyAttackable;

    private AttackSystem enemySystem;

    private HealthSystem HS;

    //Player Variables

    [SerializeField] private GameObject playerGmObj;

    [SerializeField] private DamageSystem dmgSystem;

    //Main Functions

    void Awake()
    {
        collider = attackCollider.GetComponent<BoxCollider>();

        EI = attackCollider.GetComponent<EnemyIdentifier>();
    }

    void Update()
    {
        Attack();
        Block();
    }

    //Stun Function

    IEnumerator ParryStun(float stunDuration)
    {
        stunned = true;
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
    }

    IEnumerator DamageStun(float stunDuration)
    {
        enemySystem.stunned = true;
        yield return new WaitForSeconds(stunDuration);
        enemySystem.stunned = false;
    }

    //Block and Parry Functions

    void Block()
    {
        if (attacking) return;
        if (stunned) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (canParry) StartCoroutine("ParryCoroutine");
        }

        if (isBlocking)
        {
            blockingReduce = 0.1f;
        }
        else
        {
            blockingReduce = 1f;
        }

        //StartCoroutine(Stun(damageStunDuration));

    }

    IEnumerator ParryCoroutine()
    {
        parrying = true;
        isBlocking = false;
        canParry = false;

        yield return new WaitForSeconds(parryDuration);

        parrying = false;
        isBlocking = true;

        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.F));

        isBlocking = false;

        yield return new WaitForSeconds(parryCd);

        canParry = true;
    }


    //Attack Functions

    void Attack()
    {

        if (EI.enemy != null)
        {
            enemyAttackable = EI.enemy;
            HS = enemyAttackable.GetComponent<HealthSystem>();
            enemySystem = enemyAttackable.transform.Find("AttackRange").GetComponent<AttackSystem>();
        }
        else
        {
            enemyAttackable = null;
            HS = null;
        }

        if (isBlocking) return;
        if (canAttack == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Atacou");
            StartCoroutine("AttackCoroutine");
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        attacking = true;

        if(enemyAttackable != null && HS != null)
        {
            if (enemySystem.parrying)
            {
                StartCoroutine(ParryStun(parryStunDuration));
                StopCoroutine("AttackCoroutine");
            }

            HS.TakeDamage(dmgSystem.damage * enemySystem.blockingReduce, enemyAttackable);
            StartCoroutine(DamageStun(damageStunDuration));
        }

        yield return new WaitForSeconds(attackCd);

        canAttack = true;
        attacking = false;
    }



}

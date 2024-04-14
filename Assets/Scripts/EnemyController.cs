using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int enemyElement, enemyLife, enemyAttack;
    public int[] nonWeaknessElement;
    public float wanderRadius, attackRange = 2.5f;
    public Animator enemyAnimator;

    Transform target;

    NavMeshAgent agent;

    bool isChasing = true, isWandering, isAttacking, isDead;

    float targetDistance;

    Coroutine C_OnChase;

    AudioSource asc;

    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        asc = GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        StartCoroutine(EnemyStateChecker());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Spell")
        {
            //bool isCountered = false;

            for(int i = 0; i < nonWeaknessElement.Length; i++)
            {
                if (col.gameObject.GetComponent<SpellAttribute>().spellValue == nonWeaknessElement[i])
                {
                    print("Get Stronger");
                    enemyLife++;
                    enemyAttack++;
                    this.transform.localScale = new Vector3(this.transform.localScale.x + 0.1f, this.transform.localScale.y + 0.1f, this.transform.localScale.z + 0.1f);
                    break;
                }

                if(i == nonWeaknessElement.Length - 1)
                {
                    print("Getting Damage");
                    enemyLife--;

                    if(enemyAttack > 1)
                    {
                        enemyAttack--;
                    }

                    this.transform.localScale = new Vector3(this.transform.localScale.x - 0.1f, this.transform.localScale.y - 0.1f, this.transform.localScale.z - 0.1f);
                }
            }

            Destroy(col.gameObject);
        }
    }

    IEnumerator EnemyStateChecker()
    {
        while (true)
        {
            if (!gm.isEnd)
            {
                targetDistance = Vector3.Distance(this.transform.position, target.position);

                if (targetDistance < attackRange && !target.GetComponent<PlayerAttribute>().isInvis)
                {
                    isChasing = false;

                    if (C_OnChase != null)
                        StopCoroutine(C_OnChase);

                    isAttacking = true;

                    agent.isStopped = true;
                }

                else
                {
                    isChasing = true;

                    isAttacking = false;

                    agent.isStopped = false;
                }

                if (enemyLife <= 0)
                {
                    isDead = true;

                    if(C_OnChase != null)
                    StopCoroutine(C_OnChase);

                    agent.isStopped = true;
                    //Destroy(this.gameObject);
                }

                if (target.GetComponent<PlayerAttribute>().isInvis)
                {
                    isChasing = false;
                    isAttacking = false;
                    isWandering = true;
                }
            }

            else
            {
                isChasing = false;
                isAttacking = false;
                isWandering = false;
                isDead = false;

                agent.isStopped = true;

                enemyAnimator.Play("Taunting");
            }

            AnimationStateControl();
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    void AnimationStateControl()
    {
        enemyAnimator.SetBool("isChasing", isChasing);
        enemyAnimator.SetBool("isAttacking", isAttacking);
        enemyAnimator.SetBool("isWandering", isWandering);
        enemyAnimator.SetBool("isDead", isDead);
    }

    public void PlaySound(AudioClip clip)
    {
        asc.clip = clip;
        asc.Play();
    }

    public void DealDamage()
    {
        if(targetDistance <= attackRange)
        {
            print("Damaged");
            target.GetComponent<PlayerAttribute>().healthPoints-=enemyAttack;
        }
    }

    public void StartChasing()
    {
        C_OnChase = StartCoroutine(OnChase());
    }

    IEnumerator OnChase()
    {
        while (true)
        {
            agent.SetDestination(target.position);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Wandering()
    {
        if (C_OnChase != null)
            StopCoroutine(C_OnChase);

        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) //Determining random position on navmesh surface
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void DestroyOnDead()
    {
        gm.score++;
        Destroy(this.gameObject);
    }

}

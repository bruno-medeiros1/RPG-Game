using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private const float FOLLOW_SPEED = 2f;
    private const float DISTANCE_TO_STOP = 1.2f;
    private const float PROJECTILE_SPEED = 15f;
    private const float ATTACK_TIMER_MAX = 2f;

    private Transform target;
    private Rigidbody2D r;

    private float Attack_Timer;

    private State state;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        r = GetComponent<Rigidbody2D>();
        Attack_Timer = ATTACK_TIMER_MAX;
        state = State.Chasing;
    }

    private enum State
    {
        Chasing,
        Attack,
        Stoped,
        Freeze,
    }
    private void Update()
    {
        GetComponent<Animator>().SetFloat("Speed", r.velocity.sqrMagnitude);
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Chasing:

                //Direção
                Vector3 dir = (target.transform.position - r.transform.position).normalized;

                /*Enquanto a distancia for maior que a de ataque entre o player e o enimigo vai sempre andar atrás dele*/
                if (Vector3.Distance(target.transform.position, r.transform.position) > DISTANCE_TO_STOP)
                {
                    r.velocity = new Vector2(FOLLOW_SPEED, 0f);
                    r.MovePosition(r.transform.position + dir * FOLLOW_SPEED * Time.fixedDeltaTime);

                    /*CRONOMETRO ATÉ EXECUTAR O CODIGO*/
                    Attack_Timer -= Time.deltaTime;
                    if (Attack_Timer < 0)
                    {
                        Attack_Timer += ATTACK_TIMER_MAX;
                        state = State.Attack;
                    }
                }
                else
                {
                    state = State.Stoped;

                }
                break;

            case State.Attack:
                Attack();
                break;

            case State.Stoped:
                if (Vector3.Distance(target.transform.position, r.transform.position) < DISTANCE_TO_STOP)
                {
                    r.velocity = Vector2.zero;
                }
                else { state = State.Chasing; }
                break;
            case State.Freeze:
                r.velocity = Vector2.zero; 
                break;
        }
    }

    private void Attack() 
    {
        /*Direção do player e do mole*/
        Vector3 dir = (target.transform.position - r.transform.position).normalized; 

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        SoundManager.GetInstance().Play("MoleProjectile");
        Transform Mole_Projectile = Instantiate(GameAssets.GetInstance().MoleProjectile, transform.position + new Vector3(1,1), Quaternion.Euler(0, 0, angle - 90));
        Mole_Projectile.GetComponent<Rigidbody2D>().AddForce(dir * PROJECTILE_SPEED, ForceMode2D.Impulse);
        state = State.Chasing;       
    }

    public void Stop()
    {
        state = State.Freeze;
    }
}

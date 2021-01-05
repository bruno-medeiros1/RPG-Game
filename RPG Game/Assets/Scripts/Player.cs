using UnityEngine;
using System;
using System.Collections;
public class Player : MonoBehaviour
{
    private const float PLAYER_SPEED = 8f;
    private const float ARROW_SPEED = 15f;
    private const float COOLDOWN_SPECIALSHOOT_TIMER_MAX = 5f;
    private const float DISABLE_ANIM_TIMER = 0.3f;
    private const float FALLBACK_DAMAGE_FORCE = 4500f;

    public event EventHandler OnStartBattle;
    public event EventHandler OnDied;

    public int kill;

    private Rigidbody2D r;
    private Animator anim;
    private Vector2 movement;
    private Vector3 MousePos;

    private bool OnStart = false;
    private bool SpecialShot_CoolDown = false;
    private bool IsDeath = false;
    public HealthSystem health;
    private static Player Instance;

    public bool IsBattleEnable;

    private void Awake()
    {
        Instance = this;
        r = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = new HealthSystem(150);
        kill = 0;
    }
    private void Start()
    {
        /*Instanciamos a HealthBar do Jogador*/
        Transform HealthBar = Instantiate(GameAssets.GetInstance().HealthBar, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f), Quaternion.identity);
        HealthBar.transform.parent = gameObject.transform;

        /*Definirmos a vida do player na  Health Bar*/
        HealthBar.gameObject.GetComponent<HealthBar>().Setup(health);
    }

    void Update()
    {
        if (IsDeath) 
        {
            r.constraints = RigidbodyConstraints2D.FreezeAll;
            anim.enabled = false;
            return; 
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        HandleAnimations();

        /*Se estiver a carregar no MOUSE0 e estiver parado pode disparar flechas*/
        if (Input.GetKeyDown(KeyCode.Mouse0) && movement.sqrMagnitude == 0f)
        {
            MousePos = Input.mousePosition;
            MousePos.z = 10;
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);

            Shoot();      
        }
           

        /*SE PRESSIONAR MOUSE1 VAI ATIVAR O SPECIAL SHOT*/
        if (Input.GetKeyDown(KeyCode.Mouse1) && movement.sqrMagnitude == 0f && SpecialShot_CoolDown == false)
        {
            

            SpecialShot_CoolDown = true;
            MousePos = Input.mousePosition;
            MousePos.z = 10;
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);

            SpecialShoot();
            StartCoroutine("CanSpecialShoot");

        }
        if (IsBattleEnable) 
        {
            if (!OnStart) 
            {
                /*Ativamos o evento*/
                OnStartBattle?.Invoke(this, EventArgs.Empty);
                OnStart = true;
            }  
            
        }

    }
    /*Tudo relacionado com fisicas deve ser colocado aqui pois na funcao update
     devido a os frame rates nao serem iguais pode existir bugs*/
    private void FixedUpdate()
    {
        movement.Normalize();
        r.MovePosition(r.position + movement * PLAYER_SPEED * Time.fixedDeltaTime);
    }
    IEnumerator CanSpecialShoot()
    {
        yield return new WaitForSeconds(COOLDOWN_SPECIALSHOOT_TIMER_MAX);
        SpecialShot_CoolDown = false;
    }
    private void HandleAnimations()
    {
        /*Se estiver a carregar na tecla para se movimentar vamos mudar a animação
         caso contrario fica a mesma*/
        if (movement != Vector2.zero)
        {
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
        }

        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    /*METODO RESPONSÁVEL POR DISPARAR UMA SETA COM UM COOLDOWN DE 0.2 SEGUNDOS*/
    private void Shoot()
    {
        SoundManager.GetInstance().Play("Arrow");

        /*Vetor com a posição do Player*/
        Vector3 PlayerPos = new Vector3(transform.position.x, transform.position.y);

        /*Vetor da Direção para onde a flecha vai ser lançada que é determinada 
         atráves da subtracao do Vector do Mouse Pos pelo PlayerPos*/
        Vector3 Dirrection = (MousePos - PlayerPos).normalized; //direção do vetor 

        float angle = Mathf.Atan2(Dirrection.y, Dirrection.x) * Mathf.Rad2Deg;

        HandleShootLimitations(Dirrection, angle, 1);


    }
    private void HandleShootLimitations(Vector3 dir, float _angle, int arrow_num)
    {
        string Active_SpriteName = transform.GetComponent<SpriteRenderer>().sprite.name;

        /*Limitar o angulo para 180 graus qualquer que seja a posição para a qual esteja
        a olhar o jogador!*/
        switch (Active_SpriteName)
        {
            case "hero-idle-back":

                if (_angle >= 0 && _angle <= 180)
                {
                    anim.SetBool("CanAttack", true);
                    for (int i = 0; i < arrow_num; i++)
                    {
                        GameObject Arrow = Instantiate(GameAssets.GetInstance().Arrow, transform.position, Quaternion.Euler(0, 0, _angle - 90));
                        Arrow.GetComponent<Rigidbody2D>().AddForce(dir * ARROW_SPEED, ForceMode2D.Impulse);
                        dir += new Vector3(0.1f, 0);
                    }
                    StartCoroutine("PlayAnim");
                }
                break;
            case "hero-idle-front":

                if (_angle <= 0 && _angle >= -180)
                {
                    anim.SetBool("CanAttack", true);
                    for (int i = 0; i < arrow_num; i++)
                    {
                        GameObject Arrow = Instantiate(GameAssets.GetInstance().Arrow, transform.position, Quaternion.Euler(0, 0, _angle - 90));
                        Arrow.GetComponent<Rigidbody2D>().AddForce(dir * ARROW_SPEED, ForceMode2D.Impulse);
                        dir += new Vector3(0.1f, 0);
                    }
                    StartCoroutine("PlayAnim");
                }
                break;
            case "hero-idle-side-left":
                //Debug.Log("Left");
                if (_angle >= 90 || _angle <= -90)
                {
                    anim.SetBool("CanAttack", true);
                    for (int i = 0; i < arrow_num; i++)
                    {
                        GameObject Arrow = Instantiate(GameAssets.GetInstance().Arrow, transform.position, Quaternion.Euler(0, 0, _angle - 90));
                        Arrow.GetComponent<Rigidbody2D>().AddForce(dir * ARROW_SPEED, ForceMode2D.Impulse);
                        dir += new Vector3(0.1f, 0);
                    }
                    StartCoroutine("PlayAnim");
                }
                break;
            case "hero-idle-side-right":

                if (_angle <= 90 && _angle >= -90)
                {
                    anim.SetBool("CanAttack", true);
                    for (int i = 0; i < arrow_num; i++)
                    {
                        GameObject Arrow = Instantiate(GameAssets.GetInstance().Arrow, transform.position, Quaternion.Euler(0, 0, _angle - 90));
                        Arrow.GetComponent<Rigidbody2D>().AddForce(dir * ARROW_SPEED, ForceMode2D.Impulse);
                        dir += new Vector3(0.1f, 0);
                    }
                    StartCoroutine("PlayAnim");
                }
                break;
        }
    }

    /*METODO QUE DISPARA MAIS DO QUE UMA FLECHA AO MESMO TEMPO (PODER ESPECIAL)*/
    private void SpecialShoot()
    {
        SoundManager.GetInstance().Play("Arrow");

        /*Vetor com a posição do Player*/
        Vector3 PlayerPos = new Vector3(transform.position.x, transform.position.y);

        /*Vetor da Direção para onde a flecha vai ser lançada que é determinada 
         atráves da subtracao do Vector do Mouse Pos pelo PlayerPos*/
        Vector3 Dirrection = (MousePos - PlayerPos).normalized; //direção do vetor 

        float angle = Mathf.Atan2(Dirrection.y, Dirrection.x) * Mathf.Rad2Deg;

        HandleShootLimitations(Dirrection, angle, 5);

    }
    IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(DISABLE_ANIM_TIMER);
        anim.SetBool("CanAttack", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Heart"))
        {
            SoundManager.GetInstance().Play("Heart");

            /*Adicionar +25 vida*/
            health.Heal(25);

            Destroy(collision.gameObject);

            Debug.Log("Vida: " + health.GetHealth());
        }

        if (collision.gameObject.CompareTag("Mole"))
        {
            ///*Pedaço de codigo que afasta o jogador do enimigo ao levar dano*/
            Vector3 dir = new Vector3(1,-1);

            r.AddForce(dir * FALLBACK_DAMAGE_FORCE ,ForceMode2D.Force);

            SoundManager.GetInstance().Play("Hurt");


            health.Damage(UnityEngine.Random.Range(10, 15));

            if (health.GetHealth() == 0)
            {
                /*Ativamos o nosso Evento*/              
                OnDied?.Invoke(this, EventArgs.Empty);
                IsDeath = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mole_Projectile"))
        {

            SoundManager.GetInstance().Play("Hurt");


            health.Damage(UnityEngine.Random.Range(10, 15));

            if (health.GetHealth() == 0)
            {
                /*Ativamos o nosso Evento*/
                OnDied?.Invoke(this, EventArgs.Empty);
                IsDeath = true;
            }
        }
    }
    public static Player GetInstance()
    {
        return Instance;
    }

    public int GetKills() 
    {
        return kill;
    }
}

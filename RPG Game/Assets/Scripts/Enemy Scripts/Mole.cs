using UnityEngine;
using System;

public class Mole : MonoBehaviour
{
    private static Mole instance;

    private Animator m_anim;
    public HealthSystem health;
    private GameObject Player;
    
    public event EventHandler OnDie;

    private void Awake()
    {
        instance = this;
        m_anim = GetComponent<Animator>();

        /*Referencia do Player*/
        Player = GameObject.FindGameObjectWithTag("Player");

        /*Vida do Enimigo*/
        health = new HealthSystem(100);
        
    }
    private void Start()
    {
        /*Cria a barra de vida do Enimigo*/
        Transform HealthBar = Instantiate(GameAssets.GetInstance().HealthBar, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f), Quaternion.identity);
        HealthBar.transform.parent = gameObject.transform;

        /*Referencia da HealthBar criada*/
        HealthBar.gameObject.GetComponent<HealthBar>().Setup(health);

    }

    public static Mole GetInstance() 
    {
        return instance;
    }
    public void Dead() 
    {
        OnDie?.Invoke(this, EventArgs.Empty);

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        m_anim.SetBool("IsDead", true);
        Destroy(gameObject, 0.5f);
  
        /*REFERENCIA AO SCRIPT PLAYER ONDE SE ENCONTRAM AS VARIAVEIS DA VIDA*/
        Player player = Player.GetComponent<Player>();

        /*55%*/
        if(player.health.GetHealth() < player.health.GetMaxHealth() * 0.5f) 
        {
            if(UnityEngine.Random.Range(0, 100) < 55) 
            {
                Instantiate(GameAssets.GetInstance().HeartPref, transform.position, Quaternion.identity);
                
            }
            return;
        }
        /*25%*/
        else
        {
            if (UnityEngine.Random.Range(0, 100) < 25)
            {
                Instantiate(GameAssets.GetInstance().HeartPref, transform.position, Quaternion.identity);
               
            }
            return;
        }

        
    }

}

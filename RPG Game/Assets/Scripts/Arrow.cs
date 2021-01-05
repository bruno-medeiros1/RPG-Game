using UnityEngine;

public class Arrow : MonoBehaviour
{
    private bool CriticalShot;

    private static Arrow instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Map")
        {
            /*Quando a nossa flecha colidir com algo que tenha box collider, o  effect é instanciado
             e destroido passado 0.5s (500mms) e depois a flecha*/
            GameObject effect = Instantiate(GameAssets.GetInstance().HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            Destroy(gameObject);
        }

        /*Se a seta acertar no Mole Enemy*/
        if (collision.gameObject.CompareTag("Mole"))
        {
            /*Quando a nossa flecha colidir com algo que tenha box collider, o  effect é instanciado
             e destroido passado 0.5s (500mms) e depois a flecha*/
            GameObject effect = Instantiate(GameAssets.GetInstance().HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            Destroy(gameObject);

            SoundManager.GetInstance().Play("Hit");

            /*SE A VIDA DELE FOR 0*/
            if (collision.gameObject.GetComponent<Mole>().health.GetHealth() == 0)
            {
                SoundManager.GetInstance().Play("Kill");
                Player.GetInstance().kill++;
                Debug.Log("kills: " + Player.GetInstance().GetKills());
                collision.gameObject.GetComponent<Mole>().Dead();
                return;

            }

            /*REFERENCIA DO ENINIMIGO QUE ACERTAMOS*/
            Mole enemy = collision.gameObject.GetComponent<Mole>();

            /*Probabilidade de ser critico 30%*/
            CriticalShot = Random.Range(0, 100) < 30;

            /*Dano possível do jogador*/
            int damage_amount = Random.Range(10, 15);

            if (CriticalShot)
            {
                /*DANO AO INIMIGO REFERENCIADO*/
                enemy.health.Damage(damage_amount * 2);
                DamagePopup.Create(new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 0.5f), damage_amount * 2, true);
                return;
            }
            DamagePopup.Create(new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 0.5f), damage_amount, false);
            enemy.health.Damage(damage_amount);
        }
    }

    public static Arrow GetInstance()
    {
        return instance;
    }
}

using UnityEngine;

public class Mole_Project : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Mole_Projectile")) 
        {
            GameObject effect = Instantiate(GameAssets.GetInstance().HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            Destroy(gameObject);
        }
    }
}

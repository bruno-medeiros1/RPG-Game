using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public GameObject Arrow;

    public Transform MoleProjectile;
    public Transform HealthBar;
    public Transform MoleEnemy;
    public Transform HeartPref;

    public GameObject HitEffect;

    public Transform DamagePopupPref;
   

    private void Awake()
    {
        instance = this;
    }

    public static GameAssets GetInstance() 
    {
        return instance;
    }
}

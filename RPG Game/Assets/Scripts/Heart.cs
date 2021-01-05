using UnityEngine;

public class Heart : MonoBehaviour
{
    private void Start()
    {
        /*Logo que é instanciado é destruido passado 7s*/
        Destroy(gameObject, 7f);
    }
}

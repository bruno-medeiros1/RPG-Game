using TMPro;
using UnityEngine;
using System.Collections;

/*Script responsável por mostrar o Dano dado aos inimigos*/
public class DamagePopup : MonoBehaviour
{
    private const float DISAPPEAR_SPEED = 3f;

    /*Constante responsavel por definir o tempo máximo antes do Dano desaparecer*/
    private const float DISSAPEAR_TIMER_MAX = 1f;

    /*A variavel é Static pois o valor será partilhado em todas as instâncias desta Classe*/
    private static int SortingOrder;

    private TextMeshPro damageTXT;
    private float disappearTimer;
    private Color TxtColor;
    private Vector3 moveVector;

    private void Awake()
    {
        damageTXT = GetComponent<TextMeshPro>();
    }

    /*Função que cria O damage Popup com os seguintes parametros: Posicao, Quantidade de Dano e uma
     bool para se é crítico ou nao
    
     Função Static significa que não precisa de ter uma instancia para podermos usar, logo podemos chama-la através
    do nome da Classe como por exemplo: DamagePopup.Create ou inves de DamagePopup.GetInstance().Create*/
    public static DamagePopup Create(Vector3 Pos, int Damage_Amount, bool IsCritical) 
    {
        Transform DamagePopup_TR = Instantiate(GameAssets.GetInstance().DamagePopupPref, Pos, Quaternion.identity);
        DamagePopup damagePopup = DamagePopup_TR.GetComponent<DamagePopup>();

        damagePopup.Setup(Damage_Amount, IsCritical);
        return damagePopup;
    }
    public void Setup(int damage_amount, bool IsCritical) 
    {
        /*Definimos o nosso texto igual ao Dano*/
        damageTXT.text = damage_amount.ToString();

        /*Se for Critico aparece vermelho e font maior
         caso contrario amarelo e font menor*/
        if (IsCritical) 
        {
            damageTXT.fontSize = 6;
            TxtColor = Color.red;
        }
        else
        {
            damageTXT.fontSize = 5;
            TxtColor = Color.yellow;
        }
        /*Definicao da cor do Damage Popup Text de acordo com a condição anterior*/
        damageTXT.color = TxtColor;

        /*Variavel que tem o valor do tempo máximo para dar fade o texto*/
        disappearTimer = DISSAPEAR_TIMER_MAX;

        /*Vector usado para movimentar o texto*/
        moveVector = new Vector3(0.7f, 1f) * 3f;

        /*Cada novo popup vai estar sempre sobreposto ao anterior*/
        SortingOrder++;
        damageTXT.sortingOrder = SortingOrder;
    }

    private void Update()
    {
        /*Movimentacao do Texto para o lado um bocado*/
        transform.position += moveVector * Time.deltaTime;

        /*Volta à posicao original mais lentamente*/
        moveVector -= moveVector * 8f * Time.deltaTime;

        /*O texto aumenta na 1 parte e diminui na 2 parte*/
        if(disappearTimer > DISSAPEAR_TIMER_MAX * 0.5f) 
        {
            /*1 PARTE DO FASEAMENTO DO DAMAGE TEXT*/
            transform.localScale += Vector3.one * 1f * Time.deltaTime;
        }
        else 
        {
            /*2 PARTE E ULTIMA DO FASEAMENTO DO DAMAGE TEXT*/
            transform.localScale -= Vector3.one * 1f * Time.deltaTime;
        }


        /*CRONOMETRO ATÉ EXECUTAR O CODIGO*/
        disappearTimer -= Time.deltaTime;        
        if(disappearTimer < 0) 
        {
            /*O VALOR ALFA VAI DECRESCENDO*/
            TxtColor.a -= DISAPPEAR_SPEED * Time.deltaTime;
            damageTXT.color = TxtColor;

            /*Se o alpha value chegou a zero 
             podemos destruir o gameobject*/
            if(TxtColor.a < 0) 
            {
                Destroy(gameObject);
            }
        }
    }
}

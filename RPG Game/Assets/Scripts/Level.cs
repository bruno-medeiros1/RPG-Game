using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    /*Variavel que as medidas da Arena*/
    private const float ARENA_SIZE = 13f;
    
    /*Variaveis que dão hold dos valores para
     o cronometro de spawn dos MoleEnemies*/
    private float MoleSpawningTime;
    private float MoleMaxSpawningTime;

    private float enemies_spawned;

    public event EventHandler OnNewLevel;

    public int level;
    
    /*Lista dos Moles Criados*/
    private List<Transform> Mole_Enemy_List;

    private GameStatus Status;
    private Text MolesAlive_TXT;
    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        Mole_Enemy_List = new List<Transform>(); //Inicialização da lista de 
        Status = GameStatus.Adventure;
        level = 0;
        enemies_spawned = 0;
        MoleMaxSpawningTime = 5f;
        MolesAlive_TXT = GameObject.Find("MolesAliveTXT").GetComponent<Text>();
    }
    private void Start()
    { 
        /*Subscrever ao evento de começar Battle Mode*/
        Player.GetInstance().OnStartBattle += Level_StartBattle;

        /*Subscricao ao evento de PlayerDied*/
        Player.GetInstance().OnDied += Level_OnDied;

        MolesAlive_TXT.text = "Alive: " + Mole_Enemy_List.Count;
    }

    private void OnMoleDie(object sender, EventArgs e)
    {
        /*Quando um Mole morre é removida uma posição da Lista para
         assim a contagem da lista ter sempre o nº de moles vivos*/
        Mole_Enemy_List.RemoveAt(0);
        MolesAlive_TXT.text = "Alive: " + Mole_Enemy_List.Count;
    }

    private void Level_OnDied(object sender, EventArgs e)
    {
        Status = GameStatus.End;
    }

    private void Level_StartBattle(object sender, EventArgs e)
    {
        SoundManager.GetInstance().Stop("Birds");
        SoundManager.GetInstance().Stop("GameTheme");
        SoundManager.GetInstance().Play("BossFight_Theme");
        Status = GameStatus.Battle;
        level = 1;
        MoleSpawningTime = MoleMaxSpawningTime;
        Debug.Log("MODO BATALHA!");
    }

    private void Update()
    {
        switch (Status) 
        {
            default:
                break;
      
                /*Modo Aventura o Player só explora o Mapa não existem enimigos, possiveis quests*/
            case GameStatus.Adventure:
                break;

                /*Modo Aventura terá várias Waves (facil, medio, dificil e impossivel)
                 a cada nivel a vida dos enimigos aumenta, velocidade e o dano*/
            case GameStatus.Battle:
                HandleMoleSpawning();
                break;
                /*Aqui o Player morreu terá de reiniciar e voltar ao modo Aventura!*/
            case GameStatus.End:
                SoundManager.GetInstance().Stop("BossFight_Theme");

                foreach(Transform tr in Mole_Enemy_List) 
                {
                    tr.GetComponent<EnemyAI>().Stop();
                }
                break;
        }
    }

    private enum Difficulty
    {
        easy,
        medium,
        hard,
    }
    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.easy:
                MoleMaxSpawningTime = 9f;

                break;
            case Difficulty.medium:
                MoleMaxSpawningTime = 7f;

                break;
            case Difficulty.hard:
                MoleMaxSpawningTime = 6f;

                break;
        }
    }
    private Difficulty GetDifficulty()
    {
        if (enemies_spawned >= 8) return Difficulty.hard;
        if (enemies_spawned >= 5) return Difficulty.medium;
        return Difficulty.easy;

    }
    private enum GameStatus
    {
        Adventure,
        Battle,
        End,
    }
    /*Função que spawna os Moles Enemies a cada 5s*/
    private void HandleMoleSpawning() 
    {
        MoleSpawningTime -= Time.deltaTime;
        if (MoleSpawningTime < 0)
        {
            MoleSpawningTime += MoleMaxSpawningTime;
            SpawnMole();
            SetDifficulty(GetDifficulty());
        }
    }

    private void SpawnMole() 
    {
        Transform MoleTr;
        
        MoleTr = Instantiate(GameAssets.GetInstance().MoleEnemy, new Vector3(-9f, 12f, 0f), Quaternion.identity);
        /*Subscricao ao Evento OnDie do Mole*/
        Mole.GetInstance().OnDie += OnMoleDie;

        Mole_Enemy_List.Add(MoleTr);

        /*Atualiza o Valor do TXT*/
        MolesAlive_TXT.text = "Alive: " + Mole_Enemy_List.Count;

        enemies_spawned++;
        Debug.Log("MOLE SPAWNADO...");
        /*Trigger para oo Event NewLevel*/
        if(enemies_spawned == 5) 
        {
            level = 2;
            OnNewLevel?.Invoke(this, EventArgs.Empty);
        }
        if(enemies_spawned == 8) 
        {
            level = 3;
            OnNewLevel?.Invoke(this, EventArgs.Empty);
        }
    }
}


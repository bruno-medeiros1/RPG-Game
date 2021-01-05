using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] Sounds;
    private static SoundManager instance;

    public static SoundManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        /*Esta condicao serve para verificar se quando mudamos de scene caso existam 2 AudioManager isso ja 
         nao vai ser possivel graças a este codigo pois se existir 1 o outro já será destruido.*/
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);//opcao para a musica continuar sem cortes caso haja uma mudança de scene
        foreach (Sound s in Sounds)
        {
            /*Inicializar o AudioSource dos nossos elementos da array com os atributos da classe Sound*/
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.name = s.name;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.clip = s.clip;
            s.source.mute = s.mute;
        }
    }
    private void Start()
    {
       Play("Birds");
       Play("GameTheme");

    }
    /*Função que toca um som*/
    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, Sounds => Sounds.name == name);
        if (s == null)
        {
            Debug.LogError("Som: " + name + " nao existe");
            return;
        }
        s.source.Play();

        /*Função que pará um Som*/
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, Sounds => Sounds.name == name);
        if (s == null)
        {
            Debug.LogError("Som: " + name + " nao existe");
            return;
        }
        s.source.Stop();
    }
}
[System.Serializable]
public class Sound
{
    /*Definição dos atribuitos da classe*/
    public string name;

    [Range(0.1f, 3f)]
    public float pitch;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source; /*Referencia*/

    public AudioClip clip;
    public bool mute;
}
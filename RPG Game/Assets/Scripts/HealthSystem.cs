using System;
public class HealthSystem
{
    /*Vida do jogador*/
    private int health;
    /*Variavel que define o maximo de vida que o nosso character pode ter*/
    private int max_health;

    public event EventHandler OnHealthChange;
    public HealthSystem(int max_health) 
    {
        this.max_health = max_health;
        health = max_health;
    }

    /*Devolve a percentagem da vida para usarmos para encher a barra de vida*/
    public float GetHealthPercent() 
    {
        return (float) health / max_health;
    } 
    public int GetHealth() 
    {
        return health;
    }
    public int GetMaxHealth() 
    {
        return max_health;
    }
    

    public void Damage(int damage_amount) 
    {
        health -= damage_amount;
        if (health < 0) health = 0;
        /*Inicializacao do evento */
        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int heal_amount) 
    {
        health += heal_amount;
        if (health > max_health) health = max_health;
        /*Inicializacao do evento */
        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }
}

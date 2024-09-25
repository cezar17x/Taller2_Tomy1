using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // Salud máxima del jugador
    public int currentHealth; // Salud actual del jugador

    void Start()
    {
        currentHealth = maxHealth; // Iniciar la salud con el valor máximo
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Restar daño a la salud actual
        Debug.Log("Player took damage. Current health: " + currentHealth);

        // Si la salud del jugador llega a cero, ejecuta la función Die()
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Aquí puedes agregar la lógica de muerte, como reiniciar el nivel, mostrar un mensaje, etc.
        Debug.Log("Player has died.");
    }
}

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // Salud m�xima del jugador
    public int currentHealth; // Salud actual del jugador

    void Start()
    {
        currentHealth = maxHealth; // Iniciar la salud con el valor m�ximo
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Restar da�o a la salud actual
        Debug.Log("Player took damage. Current health: " + currentHealth);

        // Si la salud del jugador llega a cero, ejecuta la funci�n Die()
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Aqu� puedes agregar la l�gica de muerte, como reiniciar el nivel, mostrar un mensaje, etc.
        Debug.Log("Player has died.");
    }
}

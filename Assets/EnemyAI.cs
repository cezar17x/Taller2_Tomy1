using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseAndAttack : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float attackRange = 2f; // Distancia de ataque
    public float attackCooldown = 1.5f; // Tiempo entre ataques
    public int damagePerAttack = 1; // Daño por ataque
    private NavMeshAgent agent; // Referencia al NavMeshAgent del enemigo
    private float attackTimer; // Temporizador para controlar la frecuencia de ataque

    void Start()
    {
        // Inicializar componentes
        agent = GetComponent<NavMeshAgent>();

        // Buscar al jugador por tag
        player = GameObject.FindWithTag("Player").transform;

        // Si no encuentra al jugador, lanzar advertencia
        if (player == null)
        {
            Debug.LogError("No se encontró al jugador. Asegúrate de que el jugador tiene el tag 'Player'.");
        }

        attackTimer = 0f; // Iniciar el temporizador
    }

    void Update()
    {
        if (player == null) return; // Si no encuentra al jugador, salir de Update

        // Mover al enemigo hacia el jugador
        agent.SetDestination(player.position);

        // Calcular la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si está dentro del rango de ataque, atacar
        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true; // Detener el movimiento del enemigo

            // Si ha pasado el tiempo de cooldown, atacar al jugador
            if (attackTimer <= 0f)
            {
                AttackPlayer(); // Llamar a la función de ataque
                attackTimer = attackCooldown; // Reiniciar el cooldown
            }
        }
        else
        {
            agent.isStopped = false; // Continuar persiguiendo al jugador si está fuera del rango de ataque
        }

        attackTimer -= Time.deltaTime; // Reducir el temporizador
    }

    void AttackPlayer()
    {
        // Comprobar si el jugador tiene el componente PlayerHealth
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Enemy attacks the player.");
            playerHealth.TakeDamage(damagePerAttack); // Restar vida al jugador
        }
        else
        {
            Debug.LogError("No se encontró el componente PlayerHealth en el jugador.");
        }
    }
}

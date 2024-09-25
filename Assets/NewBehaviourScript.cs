using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Deteccion : MonoBehaviour
{
    public Transform player;              // El jugador que el enemigo intentará detectar
    public float detectionRange = 10f;    // Rango de detección del rayo
    public float moveSpeed = 3f;          // Velocidad de movimiento del enemigo
    public float rotationSpeed = 50f;     // Velocidad de rotación del enemigo (para caminar en círculos)
    public float chaseSpeed = 5f;         // Velocidad de persecución del jugador
    public float stopDistance = 2f;       // Distancia mínima que el enemigo mantendrá con el jugador
    public LayerMask playerLayer;         // Capa del jugador (para filtrar la detección del rayo)

    public Animator animator;             // Referencia al Animator del enemigo
    public Collider attackCollider1;      // Primer collider que se activará para el golpe
    public Collider attackCollider2;      // Segundo collider que se activará para el golpe

    public int maxHealth = 3;             // Vida máxima del enemigo
    private int currentHealth;            // Vida actual del enemigo

    private bool isPlayerDetected = false;
    private bool isDead = false;          // Verificar si el enemigo ya ha muerto
    public ParticleSystem particula;
    public Image healthBarImage;
    public GameObject healthBarCanvas;

    void Start()
    {
        currentHealth = maxHealth;        // Inicializar la vida del enemigo

        // Asegúrate de que ambos colliders del ataque estén desactivados al inicio
        if (attackCollider1 != null)
        {
            attackCollider1.enabled = false;
        }
        if (attackCollider2 != null)
        {
            attackCollider2.enabled = false;
        }
        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (isDead) return; // Si el enemigo está muerto, no debe moverse ni detectar al jugador

        // Si no ha detectado al jugador, caminar en círculos
        if (!isPlayerDetected)
        {
            PatrolInCircle();
            DetectPlayer(); // Detectar jugador mientras patrulla
        }
        else
        {
            ChasePlayer(); // Perseguir al jugador cuando es detectado
        }
        //healthBarImage.transform.position = transform.position + Vector3.up;
    }

    void PatrolInCircle()
    {
        // Rotar el enemigo sobre su eje Y para hacer que camine en círculos
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        // Mover el enemigo hacia adelante en su dirección actual
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void DetectPlayer()
    {
        // Detectar al jugador en múltiples direcciones
        RaycastHit hit;

        // Dirección forward
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                isPlayerDetected = true;
                Debug.Log("Jugador Detectado por adelante");
                return;
            }
        }

        // Dirección forward-right (diagonal derecha)
        Vector3 forwardRight = (transform.forward + transform.right).normalized;
        if (Physics.Raycast(transform.position, forwardRight, out hit, detectionRange, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                isPlayerDetected = true;
                Debug.Log("Jugador Detectado por adelante-derecha");
                return;
            }
        }

        // Dirección right (derecha)
        if (Physics.Raycast(transform.position, transform.right, out hit, detectionRange, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                isPlayerDetected = true;
                Debug.Log("Jugador Detectado por la derecha");
                return;
            }
        }
    }

    void ChasePlayer()
    {
        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Solo moverse hacia el jugador si está más lejos que la distancia mínima
        if (distanceToPlayer > stopDistance)
        {
            // Obtener la dirección hacia el jugador
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            // Moverse hacia el jugador
            transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;

            // Rotar hacia el jugador para simular que lo sigue
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Si está a la distancia mínima, reproducir animación de golpe y activar ambos colliders
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Atacando al jugador");

        // Reproducir animación de golpe
        if (animator != null)
        {
            animator.Play("golperobot");
        }

        // Activar ambos colliders de ataque (golpe)
        if (attackCollider1 != null && attackCollider2 != null)
        {
            attackCollider1.enabled = true;
            attackCollider2.enabled = true;

            // Desactivar ambos colliders después de un breve período para simular un ataque
            StartCoroutine(DisableAttackCollidersAfterTime(0.5f)); // Ajusta el tiempo según tu animación
        }
    }

    IEnumerator DisableAttackCollidersAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (attackCollider1 != null)
        {
            attackCollider1.enabled = false;
        }
        if (attackCollider2 != null)
        {
            attackCollider2.enabled = false;
        }
    }

    // Sistema de vida: cuando el enemigo colisione con algo, pierde vida
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ataque")) // Verifica si colisiona con un ataque del jugador
        {
            healthBarCanvas.SetActive(true);
            TakeDamage(1);
            particula.Play();
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemigo ha recibido daño. Vida actual: " + currentHealth);

        // Activar la barra de vida solo cuando recibe el primer golpe
        if (healthBarCanvas != null && !healthBarCanvas.activeSelf)
        {
            healthBarCanvas.SetActive(true);
        }

        // Actualizar la barra de vida
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemigo muerto");
        Collider col = GetComponent<Collider>();
        // Reproducir animación de muerte
        if (animator != null && col != null)
        {
            animator.Play("muerto"); // Asegúrate de tener una animación llamada "Death"
            col.enabled = false;
        }

        isDead = true; // El enemigo ya no detectará ni se moverá
    }

    // Puedes dibujar los rayos en la escena para depuración
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
        Gizmos.DrawRay(transform.position, (transform.forward + transform.right).normalized * detectionRange);
        Gizmos.DrawRay(transform.position, transform.right * detectionRange);
    }
}
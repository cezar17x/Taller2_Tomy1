using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Jefe : MonoBehaviour
{
    public Transform player; // Asigna el jugador desde el inspector
    public float shootingRange = 10f; // Rango de disparo
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform shootingPoint; // Punto desde donde se dispara
    public float bulletSpeed = 20f; // Velocidad de la bala
    public float shootingCooldown = 1.5f; // Tiempo entre disparos
    public List<Transform> patrolPoints; // Lista de puntos a los que se moverá
    public float moveSpeed = 3f; // Velocidad de movimiento
    public float waitTimeAtPoint = 2f; // Tiempo que espera en cada punto
    public float reloadTime = 3f; // Tiempo que dura la animación de recarga
    public Animator animator; // El componente Animator para reproducir animaciones
    public float rotationSpeed = 5f; // Velocidad de rotación hacia el jugador
    public LayerMask playerLayer; // El Layer del jugador

    public bool canShoot = true; // Controla si puede disparar
    private Transform currentPatrolPoint; // Punto actual de patrullaje
    //private int patrolIndex = 0; // Índice del punto actual
    private bool isReloading = false; // Controla si está recargando
    //private bool isDead = false;          // Verificar si el enemigo ya ha muerto
    public ParticleSystem particula;
    public int maxHealth = 3;     
    private int currentHealth;

    void Start()
    {
        if (patrolPoints.Count > 0)
        {
            currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        }
    }

    void Update()
    {
        if (!isReloading)
        {
            Patrol();
            CheckForPlayerAndShoot();
        }
        RotateTowardsPlayer(); // Rotar hacia el jugador
    }

    void Patrol()
    {
        if (currentPatrolPoint == null) return;

        // Mover hacia el punto de patrullaje
        transform.position = Vector3.MoveTowards(transform.position, currentPatrolPoint.position, moveSpeed * Time.deltaTime);

        // Si llega al punto, espera un tiempo, reproduce la animación de recarga y elige un nuevo punto
        if (Vector3.Distance(transform.position, currentPatrolPoint.position) < 0.2f)
        {
            StartCoroutine(WaitAndReload());
        }
    }

    IEnumerator WaitAndReload()
    {
        isReloading = true;
        currentPatrolPoint = null;

        // Reproduce la animación de recarga
        animator.Play("Reload");
        animator.SetBool("Vuelve", true);

        // Espera a que termine la animación de recarga
        yield return new WaitForSeconds(reloadTime);

        currentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        isReloading = false;
    }

    void CheckForPlayerAndShoot()
    {
        if (isReloading) return; // No puede disparar si está recargando

        // Verifica si el jugador está dentro del rango de disparo
        if (Vector3.Distance(transform.position, player.position) <= shootingRange)
        {
            // Raycast desde el enemigo hacia el jugador, detectando solo la capa del jugador
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, shootingRange, playerLayer))
            {
                // Si golpea al jugador en la capa correcta, dispara
                if (hit.transform == player && canShoot)
                {
                    print("disparo");
                    StartCoroutine(Shoot());
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        // Reproduce la animación de disparo
        animator.Play("Shoot_BurstShot_AR");
        animator.SetBool("Vuelve", true);

        // Instancia la bala
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Agrega velocidad a la bala
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootingPoint.forward * bulletSpeed;
        }

        // Espera hasta que pueda disparar de nuevo
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    void RotateTowardsPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        // Calcula la rotación deseada
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        // Rota suavemente hacia la rotación deseada
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el rango de disparo en la escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ataque")) // Verifica si colisiona con un ataque del jugador
        {
            TakeDamage(1); // El enemigo pierde 1 de vida
            particula.Play();
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemigo ha recibido daño. Vida actual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // Si la vida llega a 0, el enemigo muere
        }
    }

    void Die()
    {
        Debug.Log("Enemigo muerto");

        // Reproducir animación de muerte
        if (animator != null)
        {
            animator.Play("Die"); // Asegúrate de tener una animación llamada "Death"
        }

        //isDead = true; // El enemigo ya no detectará ni se movera

        // Desactivar el objeto enemigo después de la animación de muerte (opcional)
        //StartCoroutine(DestroyAfterDeathAnimation());
    }
}
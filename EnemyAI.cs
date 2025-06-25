using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform player;              // Ссылка на игрока
    public float moveSpeed = 3f;          // Скорость движения
    public float attackRange = 2f;        // Радиус атаки
    public int attackDamage = 15;         // Урон за атак
    public float attackCooldown = 1f;     // Перезарядка между атаками

    [Header("Detection")]
    public float detectionRange = 10f;    // Радиус обнаружения игрока

    private float lastAttackTime;
    private bool playerInRange = false;

    void Start()
    {
        // Автоматически найти игрока, если не назначен
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверяем, находится ли игрок в зоне обнаружения если похуй то плохо
        if (distanceToPlayer <= detectionRange)
        {
            // Проверяем, что игрок жив
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.IsDead())
            {
                // Если игрок мертв, прекращаем преследование
                return;
            }

            // Если игрок в радиусе атаки
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                // Двигаемся к игроку
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Направление к игроку
        Vector3 direction = (player.position - transform.position).normalized;

        // Перемещение
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Поворот в сторону игрока
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void AttackPlayer()
    {
        // Проверяем перезарядку атаки
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Проверяем, что игрок не мертв
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"Enemy dealt {attackDamage} damage to player!");

                lastAttackTime = Time.time;

                // Анимация атаки
                PlayAttackAnimation();
            }
        }
    }

    void PlayAttackAnimation()
    {
        // Пример простой анимации атаки (изменение размера)
        StartCoroutine(AttackAnimationCoroutine());
    }

    System.Collections.IEnumerator AttackAnimationCoroutine()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 attackScale = originalScale * 1.2f;

        // Увеличиваем размер
        float t = 0;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, attackScale, t / 0.1f);
            yield return null;
        }

        // Возвращаем размер
        t = 0;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(attackScale, originalScale, t / 0.1f);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    // Визуализация зон в редакторе
    void OnDrawGizmosSelected()
    {
        // Зона атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Зона обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

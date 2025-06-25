using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform player;              // ������ �� ������
    public float moveSpeed = 3f;          // �������� ��������
    public float attackRange = 2f;        // ������ �����
    public int attackDamage = 15;         // ���� �� ����
    public float attackCooldown = 1f;     // ����������� ����� �������

    [Header("Detection")]
    public float detectionRange = 10f;    // ������ ����������� ������

    private float lastAttackTime;
    private bool playerInRange = false;

    void Start()
    {
        // ������������� ����� ������, ���� �� ��������
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

        // ���������, ��������� �� ����� � ���� ����������� ���� ����� �� �����
        if (distanceToPlayer <= detectionRange)
        {
            // ���������, ��� ����� ���
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.IsDead())
            {
                // ���� ����� �����, ���������� �������������
                return;
            }

            // ���� ����� � ������� �����
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                // ��������� � ������
                MoveTowardsPlayer();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // ����������� � ������
        Vector3 direction = (player.position - transform.position).normalized;

        // �����������
        transform.position += direction * moveSpeed * Time.deltaTime;

        // ������� � ������� ������
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void AttackPlayer()
    {
        // ��������� ����������� �����
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // ���������, ��� ����� �� �����
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead())
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"Enemy dealt {attackDamage} damage to player!");

                lastAttackTime = Time.time;

                // �������� �����
                PlayAttackAnimation();
            }
        }
    }

    void PlayAttackAnimation()
    {
        // ������ ������� �������� ����� (��������� �������)
        StartCoroutine(AttackAnimationCoroutine());
    }

    System.Collections.IEnumerator AttackAnimationCoroutine()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 attackScale = originalScale * 1.2f;

        // ����������� ������
        float t = 0;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, attackScale, t / 0.1f);
            yield return null;
        }

        // ���������� ������
        t = 0;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(attackScale, originalScale, t / 0.1f);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    // ������������ ��� � ���������
    void OnDrawGizmosSelected()
    {
        // ���� �����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ���� �����������
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

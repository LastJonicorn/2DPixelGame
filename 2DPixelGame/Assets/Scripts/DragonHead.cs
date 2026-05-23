using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class DragonHead : MonoBehaviour
{
    enum HeadState
    {
        Idle,
        MoveToAttack,
        Attack
    }
    private Transform player;
    private bool bossFightStarted = false;
    private bool isDead = false;
    public Enemy enemy; // käyttää existing HP systeemiä

    public BossHealthBar healthBar;

    public Collider2D bossArea;
    public CinemachineCamera cameraA;
    public CinemachineCamera cameraB;

    public Transform idleCenter;
    public Transform attackPoint;
    public GameObject fireballPrefab;
    public Transform fireSpawnPoint;

    public float idleMoveAmount = 0.3f;
    public float idleSpeed = 1.5f;

    public float moveSpeed = 4f;
    public float shootDelay = 0.5f;
    public float attackCooldown = 3f;

    private Vector3 idleTarget;
    private float nextAttackTime;

    private enum State { Idle, MoveToAttack, Attack }
    private State state;

    void Start()
    {
        state = State.Idle;
        idleTarget = idleCenter.position;

        if (enemy == null)
            enemy = GetComponent<Enemy>();

        FindPlayer();

        if (healthBar != null)
        {
            healthBar.SetBoss(enemy);
            healthBar.Hide();
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                HandleIdle();

                if (Time.time >= nextAttackTime)
                {
                    state = State.MoveToAttack;
                }
                break;

            case State.MoveToAttack:
                MoveToAttackPoint();
                break;

            case State.Attack:
                break;
        }

        //-------------------------------------------
        if (player == null)
        {
            FindPlayer();
            return;
        }

        if (bossArea == null) return;

        bool playerInside = bossArea.bounds.Contains(player.position);

        if (playerInside && !bossFightStarted)
        {
            bossFightStarted = true;

            if (healthBar != null)
                healthBar.Show();
        }

        if (playerInside && cameraA != null && cameraB != null)
        {
            ActivateCameraB();
        }
        else
        {
            ActivateCameraA();
        }
    }
    void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }
    public void ActivateCameraB()
    {
        cameraB.gameObject.SetActive(true);
        cameraA.gameObject.SetActive(false);
    }

    public void ActivateCameraA()
    {
        cameraA.gameObject.SetActive(true);
        cameraB.gameObject.SetActive(false);
    }

    void HandleIdle()
    {
        float x = Mathf.Sin(Time.time * idleSpeed) * idleMoveAmount;
        float y = Mathf.Cos(Time.time * idleSpeed * 0.8f) * idleMoveAmount;

        Vector3 target = idleCenter.position + new Vector3(x, y, 0f);

        transform.position = Vector3.Lerp(transform.position, target, 2f * Time.deltaTime);
    }

    void MoveToAttackPoint()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            attackPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, attackPoint.position) < 0.05f)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        state = State.Attack;

        yield return new WaitForSeconds(shootDelay);

        ShootFireball();

        yield return new WaitForSeconds(attackCooldown);

        nextAttackTime = Time.time + attackCooldown;
        state = State.Idle;
    }

    void ShootFireball()
    {
        GameObject fireball = Instantiate(
            fireballPrefab,
            fireSpawnPoint.position,
            Quaternion.identity
        );

        Vector3 dir = -(attackPoint.position - fireSpawnPoint.position).normalized;

        fireball.GetComponent<FireballProjectile>().SetDirection(dir);
    }

    public void Die()
    {
        ActivateCameraA();

        isDead = true;

        healthBar.Hide();

        StopAllCoroutines();

        DragonHead[] bosses = FindObjectsByType<DragonHead>();

        bool anyAlive = false;

        foreach (DragonHead boss in bosses)
        {
            if (boss != this && !boss.isDead)
            {
                anyAlive = true;
                break;
            }
        }

        if (!anyAlive)
        {
            //Platform logiikka
        }

        this.enabled = false;
    }
}
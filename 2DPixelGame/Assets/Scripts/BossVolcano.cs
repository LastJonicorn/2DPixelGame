using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class BossVolcano : MonoBehaviour
{
    public Animator animator;
    public EnemyPatrol enemyPatrol;
    public BossHealthBar healthBar;

    [Header("Fire Attack")]
    public GameObject firePrefab;
    public Transform fireLocation;
    private GameObject fireInstance;
    public float fireDuration = 2.5f;
    public float fireCooldown = 3f;

    [Header("State")]
    public Enemy enemy; // käyttää existing HP systeemiä

    private bool isFiring = false;
    private bool vulnerable = true;

    private Transform player;
    private bool bossFightStarted = false;
    private bool isDead = false;

    public Collider2D movementBounds;
    public CinemachineCamera cameraA;
    public CinemachineCamera cameraB;

    void Start()
    {
        if (enemy == null)
            enemy = GetComponent<Enemy>();

        FindPlayer();

        if (healthBar != null)
        {
            healthBar.SetBoss(enemy);
            healthBar.Hide();
        }

        StartCoroutine(FireLoop());
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        if (movementBounds == null) return;

        bool playerInside = movementBounds.bounds.Contains(player.position);

        if (playerInside && !bossFightStarted)
        {
            bossFightStarted = true;

            if (healthBar != null)
                healthBar.Show();

            // halutessa eventit
            Events.OnDoorEvent?.Invoke("TrapDoor");
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

    void FindPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }

    IEnumerator FireLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireCooldown);

            yield return StartCoroutine(FirePhase());
        }
    }

    IEnumerator FirePhase()
    {
        if (enemyPatrol != null)
            enemyPatrol.enabled = false;

        animator.SetTrigger("FireVolcano");

        isFiring = true;
        vulnerable = false;

        // spawn fire object
        fireInstance = Instantiate(
            firePrefab,
            fireLocation.position,
            Quaternion.identity
        );

        float timer = 0f;

        while (timer < fireDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // destroy fire object
        if (fireInstance != null)
        {
            Destroy(fireInstance);
        }

        vulnerable = true;
        isFiring = false;

        if (enemyPatrol != null)
            enemyPatrol.enabled = true;
    }

    public void Die()
    {
        ActivateCameraA();

        isDead = true;

        healthBar.Hide();

        StopAllCoroutines();

        Events.OnDoorEvent?.Invoke("BossDoor");

        this.enabled = false; // lopettaa Update kokonaan
    }
}
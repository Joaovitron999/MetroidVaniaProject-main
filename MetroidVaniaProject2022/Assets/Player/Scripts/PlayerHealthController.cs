using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private float invincibilityTime = 1f;

    [SerializeField] private UICounterData lifeCounter;

    //Text mesh pro to test the health
    [SerializeField] private TextMeshProUGUI healthText;


    private float invincibilityTimeCounter;

    [SerializeField] private CheckPoint checkPoint;
    private bool isInvincible = false;


    private void LoadPersistHealth()
    {
        if(MainManager.Instance.health > 0 && MainManager.Instance.health >= 0)
        {
            currentHealth = MainManager.Instance.health;
            maxHealth = MainManager.Instance.maxHealth;
        }
    }

    public void QuickDamage()
    {
        isInvincible = true;
        invincibilityTimeCounter = invincibilityTime;
        currentHealth--;

        if (currentHealth <= 0)
        {
            lifeCounter.SetCurrentQuantity(0);
            checkPoint.ResetPersistPos();
            Die();
        }
        else
        {   
            
            lifeCounter.SetCurrentQuantity(currentHealth);
            //Save position
            checkPoint.PersistPos();
            //reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //save health
        MainManager.Instance.health = currentHealth;
        MainManager.Instance.maxHealth = maxHealth;
    }

    private void Die()
    {
        //Animation
        //Die
        //reload scene
        MainManager.Instance.health = maxHealth;
        MainManager.Instance.maxHealth = maxHealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    private void Start()
    {
        currentHealth = maxHealth;
        checkPoint = transform.GetComponent<CheckPoint>();
        LoadPersistHealth();
        lifeCounter.SetMaxQuantity(maxHealth);
        lifeCounter.SetCurrentQuantity(currentHealth);
    }

    //awake

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimeCounter -= Time.deltaTime;
            if (invincibilityTimeCounter <= 0)
            {
                isInvincible = false;
            }
        }

        healthText.text = currentHealth.ToString();
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            invincibilityTimeCounter = invincibilityTime;
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                lifeCounter.SetCurrentQuantity(0);
                checkPoint.ResetPersistPos();
                Die();
            }
            else
            {
                lifeCounter.SetCurrentQuantity(currentHealth);
            }
            //save health
            MainManager.Instance.health = currentHealth;
            MainManager.Instance.maxHealth = maxHealth;

            //knockback
            StartCoroutine(Knockback(attackDirection));
        }
    }

    private IEnumerator Knockback(Vector2 attackDirection)
    {
        //testa se o ataque veio da direita ou da esquerda
        /* FAZER DPS
        if (attackDirection.x > transform.position.x)
        {
            PlayerController.Instance.Knockback(-1);
        }
        else
        {
            PlayerController.Instance.Knockback(1);
        }
        yield return new WaitForSeconds(invincibilityTime);
        PlayerController.Instance.Knockback(0);
        */
        yield return null;
    }

}
  

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int health = 3;
    public int gifts = 1;
    public LayerMask wallLayer;
    public LayerMask giftLayer;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject moveButtons;
    public GameObject pauseButton;
    public GameObject tutorialButton;
    public GameObject[] hearts;
    public GameObject heartsPanel;
    public AudioSource victorySound;
    public AudioSource holesSound;
    public AudioSource gameOverSound;
    public AudioSource giftSound;
    public SpriteRenderer spriteRenderer;
    public GameObject giftsPanel;
    public TMP_Text giftsCounter;
    public TMP_Text allGifts;

    private bool isOnActiveSpikes = false;
    private int counter = 0;

    private void Start()
    {
        allGifts.text = gifts.ToString();
    }

    public void MoveUp()
    {
        TryMove(Vector2.up, 0);
    }
    public void MoveDown()
    {
        TryMove(Vector2.down, 180);
    }
    public void MoveLeft()
    {
        TryMove(Vector2.left, 90);
    }
    public void MoveRight()
    {
        TryMove(Vector2.right, -90);
    }

    private void TryMove(Vector2 direction, float rotation)
    {
        Vector2 newPosition = (Vector2) transform.position + direction;
        if (CanMoveTo(newPosition))
        {
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            Collider2D hit = Physics2D.OverlapPoint(newPosition, giftLayer);
            if (hit != null)
            {
                OnGiftCollected(hit);
            }
        }
    }

    private bool CanMoveTo(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapPoint(position, wallLayer);
        if (hit != null)
        {
            return false;
        }

        return true;
    }

    private void TakeDamage(int damage)
    {
        Destroy(hearts[health - 1]);
        health -= damage;
        if (health <= 0)
        {
            moveButtons.SetActive(false);
            pauseButton.SetActive(false);
            giftsPanel.SetActive(false);
            if (tutorialButton != null) tutorialButton.SetActive(false);
            gameOverSound.Play();
            StartCoroutine(DestroyAfterSound());
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(gameOverSound.clip.length);
        gameOverPanel.SetActive(true);
        Destroy(gameObject);
        Time.timeScale = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            Hole hole = other.GetComponent<Hole>();
            if (hole != null && hole.IsActive() && !isOnActiveSpikes)
            {
                TakeDamage(1);
                holesSound.Play();
                isOnActiveSpikes = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            isOnActiveSpikes = false;
        }
    }

    private void OnGiftCollected(Collider2D gift)
    {
        Destroy(gift.gameObject);
        giftSound.Play();
        counter++;
        giftsCounter.text = counter.ToString();
        if (counter == gifts)
        {
            moveButtons.SetActive(false);
            pauseButton.SetActive(false);
            heartsPanel.SetActive(false);
            giftsPanel.SetActive(false);
            if (tutorialButton != null) tutorialButton.SetActive(false);
            PlayerPrefs.SetInt("LastCompletedLevel", Mathf.Max(PlayerPrefs.GetInt("LastCompletedLevel", 0), SceneManager.GetActiveScene().buildIndex - 1));
            victorySound.Play();
            StartCoroutine(WaitAndShowVictoryPanel());
        }
    }

    private IEnumerator WaitAndShowVictoryPanel()
    {
        yield return new WaitForSeconds(1);
        victoryPanel.SetActive(true);
    }
}
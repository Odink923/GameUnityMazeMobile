using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    // private Animator animator;
    public bool isDead = false;
    public VirtualJoystick joystick; // Додайте це поле

    void Start()
    {
        // animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Отримання вводу від джойстика
        float moveHorizontal = joystick.Horizontal();
        float moveVertical = joystick.Vertical();

        // Рух гравця
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0).normalized;
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        // animator.SetFloat("Speed", movement.magnitude);

         if (movement.x < 0)
    {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Повертаємо персонажа вліво
    }
    else if (movement.x > 0)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Повертаємо персонажа вправо
    }
    }

    public void Die()
    {
    //     if (!isDead)
    //     {
    //         isDead = true;
    //         animator.SetTrigger("Die");
    //         DeathCounter deathCounter = FindObjectOfType<DeathCounter>();
    //         if (deathCounter != null)
    //         {
    //             deathCounter.IncrementDeathCount();
    //         }
    //         Invoke("RestartLevel", 0.7f);
    //     }
    }

    void RestartLevel()
    {
        Debug.Log("Restarting level...");
        // isDead = false;
        // animator.ResetTrigger("Die");
        // animator.Play("Culm");
        // animator.SetFloat("Speed", 0);
        // MazeGenerator mazeGenerator = FindObjectOfType<MazeGenerator>();
        // if (mazeGenerator != null)
        // {
        //     mazeGenerator.GenerateAndDrawMaze();
        // }
        // Debug.Log("Level restarted successfully.");
    }
}

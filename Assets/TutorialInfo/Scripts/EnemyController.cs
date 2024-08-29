using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f; // Швидкість руху моба
    private Vector2 initialPosition; // Початкова позиція моба
    private Vector2 targetPosition; // Цільова позиція моба
    private bool movingForward = true; // Рух вперед або назад

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = SetRandomTargetPosition(); // Встановити випадкову початкову цільову позицію
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        // Рух моба до цільової позиції
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Якщо моб досягає цільової позиції, змінюємо напрямок руху
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            movingForward = !movingForward;
            targetPosition = movingForward ? initialPosition : SetRandomTargetPosition();
        }
    }

    Vector2 SetRandomTargetPosition()
    {
        // Встановлює випадкову цільову позицію в межах лабіринту, враховуючи поточні розміри лабіринту
        MazeGenerator mazeGenerator = FindObjectOfType<MazeGenerator>();
        if (mazeGenerator != null)
        {
            float randomX = Random.Range(1, mazeGenerator.width * 2 - 1); // Враховуємо множник масштабу
            float randomY = Random.Range(1, mazeGenerator.height * 2 - 1); // Враховуємо множник масштабу
            return new Vector2(randomX, randomY);
        }
        else
        {
            // Якщо лабіринт не знайдено, повертаємо початкову позицію
            return initialPosition;
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null && !playerController.isDead)
        {
            playerController.Die();
        }
    }
}
}

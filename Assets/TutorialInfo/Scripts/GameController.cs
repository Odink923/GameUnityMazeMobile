using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MazeGenerator mazeGenerator;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name +"other tag:"+ other.tag); // Додаємо цей рядок для налагодження

        if (other.CompareTag("Finish"))
        {
            Debug.Log("You Win!");
            mazeGenerator.IncreaseMazeSizeAndRegenerate();
        }
    }
}


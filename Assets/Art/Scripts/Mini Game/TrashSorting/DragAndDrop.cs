using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector2 initialPosition;
    private bool isDragging = false;
    private bool isOverBin = false;
    private GameObject targetBin;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x, mousePosition.y);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (isOverBin && targetBin != null)
        {
            TrashBin bin = targetBin.GetComponent<TrashBin>();
            if (bin != null && bin.IsCorrectBin(gameObject.tag))
            {
                // Correct bin
                GameManager.Instance.AddScore(10);
                Destroy(gameObject);
            }
            else
            {
                // Wrong bin
                GameManager.Instance.AddScore(-5);
                transform.position = initialPosition;
            }
        }
        else
        {
            // Not over any bin
            transform.position = initialPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TrashBin"))
        {
            isOverBin = true;
            targetBin = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TrashBin"))
        {
            isOverBin = false;
            targetBin = null;
        }
    }
}


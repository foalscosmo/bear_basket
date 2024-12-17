using System.Collections;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    private Camera mainCamera;

    private static bool isAnyItemDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isAnyItemDragging && !isDragging) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (IsTouchingObject(mousePos))
            {
                offset = transform.position - mousePos;
                isDragging = true;
                isAnyItemDragging = true;

                transform.SetAsLastSibling();
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
        }
    }

    private bool IsTouchingObject(Vector3 position)
    {
        Collider2D collider = GetComponent<Collider2D>();
        return collider == Physics2D.OverlapPoint(position);
    }

    private void EndDrag()
    {
        isDragging = false;
        isAnyItemDragging = false;

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(transform.position);
        bool droppedOnCorrectZone = false;

        foreach (Collider2D hitCollider in hitColliders)
        {
            DropZone dropZone = hitCollider.GetComponent<DropZone>();
            if (dropZone != null)
            {
                if (dropZone.CheckMatch(gameObject))
                {
                    droppedOnCorrectZone = true;
                    dropZone.OnCorrectDrop(gameObject);
                    break;
                }
                else
                {
                    dropZone.OnWrongDrop();
                }
            }
        }

        if (!droppedOnCorrectZone)
        {
            StartCoroutine(SmoothReturn());
        }

    }

    private IEnumerator SmoothReturn()
    {
        float time = 0f;
        float duration = 0.5f;
        Vector3 startPos = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, originalPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}

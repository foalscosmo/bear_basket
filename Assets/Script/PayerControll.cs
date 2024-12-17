using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    public float moveDistance = 10f; 
    public float moveSpeed = 5f;      

    public DropZone dropZone;

    public OrderSystem orderSystem;

    public bool isMoving = false; 
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (dropZone.val == 2 && !isMoving)
        {
            dropZone.gameObject.SetActive(false);
            orderSystem.gameObject.SetActive(false);
            StartCoroutine(MovePlayerSmoothly());
            dropZone.val = 0;
        }
    }

    private void DestroyRandomFoodPrefab()
    {
        GameObject randomFoodPrefab = GameObject.Find("RandomFood"); 
        if (randomFoodPrefab != null)
        {
            Destroy(randomFoodPrefab); 
        }
    }
    IEnumerator MovePlayerSmoothly()
    {
        yield return new WaitForSeconds(0.8f);
        isMoving = true;
        animator.SetBool("IsMoving", isMoving);
        float startX = transform.position.x;
        float targetX = startX + moveDistance; 

        float journeyLength = Mathf.Abs(targetX - startX);
        float startTime = Time.time;

        while (Mathf.Abs(transform.position.x - targetX) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed; 
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = new Vector3(Mathf.Lerp(startX, targetX, fractionOfJourney), transform.position.y, transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        isMoving = false;
        animator.SetBool("IsMoving", isMoving);
        if (orderSystem.now <= orderSystem.max)
        {
            dropZone.gameObject.SetActive(true);
            orderSystem.gameObject.SetActive(true);
        }

        Debug.Log("Stop");

    }
}

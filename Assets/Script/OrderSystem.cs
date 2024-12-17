using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class OrderSystem : MonoBehaviour
{
    [SerializeField] private RectTransform thinkingSprite;
    [SerializeField] private SpriteRenderer orderSpriteRenderer;
    [SerializeField] public Sprite[] foodSprites;
    [SerializeField] private DropZone dropZone;
    [SerializeField] private DraggableItem[] draggableItems;
    [SerializeField] private Vector2 offset = new Vector2(-120, 200);
    public int start = 6;
    public int end = 0;

    public int max = 3;
    public int now = 1;


    public string[] foodTags = { };

    private void Start()
    {
        GenerateNewOrder();
    }

    public void GenerateNewOrder()
    {
        int randomIndex = Random.Range(end, start);

        orderSpriteRenderer.sprite = foodSprites[randomIndex];
        dropZone.expectedItemTag = foodTags[randomIndex];
        Debug.Log("asdkuas " + randomIndex);

        
        AdjustFoodPositionAndScale();

        foreach (var item in draggableItems)
        {
            if (item.tag == dropZone.expectedItemTag) continue;
            int randomTagIndex = Random.Range(0, foodTags.Length);
            item.tag = foodTags[randomTagIndex];
        }
    }

    private void AdjustFoodPositionAndScale()
    {
        Vector2 thinkingPosition = thinkingSprite.anchoredPosition;
        //orderSpriteRenderer.transform.position = thinkingPosition + offset;

        orderSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);

        float thinkingWidth = thinkingSprite.rect.width;
        float thinkingHeight = thinkingSprite.rect.height;

        float foodWidth = orderSpriteRenderer.bounds.size.x;
        float foodHeight = orderSpriteRenderer.bounds.size.y;

        float scaleFactor = Mathf.Min(thinkingWidth / foodWidth, thinkingHeight / foodHeight);

        if (scaleFactor < 1)
        {
            orderSpriteRenderer.transform.localScale = new Vector3(0.065f, 0.065f, 1);
        }
        else
        {
            orderSpriteRenderer.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }
    }
    public void ShakeOrderSprite()
    {
        //StartCoroutine(ShakeCoroutine());
        orderSpriteRenderer.transform.DOShakePosition(0.5f, Vector2.left * 0.5f, vibrato: 15,randomness: 0, randomnessMode: ShakeRandomnessMode.Harmonic);
    }

    private IEnumerator ShakeCoroutine()
    {
        Vector3 originalPosition = orderSpriteRenderer.transform.localPosition;
        float duration = 0.3f;
        float magnitude = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-magnitude, magnitude);
            float yOffset = 0;

            orderSpriteRenderer.transform.localPosition = originalPosition + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime*2;
            yield return null;
        }

        orderSpriteRenderer.transform.localPosition = originalPosition;
    }
}

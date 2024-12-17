using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DropZone : MonoBehaviour
{
    public string expectedItemTag;

    public OrderSystem orderSystem;

    [SerializeField] private Cart cart;
    public int val = 0;

    public bool CheckMatch(GameObject draggable)
    {
        return draggable.CompareTag(expectedItemTag);
    }

    public void OnCorrectDrop(GameObject draggable)
    {   
        cart.Dropp();
        if (orderSystem.now <= orderSystem.max)
        {
            Debug.Log("Correct item delivered!");

            StartCoroutine(FlashColor(Color.green, 0.5f));
            StartCoroutine(FlashColor(Color.white, 0.01f));
            string itemTag = draggable.tag;
            orderSystem.foodTags = RemoveTagFromList(orderSystem.foodTags, itemTag);

            Sprite itemSprite = draggable.GetComponent<SpriteRenderer>().sprite;
            orderSystem.foodSprites = RemoveSpriteFromList(orderSystem.foodSprites, itemSprite);
            orderSystem.start -= 1;
            Destroy(draggable);
            val += 1;
            if (val == 2)
            {
                orderSystem.end += 4;
                orderSystem.start += 6;
                orderSystem.now += 1;
            }
            Debug.Log("start "+orderSystem.start);
            Debug.Log("end" + orderSystem.end);
            if (orderSystem.now <= orderSystem.max)
            {

                Debug.Log(orderSystem.start);
                FindObjectOfType<OrderSystem>().GenerateNewOrder();
            }

        }

    }

    public void OnWrongDrop()
    {
        Debug.Log("Wrong item delivered!");

        StartCoroutine(FlashColor(Color.red, 0.5f));
        orderSystem.ShakeOrderSprite();
    }

    public IEnumerator FlashColor(Color flashColor , float time)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;

        renderer.color = flashColor;
        yield return new WaitForSeconds(time);

        renderer.color = originalColor;
    }

    private string[] RemoveTagFromList(string[] tags, string tagToRemove)
    {
        return tags.Where(tag => tag != tagToRemove).ToArray();
    }

    private Sprite[] RemoveSpriteFromList(Sprite[] sprites, Sprite spriteToRemove)
    {
        return sprites.Where(sprite => sprite != spriteToRemove).ToArray();
    }
}

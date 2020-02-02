using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    public int yOffset = -200;
    public int offsetBetweenCards = 20;
    private int offsetBetweenCardsTotal;
    public int moveTime = 10;

    public GameObject cardPrefab;
    private List<RecipeCard> orderUIList;

    private int cardWidth;

    private void Start()
    {
        orderUIList = new List<RecipeCard>();
        cardWidth = (int)(cardPrefab.GetComponent<RectTransform>().sizeDelta.x * cardPrefab.transform.localScale.x);
        offsetBetweenCardsTotal = offsetBetweenCards + cardWidth;
    }

    public void AddOrder(Recipe recipe)
    {
        GameObject newCardObj = Instantiate(cardPrefab, this.transform);
        Vector2 position = newCardObj.GetComponent<RectTransform>().anchoredPosition;
        position.y = yOffset;
        position.x = offsetBetweenCards - offsetBetweenCardsTotal;
        newCardObj.GetComponent<RectTransform>().anchoredPosition = position;

        newCardObj.GetComponent<RecipeCard>().SetRecipe(recipe);

        orderUIList.Insert(0, newCardObj.GetComponent<RecipeCard>());
        StartCoroutine("Shift");
    }

    IEnumerator Shift()
    {
        for (int i = 0; i < moveTime; i++)
        {
            foreach (RecipeCard card in orderUIList)
            {
                Vector2 position = card.gameObject.GetComponent<RectTransform>().anchoredPosition;
                position.x += offsetBetweenCardsTotal / moveTime;
                card.gameObject.GetComponent<RectTransform>().anchoredPosition = position;
            }

            yield return null;
        }
    }
}

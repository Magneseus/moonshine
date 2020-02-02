using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Recipe vodka, wine, whiskey;
    private List<Recipe> availableRecipes;

    public OrderUI orderui;

    public int score = 0;
    private const int scorePerDrink = 100;
    private const int scoreBonus = 200;

    private List<Recipe> orderQueue;
    private float timeBetweenOrders = 5.0f;
    private float timeVarianceBetweenOrders = 2.5f;
    private float lastOrderTime = 0.0f;
    private float nextOrderTime = 0.0f;

    private void Awake()
    {
        score = 0;
        orderQueue = new List<Recipe>();
    }

    void Start()
    {
        vodka = new Recipe("vodka", "potato", "potato", "potato", 5.0f);
        wine = new Recipe("wine", "grapes", "grapes", "grapes", 5.0f);
        whiskey = new Recipe("whiskey", "wheat", "wheat", "wheat", 5.0f);
        availableRecipes = new List<Recipe>();
        availableRecipes.Add(vodka);
        availableRecipes.Add(wine);
        availableRecipes.Add(whiskey);

        lastOrderTime = Time.realtimeSinceStartup;
        CalcNextOrderTime();
    }

    void Update()
    {
        if (Time.realtimeSinceStartup - lastOrderTime > nextOrderTime)
        {
            CreateOrder();
            CalcNextOrderTime();
        }
    }

    public bool ValidateOrder(string orderName)
    {
        for (int i = orderQueue.Count-1; i >= 0;  i--)
        {
            if (orderQueue[i].VerifyRecipe(orderName))
            {
                // Good order, win points
                Recipe finishedRecipe = orderQueue[i];
                orderQueue.RemoveAt(i);

                score += scorePerDrink + (Mathf.CeilToInt(scoreBonus * finishedRecipe.GetPercentage()));

                return true;
            }
        }

        // Bad order, lose points
        score -= scorePerDrink / 2;
        return false;
    }

    private void CreateOrder()
    {
        Recipe newRecipe = availableRecipes[Random.Range(0, availableRecipes.Count)].DeepCopy();
        orderQueue.Insert(0, newRecipe);

        orderui.AddOrder(newRecipe);
    }

    private void CalcNextOrderTime()
    {
        lastOrderTime = Time.realtimeSinceStartup;
        nextOrderTime = timeBetweenOrders + Random.Range(-timeVarianceBetweenOrders, timeVarianceBetweenOrders);
    }
}

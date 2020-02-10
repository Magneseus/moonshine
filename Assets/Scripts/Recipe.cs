using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : Object
{
    public string recipeName;
    public string ing1, ing2, ing3;
    public Sprite ing1Img, ing2Img, ing3Img;
    public float recipeTime;
    public GameObject outputPrefab;

    private float startTime;

    private const string ing_suffix = "_for_recipe";

    public Recipe(string recipeName, string ing1, string ing2, string ing3, float recipeTime)
    {
        this.recipeName = recipeName;
        this.ing1 = ing1;
        this.ing2 = ing2;
        this.ing3 = ing3;
        this.recipeTime = recipeTime;

        this.ing1Img = Resources.Load("Ingredients/" + ing1 + ing_suffix, typeof(Sprite)) as Sprite;
        this.ing2Img = Resources.Load("Ingredients/" + ing2 + ing_suffix, typeof(Sprite)) as Sprite;
        this.ing3Img = Resources.Load("Ingredients/" + ing3 + ing_suffix, typeof(Sprite)) as Sprite;

        this.outputPrefab = Resources.Load("Ingredients/" + recipeName + ing_suffix) as GameObject;

        startTime = Time.realtimeSinceStartup;
    }

    public Recipe DeepCopy()
    {
        Recipe ret = new Recipe(recipeName, ing1, ing2, ing3, recipeTime);
        return ret;
    }

    public float GetTimeLeft()
    {
        return Time.realtimeSinceStartup - startTime;
    }

    public float GetPercentage()
    {
        return 1.0f - (GetTimeLeft() / recipeTime);
    }

    public bool VerifyRecipe(Recipe otherRecipe)
    {
        return otherRecipe.recipeName == this.recipeName;
    }

    public bool VerifyRecipe(string otherRecipe)
    {
        return otherRecipe == this.recipeName;
    }
}
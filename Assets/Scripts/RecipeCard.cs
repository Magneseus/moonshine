using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
    public Progressbar progressBar;
    public Image ingredient1, ingredient2, ingredient3;
    public Text recipeName;

    private Recipe recipeRef;

    public void SetRecipe(Recipe recipe)
    {
        recipeRef = recipe;

        ingredient1.sprite = recipe.ing1Img;
        ingredient2.sprite = recipe.ing2Img;
        ingredient3.sprite = recipe.ing3Img;

        recipeName.text = recipe.recipeName;
    }

    private void SetPercentage(float newPercentage)
    {
        progressBar.setPercentage(Mathf.Max(newPercentage, 0.0f));
    }

    private void Update()
    {
        float perc = recipeRef.GetPercentage();
        SetPercentage(perc);
        if (perc <= 0)
        {
            Destroy(this);
        }
    }
}

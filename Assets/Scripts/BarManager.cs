using System;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public float hunger, maxHunger = 400;
    public float hp, maxHP = 400;
    public float happy, maxHappy = 400;
   
    public Image[] hungerPoints;

    public Image[] hpPoints;

    public Image[] happyPoints;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hunger = maxHunger;
        hp = maxHP;
        happy = maxHappy;
    }

    // Update is called once per frame
    void Update()
    {
        if(hunger > maxHunger) hunger = maxHunger;
        if(hp > maxHP) hp = maxHP;
        if(happy > maxHappy) happy = maxHappy;
        
        BarFiller();
        ColorChanger();
        
    }

    void BarFiller()
    {
        for (int i = 0; i < hungerPoints.Length; i++)
        {
            hungerPoints[i].enabled = !DisplayHungerPoint(hunger, i);
        }
        for (int i = 0; i < hpPoints.Length; i++)
        {
            hpPoints[i].enabled = !DisplayHPPoint(hp, i);
        }
        for (int i = 0; i < happyPoints.Length; i++)
        {
            happyPoints[i].enabled = !DisplayHappyPoint(happy, i);
        }
        
    }

    void ColorChanger()
    {
        //hunger bar color
        Color hungerColor = Color.green;
        if (hunger <= maxHunger*.75)
        {
            hungerColor = Color.yellow;
        }
        if (hunger <= maxHunger*.50)
        {
            hungerColor = Color.orange;
        }
        if (hunger <= maxHunger*.25)
        {
            hungerColor = Color.red;
        }
        
        for (int i = 0; i < hungerPoints.Length; i++)
        {
            hungerPoints[i].color = hungerColor;
        }
            
        //hp colors
        Color healthColor = Color.green;
        if (hp <= maxHP*.75)
        {
            healthColor = Color.yellow;
        }
        if (hp <= maxHP*.50)
        {
            healthColor = Color.orange;
        }
        if (hp <= maxHP*.25)
        {
            healthColor = Color.red;
        }
        
        for (int i = 0; i < hpPoints.Length; i++)
        {
            hpPoints[i].color = healthColor;
        }
        
        //happy colors
        Color happyColor = Color.green;
        if (happy <= maxHappy*.75)
        {
            happyColor = Color.yellow;
        }
        if (happy <= maxHappy*.50)
        {
            happyColor = Color.orange;
        }
        if (happy <= maxHappy*.25)
        {
            happyColor = Color.red;
        }
        
        for (int i = 0; i < happyPoints.Length; i++)
        {
            happyPoints[i].color = happyColor;
        }
    }

    bool DisplayHungerPoint(float hungry, int pointNumber)
    {
        float pointsPerBar = maxHunger / hungerPoints.Length;
        return (pointNumber * pointsPerBar) >= hungry;
    }
    
    bool DisplayHPPoint(float health, int pointNumber)
    {
        return ((pointNumber * 20) >= health);
    }

    bool DisplayHappyPoint(float happy, int pointNumber)
    {
        return ((pointNumber * 20) >= happy);
    }

    public void Starve(float damagePoints)
    {
        if (hunger > 0)
        {
            hunger -= damagePoints;
        }
    }

    public void Feed(float healPoints)
    {
        if (hunger < maxHunger)
        {
            hunger += healPoints;
        }
    }

    public void Damage(float damagePoints)
    {
        if (hp > 0)
        {
            hp -= damagePoints;
        }
        
    }

    public void Heal(float healPoints)
    {
        if(hp < maxHP)
        {
            hp += healPoints;
        }
    }
    
    public void cheerUp(float cheerUpPoints)
    {
        if (happy < maxHappy)
        {
            happy += cheerUpPoints;
        }
    }

    public void cheerDown(float cheerDownPoints)
    {
        if (happy > 0)
        {
            happy -= cheerDownPoints;
        }
    }
    public void SetHunger(float value, float max)
    {
        hunger = (value / max) * maxHunger;
    }
    
    
}

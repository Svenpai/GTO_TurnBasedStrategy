﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public Structure prefab;
    public List<Cost> unitCost;

    public Structure Buy()
    {
        bool affordable = true;
        foreach (Cost c in unitCost)
        {
            if (c.resource.Amount - c.cost < 0)
            {
                affordable = false;
                print("You don't have enough " + c.resource.name);
            }           
        }

        if (affordable)
        {
            foreach (Cost c in unitCost)
            {
                c.resource.Change(-c.cost);
            }

            return Instantiate(prefab);
        }

        return null;
    }
}

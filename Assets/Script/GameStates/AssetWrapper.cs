using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AssetWrapper
{
    public const int INITSIZE = 5;

    public EnemyState[] enemies;
    private int enemyCount;
    private int curEnemySize = INITSIZE;
    
    public DoorState[] doors;
    private int doorCount;
    private int curDoorSize = INITSIZE;

    public ItemState[] items;
    private int itemCount;
    private int curItemSize = INITSIZE;

    public AssetWrapper()
    {
        enemies = new EnemyState[curEnemySize];
        enemyCount = 0;
        doors = new DoorState[curDoorSize];
        doorCount = 0;
        items = new ItemState[curItemSize];
        itemCount = 0;
    }

    public void Push(ItemState item)
    {
        if(itemCount < curItemSize)
        {
            items[itemCount] = item;
            itemCount++;
        }
        else
        {
            ItemState[] temp = items;
            curItemSize += INITSIZE;
            items = new ItemState[curItemSize];
            for(int i = 0;i<temp.Length; i++)
            {
                items[i] = temp[i];
            }
            items[itemCount] = item;
            itemCount++;

        }
    }

    public void Push(EnemyState item)
    {
        if (enemyCount < curEnemySize)
        {
            enemies[enemyCount] = item;
            enemyCount++;
        }
        else
        {
            EnemyState[] temp = enemies;
            curEnemySize += INITSIZE;
            enemies = new EnemyState[curEnemySize];
            for (int i = 0; i < temp.Length; i++)
            {
                enemies[i] = temp[i];
            }
            enemies[enemyCount] = item;
            enemyCount++;

        }
    }

    public void Push(DoorState item)
    {
        if (doorCount < curDoorSize)
        {
            doors[doorCount] = item;
            doorCount++;
        }
        else
        {
            DoorState[] temp = doors;
            curDoorSize += INITSIZE;
            doors = new DoorState[curDoorSize];
            for (int i = 0; i < temp.Length; i++)
            {
                doors[i] = temp[i];
            }
            doors[doorCount] = item;
            doorCount++;

        }
    }
}

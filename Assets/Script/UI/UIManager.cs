using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Player;
    public Sprite FullHP;
    public Sprite EmptyHP;
    public Image[] HealthBar;

    private PlayerMovement PlayerMov;
    private int previousHP;

    void Awake()
    {
        PlayerMov = (PlayerMovement)Player.gameObject.GetComponent<PlayerMovement>();
        for (int i = 0; i < PlayerMovement.MAXHP; i++)
        {
            if (i < PlayerMov.state.HP )
            {
                HealthBar[i].sprite = FullHP;
            }
            else if(i < PlayerMov.state.currentMaxHP)
            {
                HealthBar[i].sprite = EmptyHP;
            }
            else if (i >= PlayerMov.state.currentMaxHP)
            {
                HealthBar[i].enabled = false;
            }
        }
        previousHP = PlayerMov.state.HP;
    }

    // Update is called once per frame
    void Update()
    {   
        if (previousHP != PlayerMov.state.HP)
        {
            previousHP = PlayerMov.state.HP;
            Debug.Log(previousHP);
            for (int i = 0; i < PlayerMovement.MAXHP; i++)
            {
                if (i < PlayerMov.state.HP)
                {
                    HealthBar[i].sprite = FullHP;
                }
                else if (i < PlayerMov.state.currentMaxHP)
                {
                    HealthBar[i].sprite = EmptyHP;
                }
                else if (i >= PlayerMov.state.currentMaxHP)
                {
                    HealthBar[i].enabled = false;
                }
            }
        }
    }
}

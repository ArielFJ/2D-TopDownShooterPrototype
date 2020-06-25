using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.SocialPlatforms.Impl;

public class Store : MonoBehaviour
{

    public static Store instance;
    PlayerController player;
       
    [HideInInspector] public StoreItem equippedAmmo;
    public List<StoreItem> items;

    private void Awake()
    {
        instance = this;
        player = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    private void Start()
    {
        GameManager.instance.OnRestart += RestartItems;
        GameManager.instance.OnRestart += InitGame;


        InitGame();

    }

    void InitGame()
    {
        foreach (var item in items)
        {
            item.InitState();
        }

        if (player.projectilePrefab == null)
        {
            GameManager.instance.ChangeScore(items[0].projectilePrefab.GetComponent<Projectile>().price, false);
            BuyProjectile(0);
        }
    }


    public void UpgradeItem(int index)
    {
        StoreItem item = items[index];
        Projectile p = item.projectilePrefab.GetComponent<Projectile>();       
        
        float multiplier = ((int)item.weaponLevel + 1) * p.priceMultiplier;    
        int upgradePrice = (int)(p.price * multiplier);

        if(GameManager.instance.score >= upgradePrice && item.weaponLevel != Level.Mega)
        {
            GameManager.instance.ChangeScore(-upgradePrice, false);
            item.UpgradeItem();

            if (GameManager.instance.score < upgradePrice)
            {
                Button upgradeButton = item.whenOwnedUI.GetComponentsInChildren<Button>()[1];
                //Button buyButton = item.whenNotOwnedUI.GetComponentInChildren<Button>();
                upgradeButton.interactable = false;
            }
        }
    }

    public void BuyProjectile(int index)
    {
        StoreItem item = items[index];
        int ammoPrice = item.projectilePrefab.GetComponent<Projectile>().price;
        if (GameManager.instance.score >= ammoPrice)
        {
            item.BuyItem();
            if(player.projectilePrefab == null)
            {
                player.projectilePrefab = item.projectilePrefab;
                player.timeBetweenShots = item.projectilePrefab.GetComponent<Projectile>().timeBetweenShots;
                Text equipText = item.whenOwnedUI.GetComponentInChildren<Button>().GetComponentInChildren<Text>();                
                equipText.text = "equipped";                
            }
            GameManager.instance.ChangeScore(-ammoPrice, true);

            float multiplier = ((int)item.weaponLevel + 1) * item.projectilePrefab.GetComponent<Projectile>().priceMultiplier;

            int upgradePrice = (int)(item.projectilePrefab.GetComponent<Projectile>().price * multiplier);

            if (GameManager.instance.score < upgradePrice)
            {
                Button upgradeButton = item.whenOwnedUI.GetComponentsInChildren<Button>()[1];
                upgradeButton.interactable = false;
            }
        }
    }

    public void EquipProjectile(int index)
    {
        StoreItem item = items[index];
        player.projectilePrefab = item.projectilePrefab;
        player.timeBetweenShots = item.projectilePrefab.GetComponent<Projectile>().timeBetweenShots;
        foreach (var i in items)
        {
            Text equipText = i.whenOwnedUI.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
            if(item == i)
            {                
                equipText.text = "equipped";
            }
            else
            {
                equipText.text = "equip";
            }
        }
    }

    public void RestartItems()
    {
        foreach(var item in items)
        {
            item.weaponLevel = Level.Basic;
            item.hasOwned = false;
        }
        GameManager.instance.ChangeScore(items[0].projectilePrefab.GetComponent<Projectile>().price, false);
        BuyProjectile(0);
       
        foreach (var item in items)
        {
            item.InitState();
        }
    }

}

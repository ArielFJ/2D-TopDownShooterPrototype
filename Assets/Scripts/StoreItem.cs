using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StoreItem
{
    public GameObject projectilePrefab;
    public GameObject whenOwnedUI;
    public GameObject whenNotOwnedUI;
    public Level weaponLevel;
    public bool hasOwned = false;
    
    Text priceTextUI;
    Text levelTextUI;
    Text upgradePriceTextUI;

    public void InitState()
    {               
        levelTextUI = whenOwnedUI.GetComponentsInChildren<Text>()[0];
        upgradePriceTextUI = whenOwnedUI.GetComponentsInChildren<Text>()[1];
        priceTextUI = whenNotOwnedUI.GetComponentInChildren<Text>();
       
        weaponLevel = Level.Basic;
        Projectile p = projectilePrefab.GetComponent<Projectile>();
        p.level = this.weaponLevel;

        priceTextUI.text = p.price.ToString();

        if (hasOwned)
        {
            whenOwnedUI.SetActive(true);
            whenNotOwnedUI.SetActive(false);

            levelTextUI.text = ((int)p.level + 1).ToString();
            int upgradePrice = (int)(p.price * (((int)weaponLevel + 1) * p.priceMultiplier));
            upgradePriceTextUI.text = upgradePrice.ToString();
        }
        else
        {
            whenNotOwnedUI.SetActive(true);
            whenOwnedUI.SetActive(false);
        }
    }

    public void BuyItem()
    {
        hasOwned = true;
        whenOwnedUI.SetActive(true);
        whenNotOwnedUI.SetActive(false);
        Projectile p = projectilePrefab.GetComponent<Projectile>();
        int upgradePrice = (int)(p.price * (((int)weaponLevel + 1) * p.priceMultiplier));
        upgradePriceTextUI.text = upgradePrice.ToString();
    }

    public void UpgradeItem()
    {
        Projectile p = projectilePrefab.GetComponent<Projectile>();
        if (p != null)
        {
            if (hasOwned)
            {
                Level l = (Level)Mathf.Clamp((float)weaponLevel + 1, 0, 2);
                weaponLevel = l;
                levelTextUI.text = ((int)l + 1).ToString();
                p.level = weaponLevel;
                if(weaponLevel != Level.Mega)
                {
                    int upgradePrice = (int)(p.price * (((int)weaponLevel + 1) * p.priceMultiplier));
                    upgradePriceTextUI.text = upgradePrice.ToString();
                }
                else
                {
                    upgradePriceTextUI.text = "max";
                }
            }
        }
    }

}

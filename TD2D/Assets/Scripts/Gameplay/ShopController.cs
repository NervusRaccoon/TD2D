using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameManager gameManager;
    public Text moneyText;
    public Text hpText;
    public GameObject shopWindow;
    public GameObject towerInfo;
    public int money;
    public int hp;
    public GameObject board;
    private Vector2 pos = Vector2.zero;
    private GameObject selectedTower = null;
    private List<Tower> towerList;

    public void SetValue(List<Tower> tList)
    {
        towerList = tList;
        int count = 0; 
        foreach(Transform child in shopWindow.transform)
        {
            FillSlot(child, towerList[count]);
            count++;
        } 
    }

    private void Awake()
    {
        //mark = PracticeController.mark;
        shopWindow.SetActive(false);
        towerInfo.SetActive(false);
        moneyText.text = money.ToString();
        hpText.text = hp.ToString();
    }
    
    public void MoneyOperation(int partMoney)
    {
        money += partMoney;
        moneyText.text = money.ToString();
    }

    public void HealthOperation()
    {
        hp -= 1;
        hpText.text = hp.ToString();
        if (hp <= 0)
        {
            gameManager.Restart();
        }
    }

    public void OpenCloseShop(Vector2 towerPos)
    {
        if (shopWindow.activeSelf || towerInfo.activeSelf)
        {
            shopWindow.SetActive(false);
            towerInfo.SetActive(false);
            board.GetComponent<BoardController>().openShop = false;
            pos = Vector2.zero;
        }
        else
        {
            if (TowerExist(towerPos) == null)
            {
                shopWindow.SetActive(true);
                pos = towerPos;
            }
            else
            {
                towerInfo.SetActive(true);

                GameObject towerGO = TowerExist(towerPos);
                Tower tower = towerGO.GetComponent<TowerController>().towerSettings;
            
                if (selectedTower == null || !(selectedTower.GetComponent<TowerController>().towerSettings.towerType == tower.towerType))
                    FillSlot(towerInfo.transform, tower);
                selectedTower = towerGO;
            }
        }
    }

    public void DestroyTower()
	{
        MoneyOperation((int)(selectedTower.GetComponent<TowerController>().towerSettings.towerCost*0.8f));
		Destroy(selectedTower);
        towerInfo.SetActive(false);
        board.GetComponent<BoardController>().openShop = false;
	}

    public void SelectSlot(Text type)
    {
        int cost = 0;
        foreach (Tower tower in towerList)
            if (tower.towerType == type.text)
            {
                cost = tower.towerCost;
            }
        if (pos != Vector2.zero && money >= cost)
        {
            if (TowerExist(pos) == null)
            {
                gameManager.InstanceTower(type.text, pos);
                MoneyOperation(-cost);
            }
        }
        board.GetComponent<BoardController>().openShop = false;
        shopWindow.SetActive(false);
    }

    private GameObject TowerExist(Vector2 pos)
    {
        GameObject towerGO = null;
        if (gameManager.towerParent.childCount > 0)
            foreach (Transform tower in gameManager.towerParent)   
                if (Vector2.Distance(tower.position, pos) < .5f)
                    towerGO = tower.gameObject;
        return towerGO;
    }

    private void FillSlot(Transform parent, Tower tower)
    {
        foreach (Transform slotInfo in parent)
        {
            if (slotInfo.gameObject.name == "Image")
                slotInfo.gameObject.GetComponent<Image>().sprite = tower.towerPref.GetComponent<SpriteRenderer>().sprite;
            if (slotInfo.gameObject.name == "Name")
                slotInfo.gameObject.GetComponent<Text>().text = tower.towerType;
            if (slotInfo.gameObject.name == "Description")
                slotInfo.gameObject.GetComponent<Text>().text = "Урон: " + tower.towerDamage + "\nОписание: " + tower.towerType;
            if (slotInfo.gameObject.name == "Cost")
                if (parent == towerInfo.transform)
                    slotInfo.gameObject.GetComponent<Text>().text = "Стоимость продажи: " + (tower.towerCost*0.8f).ToString();
                else
                    slotInfo.gameObject.GetComponent<Text>().text = "Стоимость: " + tower.towerCost.ToString();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultController : MonoBehaviour
{
    private float mark = 0f;
    private ShopController shopController;
    public GameObject resultWindow;
    public Text markText;
    public Text markReview;
    public Text markInfo;
    private const string markA = "Тема усвоена в полной мере! Так держать!";
    private const string markB = "Есть недочеты, но тема усвоена хорошо!";
    private const string markC = "Есть ошибки, а значит есть куда стримиться";
    private const string markD = "Много ошибок, возможно, стоит попробовать снова?";
    private const string markE = "Слишком много ошибок, этап нужно перепройти";
    
    private void Start()
    {
        resultWindow.SetActive(false);
        shopController = gameObject.GetComponent<ShopController>();
    }

    public void Result()
    {
        resultWindow.SetActive(true);
		if (shopController.hp < 20f)
			mark += shopController.hp/20f;
        float genMark = 10f - mark - PracticeController.mark - GameManager.mark;
        if (genMark < 0)
            genMark = 0;
        markText.text = genMark.ToString();
        if (genMark >= 8f)
            markReview.text = markA;
        else if (genMark >= 6f)
            markReview.text = markB;
        else if (genMark >= 4f)
            markReview.text = markC;
        else if (genMark >= 2f)
            markReview.text = markD;
        else if (genMark < 2f)
            markReview.text = markE;
        markInfo.text = PracticeController.mark + "\n" + "0\n" + GameManager.mark + "\n" + mark;
        PlanController.SaveMark(genMark);
    }
    public void BackToPlan()
    {
        SceneManager.LoadSceneAsync("Plan");
    }
}
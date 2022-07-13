using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TheoryData
{
    public int id;
    public string theoryHeader;
    public List<string> theoryText;
}

[System.Serializable]
public class UserData
{
    public int id;
    public List<float> markList;
}

[System.Serializable]
public class SpriteList
{
    public int id;
    public List<Sprite> sprite;
}

public class PlanController : MonoBehaviour
{
    public static int id = 0;
    public static UserData userData;
    public static string pathRoot;
    private List<TheoryData> theoryList = new List<TheoryData>();
    public GameObject planPanel;
    public Transform planWindow;
    public GameObject lecturePanel;
    private const string lecturePath = "/TheoryData.json";
    private const string userPath = "/UserData.json";
    private Color blockedCol = new Color(0, 0, 0, 0.3f);
    private Color selectedCol = new Color(0.7f, 0.7f, 0.7f, 1f);
    private GameObject prevButton = null;
    public Image planImg;
    public Text lectureHeader;
    public Text markListText;

    private const string header = "Header";
    private const string window1 = "TheoryWindow1";
    private const string window2 = "TheoryWindow2";
    private const string window3 = "TheoryWindow3";
    private const string window4 = "TheoryWindow4";

    public List<SpriteList> spriteList;

    private void Start()
    {
        lecturePanel.SetActive(false);
        planPanel.SetActive(true);
        pathRoot = Application.streamingAssetsPath;
        LoadData();
        BlockPlan();
    }

    private void LoadData()
    {
        string path = pathRoot + userPath;
        var jsonString = File.ReadAllText(path);

        UserData[] data = JsonHelper.FromJson<UserData>(jsonString);
        id = data[0].id;
        userData = data[0];

        path = pathRoot + lecturePath;
        jsonString = File.ReadAllText(path);

        TheoryData[] theoryData = JsonHelper.FromJson<TheoryData>(jsonString);
        foreach(TheoryData lecture in theoryData)
            theoryList.Add(lecture);

    }

    private void BlockPlan()
    {
        foreach (Transform child in planWindow)
        {
            int name = Convert.ToInt32(child.gameObject.name);
            if (name > id)
            {
                child.GetChild(0).GetComponent<Text>().color = blockedCol;
                child.gameObject.GetComponent<Button>().interactable = false;
            }
        }

        foreach (float mark in userData.markList)
            markListText.text =  markListText.text + mark.ToString() + "\n";
    }

    public void StepClick(GameObject button)
    {
        int name = Convert.ToInt32(button.name);
        id = name;
        button.GetComponent<Image>().color = selectedCol;
        if (prevButton != null)
            prevButton.GetComponent<Image>().color = Color.white;
        prevButton = button;
        planImg.sprite = spriteList[0].sprite[0]; //planImg.sprite = spriteList[id].sprite[0];
    }

    public void WindowSwitch()
    {
        if (lecturePanel.activeSelf)
            {
                lecturePanel.SetActive(false);
                planPanel.SetActive(true);
            }
        else
            {
                lecturePanel.SetActive(true);
                FillLecture();
                planPanel.SetActive(false);                
            }
    }

    private void FillLecture()
    {
        foreach(Transform child in lecturePanel.transform)
        {
            if (child.gameObject.name == header)
                child.GetChild(0).GetComponent<Text>().text = theoryList[id].theoryHeader;
            if (child.gameObject.name == window1)
                child.GetChild(0).GetComponent<Image>().sprite = spriteList[0].sprite[1]; //child.GetChild(0).GetComponent<Image>().sprite = spriteList[id].sprite[0];
            if (child.gameObject.name == window2)
                child.GetChild(0).GetComponent<Text>().text = theoryList[id].theoryText[0];
            if (child.gameObject.name == window3)
                child.GetChild(0).GetComponent<Text>().text = theoryList[id].theoryText[1];
            if (child.gameObject.name == window4)
                child.GetChild(0).GetComponent<Text>().text = theoryList[id].theoryText[2];
        }
    }

    public void ToPractice()
    {
        SceneManager.LoadSceneAsync("Practice");
    }

    public static void SaveMark(float mark)
    {
        if (userData.markList.Count < id+1)
            userData.markList.Add(mark);
        else
            if (userData.markList[id] < mark)
                userData.markList[id] = mark;
        if (mark >= 2f)
            userData.id = id+1;

        UserData[] data = new UserData[1];
        data[0] = userData;

        string json = JsonHelper.ToJson(data, true);
        string path = pathRoot + userPath;
        File.WriteAllText(path, json);
    }

    public class JsonHelper
    {
        public static T[] FromJson<T>(string json) 
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T> 
        {
            public T[] Items;
        }
    }
}

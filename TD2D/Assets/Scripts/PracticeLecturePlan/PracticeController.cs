using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Practice
{
    public int id;
    public string questText;
    public string helpText;
    public string defaultCode;
    public string correctCode;
    public string practiceSprite;
}

public class PracticeController : MonoBehaviour
{
    public static float mark = 0f;
    private string path;
    private Practice currData;
    public int id = 0;
    public Text headerText;
    public Text codeText;
    public Dropdown helpDropdown;
    private Color redInputColor = new Color();
    private Color greenInputColor = new Color();
    private Color yellowInputColor = new Color();
    public Text inputFieldText;
    public Image codeWindowImg;
    private const string helpDefaultText = "Что именно нужно написать";
    private bool playMode = false;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("#F6847D", out redInputColor);
        ColorUtility.TryParseHtmlString("#98BA98", out greenInputColor);
        ColorUtility.TryParseHtmlString("#FECEA8", out yellowInputColor);
        codeWindowImg.color = yellowInputColor;
        
        id = PlanController.id;
        path = Application.streamingAssetsPath + "/PracticeData.json";
        LoadData();

    }

    private void LoadData()
    {
        var jsonString = File.ReadAllText(path);

        Practice[] data = JsonHelper.FromJson<Practice>(jsonString);
        currData = data[id];

        headerText.text = currData.questText;

        codeText.text = currData.defaultCode;

        helpDropdown.ClearOptions();
        Dropdown.OptionData option = new Dropdown.OptionData();
        option.text = currData.helpText;
        helpDropdown.options.Add(option);
    }

    public void CheckInput()
    {
        if (inputFieldText.text == currData.correctCode)
        {
            playMode = true;
            codeWindowImg.color = greenInputColor;
        }
        else
        {
            mark += 1f;
            codeWindowImg.color = redInputColor;
        }
    }

    public void PlayGame()
    {
        if (playMode)
        {
            //DontDestroyOnLoad(gameObject);
            //SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
            SceneManager.LoadSceneAsync("Gameplay");
        }
        else
            CheckInput();
    }

    public void ChangeHelpText()
    {
        GameObject nameButton = EventSystem.current.currentSelectedGameObject;
        Text helpCodeText = nameButton.transform.GetChild(0).GetComponent<Text>();
        if (helpCodeText.text == helpDefaultText)
            helpCodeText.text = currData.correctCode;
        else
            helpCodeText.text = helpDefaultText;
    }

    public void EditInputField()
    {
        codeWindowImg.color = yellowInputColor;
        playMode = false;
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

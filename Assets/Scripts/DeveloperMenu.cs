using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class DataBox
{
    TextMeshProUGUI tmValue;
    TextMeshProUGUI tmName;
    Slider slider;
    
    private string name;
   
    private float valuef=-1;

    private int valuei = -1;



    // Property for 'name'
    public Slider Slider
    {
        get { return slider; }
        set { slider = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }


    // Property for 'valuef'
    public float ValueF
    {
        get { return valuef; }
        set { valuef = value; }
    }

   

    // Property for 'valuei'
    public int ValueI
    {
        get { return valuei; }
        set { valuei = value; }
    }
    public TextMeshProUGUI TmValue
    {
        get { return  tmValue; }
        set {  tmValue = value; }
    }
    public TextMeshProUGUI TmName
    {
        get { return tmName; }
        set { tmName = value; }
    }
   
 
  
 

   
}



public class DeveloperMenu : MonoBehaviour
{
    public static Action OnApplyChanges;
    public List<GameObject> DataboxPages;
    public List<GameObject> dataBoxGameObjects;
    List<DataBox> dataBoxes = new List<DataBox>();
    Dictionary<string, object> gameVariables;
    List<(string, float)> _gameVariables =new List<(string, float)>();
    public static DeveloperMenu instance;

    int pageIndex = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    
      List<object> LgameVariables = new List<object>();
      gameVariables = new Dictionary<string, object>
      {
          { "Frogspeed", GameManager.instance._frogspeed },         
          { "SpeedBoost", BuffManager.instance.SpeedBoostValue },
          { "SpeedDecrease", BuffManager.instance.SpeedDecreaseValue },
          { "NightMode", BuffManager.instance.NightModeLength },

      };
    
    
        var gameVariable1 = ("Frogspeed", GameManager.instance._frogspeed);
        var gameVariable2 = ("SpeedBoost", BuffManager.instance.SpeedBoostValue);
        var gameVariable3 = ("SpeedDecrease", BuffManager.instance.SpeedDecreaseValue);
        var gameVariable4 = ("NightMode", BuffManager.instance.NightModeLength);

        _gameVariables.Add(gameVariable1);
        _gameVariables.Add(gameVariable2);
        _gameVariables.Add(gameVariable3);
        _gameVariables.Add(gameVariable4);

        addDataBox();


        for (int i = 0; i < dataBoxes.Count; i++)
        {
            dataBoxes[i].TmName.text = dataBoxes[i].Name;
            if (dataBoxes[i].ValueF != -1)
            {
                dataBoxes[i].TmValue.text = dataBoxes[i].ValueF.ToString();
            }
            else if (dataBoxes[i].ValueI != -1)
            {
           
                dataBoxes[i].TmValue.text = dataBoxes[i].ValueI.ToString();
            }

        }

        foreach (var dataBox in dataBoxes)
        {
            if (dataBox.Name == "Frogspeed")
            {
                dataBox.Slider.minValue = 300;
                dataBox.Slider.maxValue = 640;
                dataBox.Slider.value = GameLoopManager.progress.speed;

            }      
            if (dataBox.Name == "SpeedBoost")
            {
                dataBox.Slider.minValue = 80f;
                dataBox.Slider.maxValue = 320;
                dataBox.Slider.value = GameLoopManager.progress.SpeedBoostValue;
            }
            if (dataBox.Name == "SpeedDecrease")
            {
                dataBox.Slider.minValue = 80f;
                dataBox.Slider.maxValue = 320;
                dataBox.Slider.value = GameLoopManager.progress.SpeedDecreaseValue;
            }
            if (dataBox.Name == "NightMode")
            {
                dataBox.Slider.minValue = 1f;
                dataBox.Slider.maxValue = 20;
                dataBox.Slider.value = GameLoopManager.progress.NightModeLength;
            }

        }

    

      
        
      



        ApplyChanges();




    }
    public void NextPage()
    {
        if (pageIndex < DataboxPages.Count - 1)
        {
            DataboxPages[pageIndex].SetActive(false);
            pageIndex++;
            DataboxPages[pageIndex].SetActive(true);
        }
    }
    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            DataboxPages[pageIndex].SetActive(false);
            pageIndex--;
            DataboxPages[pageIndex].SetActive(true);
        }
    }

    public void initData()
    {

        foreach (var dataBox in dataBoxes)
        {
            if (dataBox.Name == "Frogspeed")
            {
                GameManager.instance._frogspeed = dataBox.ValueF;
            }
            if (dataBox.Name == "SpeedBoost")
            {
                GameLoopManager.progress.SpeedBoostValue = dataBox.ValueF;
            }
            if (dataBox.Name == "SpeedDecrease")
            {
                GameLoopManager.progress.SpeedDecreaseValue = dataBox.ValueF;
            }
            if (dataBox.Name == "NightMode")
            {
                GameLoopManager.progress.NightModeLength = dataBox.ValueF;
            }
        }


    }
    public void ApplyChanges()
    {

        foreach (var dataBox in dataBoxes)
        {
            if (dataBox.Name == "Frogspeed")
            {
                GameManager.instance._frogspeed = dataBox.ValueF;
                GameLoopManager.progress.speed = dataBox.ValueF;
            }
            if (dataBox.Name == "SpeedBoost")
            {
                GameLoopManager.progress.SpeedBoostValue = dataBox.ValueF;
            }
            if (dataBox.Name == "SpeedDecrease")
            {
                GameLoopManager.progress.SpeedDecreaseValue = dataBox.ValueF;
            }
            if (dataBox.Name == "NightMode")
            {
                GameLoopManager.progress.NightModeLength = dataBox.ValueF;
            }
        }
        ProgressManager.SaveGameProgress("savedProgress", GameLoopManager.progress); 

        OnApplyChanges?.Invoke();
    }
    public void OnSliderChanged(string str)
    {

        
        foreach(var gamevar in _gameVariables)
        {
            

            if (gamevar.Item1 == str)
            {
                for(int i=0; i<dataBoxes.Count;i++)
                {
                    if (str == dataBoxes[i].Name)
                    {
                       
                        object value = dataBoxes[i].Slider.value;
                        if (value is int intval)
                        {
                            dataBoxes[i].ValueF = intval;
                            
                        }
                        else if (value is float floatval)
                        {
                            dataBoxes[i].ValueF = floatval;
                        }

                    }
                }

            }
          
        }
      //  if (i == 1)
      //  {
      //
      //  }
      //  else if (i == 2)
      //  {
      //
      //  }
      //  else if (i == 3)
      //  {
      //
      //  }
      //  else if (i == 4)
      //  {
      //
      //  }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < dataBoxes.Count; i++)
        {
            dataBoxes[i].TmName.text = dataBoxes[i].Name;
            if (dataBoxes[i].ValueF != -1)
            {
                dataBoxes[i].TmValue.text = dataBoxes[i].ValueF.ToString();
   
            }
            else if (dataBoxes[i].ValueI != -1)
            {
               
                dataBoxes[i].TmValue.text = dataBoxes[i].ValueI.ToString();
            }

        }


    }


    void addDataBox()
    {
        int iteratorIndex = 0;

        // db.addDataBox("minFrogSpeed", "maxFrogSpeed", frogfactoryscript.minFrogSpeed, frogfactoryscript.maxFrogSpeed);

        //All the databoxes
        for (int n =0; n<dataBoxGameObjects.Count; n++)
        {

            
           




            Slider[] sliders = dataBoxGameObjects[n].GetComponentsInChildren<Slider>();

            GameObject[] subDataBoxes=new GameObject[dataBoxGameObjects[n].transform.childCount];
  
            for (int y=0; y< dataBoxGameObjects[n].transform.childCount;y++)
            {
                subDataBoxes[y]=dataBoxGameObjects[n].transform.GetChild(y).gameObject;
            }
            if (subDataBoxes.Length == 1)
            {
                DataBox db=new DataBox();
                GameObject[] goChildren = new GameObject[subDataBoxes[0].transform.childCount];
               
                for (int y = 0; y < subDataBoxes[0].transform.childCount; y++)
                {
                    goChildren[y] = subDataBoxes[0].transform.GetChild(y).gameObject;
                }

                //Inside the databox
                for (int i=0; i<goChildren.Length; i++)
                {
                    iteratorIndex++;
                    if (goChildren[i].name=="Value")
                    {
                        foreach(var kvp in gameVariables)
                        {
                            string key = kvp.Key;
                            object value = kvp.Value;
                            if (dataBoxGameObjects[n].name ==key)
                            {
                         
                                db.Name = key;
                                if(value is int intval)
                                {
                                    db.ValueI= intval;
                                }
                                else if(value is float floatval)
                                {
                                    db.ValueF= floatval;
                                }
                               
                            }
                        }
                        db.TmValue = goChildren[i].GetComponent<TextMeshProUGUI>();

                    }
                    else if (goChildren[i].name=="Name")
                    {
                        db.TmName = goChildren[i].GetComponent<TextMeshProUGUI>();

                    }
                    else if (goChildren[i].name=="Slider")
                    {
                        db.Slider = goChildren[i].GetComponent<Slider>();
                        
                    }
                 
                }
               
                dataBoxes.Add(db);



            }
           else if (subDataBoxes.Length == 2)
           {
                for(int i=0;i<2;i++)
                {
                    iteratorIndex++;
                    GameObject[] goChildren = new GameObject[subDataBoxes[i].transform.childCount];

                    for (int y = 0; y < subDataBoxes[i].transform.childCount; y++)
                    {
                        goChildren[y] = subDataBoxes[i].transform.GetChild(y).gameObject;
                    }

         
                    DataBox dataBox = new DataBox();

                    for (int j=0; j<goChildren.Length;j++)
                    {
                    
                        if (goChildren[j].name == "Value")
                        {
                            foreach(var kvp in gameVariables)
                            {
                                string key= kvp.Key;
                                object val = kvp.Value;
                                if (subDataBoxes[i].name==key)
                                {
                                    dataBox.Name = key;
                                    if(val is int intval)
                                    {
                                        dataBox.ValueI= intval;
                                    }
                                    else if(val is float floatval)
                                    {
                                        dataBox.ValueF= floatval;
                                    }
                                }
                            }
                            dataBox.TmValue = goChildren[j].GetComponent<TextMeshProUGUI>();

                        }
                        else if (goChildren[j].name == "Name")
                        {
                            dataBox.TmName = goChildren[j].GetComponent<TextMeshProUGUI>();
                        }
                        else if (goChildren[j].name == "Slider")
                        {
                            dataBox.Slider = goChildren[j].GetComponent<Slider>();

                        }
                       
                    }
                    dataBoxes.Add(dataBox);

                }
         
           }
               
           }



       


    }




 

}

    

 
    


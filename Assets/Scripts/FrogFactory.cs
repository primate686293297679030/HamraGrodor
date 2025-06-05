using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class frogObject : MonoBehaviour
{
    public bool isActive;
    public RectTransform rectTransform;
    public float speed;
    public FrogScript frogScript;

    public RectTransform frogRectTransform;
    public bool isAnimating;
    public Tween tweenerInDown;
    public Tween tweenerInUp;
    public Tween tweenerUp;
    public Tween tweenerDown;
    public Tween tweenerSize;
    public Sequence sequence;

    public Sequence LillypadSequence;
    public Tween LillypadDown;
    public Tween LillypadUp;

    public bool endOfLoop;
}
public class FrogFactory : MonoBehaviour
{
    [SerializeField] private GameObject _frogPrefab; // Reference to the prefab you want to spawn
    [SerializeField] private RectTransform _parentTransform; // The RectTransform of borderCollider.gameObject is used as parent for the _frogPrefab

    public float minFrogSpeed =0f;
        public float maxFrogSpeed=0f;
    public static FrogFactory instance;

    List<frogObject> frogObjects= new List<frogObject>();
    public float[] yPositions = new float[4] { -155.7142857142857f, -311.4285714285714f, -467.1428571428571f,-622.8571428571428f };

    int yIndex;

    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

    // Method to create new instances of the prefab
    private GameObject CreateGameObject(Vector3 position, Quaternion rotation)
    {
        _frogPrefab.transform.parent= _parentTransform;
        _frogPrefab = Instantiate(_frogPrefab, position, rotation);
        return _frogPrefab;
    }

    // Method to destroy the game object
    private void DestroyGameObject(GameObject objectToDestroy)
    {
        Destroy(objectToDestroy);
    }
    Vector3 GetRandomPosition()
    {


         yIndex++;
        if(yIndex>3)
        {
            yIndex=0;
        }
     
        Vector3 randomPosition = new Vector3(1, yPositions[yIndex],1f);
     
        return randomPosition;
  

    }
    public List<frogObject> InstansiateFrogs(int amount)
    { 
        for(int i=1;i<=amount;i++)
        {           
            if (_parentTransform.gameObject != null)
            {
                if (_frogPrefab != null)
                {
       
                    
                       
              
                        Vector3 randomPosition = GetRandomPosition();
                     
                        GameObject frog = Instantiate(_frogPrefab);
                    
                        frog.transform.parent = _parentTransform;

                      

                       

                        if (frog.TryGetComponent(out RectTransform rectTransform))
                        {
                            rectTransform.anchoredPosition = randomPosition;
                           rectTransform.localScale=new Vector3(2f,2f,1);
                        frogObject frogObject = new frogObject();
                        
                        frogObject.rectTransform = rectTransform;
                        frogObject.isActive = false;
                        frogObject.speed = 47500;
                        frogObject.frogRectTransform= frog.transform.GetChild(0).GetComponent<RectTransform>();
                        frogObject.frogScript = frog.GetComponentInChildren<FrogScript>();
                            frog.name = "frogPrefabInstance";
                            frogObjects.Add(frogObject);
                        
                        }



                    
                }
            }            
        }
        return frogObjects;
    }
    
}


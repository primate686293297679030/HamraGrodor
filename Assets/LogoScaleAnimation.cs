using UnityEngine;
using DG.Tweening;

public class LogoScaleAnimation : MonoBehaviour
{
  
    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }
    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0.5f, 2f).SetEase(Ease.OutBack)); 


    }
}


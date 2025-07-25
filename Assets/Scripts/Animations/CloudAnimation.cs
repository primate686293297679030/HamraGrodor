using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Scripting.APIUpdating;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using System;

public class CloudAnimation : MonoBehaviour
{

    // Async operation completed, now perform DoTween animation
    [SerializeField] Vector3 targetPosition = new Vector3 (0,0,0);
    [SerializeField] float duration = 6f;
    [SerializeField] float delay = 0f;
    [SerializeField] private RectTransform rectTransform;

    // Start is called before the first frame update
    async void Start()
    {
        duration=UnityEngine.Random.Range(100, 250);
     //   RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), transform.position, null, out targetPosition);
        try
        {
            await taskLoop();
        }
        catch (OperationCanceledException)
        {
           
        }
    }

    private async Task taskLoop()
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {

            // transform.DOLocalMove(targetPosition, duration);
            
                 await Awaitable.WaitForSecondsAsync(delay,destroyCancellationToken);
            await rectTransform.DOAnchorPos(targetPosition, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).AsyncWaitForCompletion();
            duration= UnityEngine.Random.Range(100, 250);
            
        }
    }
}

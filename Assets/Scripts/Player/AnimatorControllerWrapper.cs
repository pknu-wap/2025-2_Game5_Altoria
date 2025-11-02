using System.Collections;
using UnityEngine;

public class AnimatorControllerWrapper
{
     readonly Animator animator;
     readonly MonoBehaviour coroutineHost;

    Coroutine currentBlendRoutine;

    public AnimatorControllerWrapper(Animator animator, MonoBehaviour coroutineHost)
    {
        this.animator = animator;
        this.coroutineHost = coroutineHost;
    }

    
    public void SetBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void SetInt(string name, int value)
    {
        animator.SetInteger(name, value);
    }

    public float GetLayerWeight(int index) => animator.GetLayerWeight(index);
    public void SetLayerWeight(int index, float weight) => animator.SetLayerWeight(index, weight);
    

   
    public void BlendLayerWeight(int layerIndex, float from, float to, float duration)
    {
        
        if (currentBlendRoutine != null)
            coroutineHost.StopCoroutine(currentBlendRoutine);

        currentBlendRoutine = coroutineHost.StartCoroutine(BlendRoutine(layerIndex, from, to, duration));
    }

    IEnumerator BlendRoutine(int layerIndex, float from, float to, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float weight = Mathf.Lerp(from, to, time / duration);
            animator.SetLayerWeight(layerIndex, weight);
            yield return null;
        }

        animator.SetLayerWeight(layerIndex, to);
        currentBlendRoutine = null;
    }
 
}

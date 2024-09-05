using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;  // Animator ������Ʈ ����
    [SerializeField] private AnimationClip[] animationClips;  // �ִϸ��̼� Ŭ���� �̸� �迭

    public void PlayRandomAnimation()
    {
        Debug.Log("playRandomAni");
        if (animationClips.Length == 0)
        {
            Debug.LogWarning("No animation clip names assigned.");
            return;
        }

        // �ִϸ��̼� Ŭ�� �̸� �� �ϳ��� �������� ����
        int randomIndex = Random.Range(0, animationClips.Length);
        AnimationClip clipName = animationClips[randomIndex];

        // ���õ� �ִϸ��̼� Ŭ���� Animator�� ����Ͽ� ���
        animator.Play(clipName.name);
    }

}

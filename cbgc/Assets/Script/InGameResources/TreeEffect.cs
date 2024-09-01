using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;  // Animator 컴포넌트 참조
    [SerializeField] private AnimationClip[] animationClips;  // 애니메이션 클립의 이름 배열

    public void PlayRandomAnimation()
    {
        Debug.Log("playRandomAni");
        if (animationClips.Length == 0)
        {
            Debug.LogWarning("No animation clip names assigned.");
            return;
        }

        // 애니메이션 클립 이름 중 하나를 랜덤으로 선택
        int randomIndex = Random.Range(0, animationClips.Length);
        AnimationClip clipName = animationClips[randomIndex];

        // 선택된 애니메이션 클립을 Animator에 명령하여 재생
        animator.Play(clipName.name);
    }

}

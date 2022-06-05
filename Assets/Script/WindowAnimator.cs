using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class WindowAnimator : EditorWindow
{

    private float time;
    private static Animator[] AllAnimators;
    private GameObject AnimatorFocus;
    private Animator animator;
    private bool PlayAnim;
    private bool Loop;
    private float SpeedAnim =1;
    private AnimationClip CurrentClip;
    [MenuItem("Toolbox/WindowAnimator")]
    [MenuItem("Window/Toolbox/WindowAnimator")]
  

    static void Init()
    {
        AllAnimators = (Animator[])FindObjectsOfType(typeof(Animator));
        Debug.Log("Init WindowAnimator");
        WindowAnimator window = GetWindow<WindowAnimator>();
        window.Show();
        window.titleContent = new GUIContent("WindowAnimator");
    }

    void Update()
    {
        if (PlayAnim)
        {
            time += Time.deltaTime*SpeedAnim;
            if (time >= CurrentClip.length && Loop)
            {
                time = 0;
                
            }
        }

        if (!EditorApplication.isPlaying && (AnimationMode.InAnimationMode() || time > 0) && CurrentClip != null)
        {
            AnimationMode.StartAnimationMode();
            AnimationMode.BeginSampling();
            AnimationMode.SampleAnimationClip(AnimatorFocus, CurrentClip, time);
            AnimationMode.EndSampling();

            SceneView.RepaintAll();
        }
    }

    private void OnGUI()
    {

        foreach (var Animators in AllAnimators)
        {
            if (GUILayout.Button(Animators.name))
            {
                AnimatorFocus = Animators.gameObject;
                animator = Animators;
                time = 0;
                PlayAnim = false;
                AnimationMode.StopAnimationMode();
            }
        }

        GUILayout.Space(50);

        if(animator != null)
        {
            foreach (var AllAnim in animator.runtimeAnimatorController.animationClips)
            {
                if (GUILayout.Button(AllAnim.name))
                {
                    AnimationMode.StartAnimationMode();
                    if(CurrentClip == AllAnim)
                    {
                        PlayAnim = false;
                    }
                    else
                    {
                        PlayAnim = true;
                    }
                    CurrentClip = AllAnim;
                    if (!PlayAnim)
                    {
                        AnimationMode.StopAnimationMode();
                    }
                }
            }
            GUILayout.Space(25);
            GUILayout.Label("Accelerate");
            SpeedAnim = EditorGUILayout.Slider(SpeedAnim, 0, 2f);
            GUILayout.Label("Time Anim : " + CurrentClip.length + " sec");
            time = EditorGUILayout.Slider(time, 0f, CurrentClip.length);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Loop ");
            GUILayout.Space(-100);
            Loop = EditorGUILayout.Toggle(Loop);
            GUILayout.EndHorizontal();
        }
        
    }
}





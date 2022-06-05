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
                if (Selection.activeGameObject != Animators.gameObject)
                {
                    Highlighter.Highlight("Hierarchy", Animators.name);
                    Selection.activeGameObject = Animators.gameObject;
                    SceneView.FrameLastActiveSceneView();
                }
                CurrentClip = null;
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

            GUILayout.Label("Click On Animation and click Play bouton to Start Animation");
            GUILayout.Space(10
                );
            foreach (var AllAnim in animator.runtimeAnimatorController.animationClips)
            {
                if (GUILayout.Button(AllAnim.name))
                {
                    CurrentClip = AllAnim;
                }
            }

            
            

            if(CurrentClip != null){
                GUILayout.Space(25);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("PlayButton"))
                {
                    if (!Loop)
                    {
                        time = 0;
                    }
                    PlayAnim = true;
                }
                if (GUILayout.Button("StopButton"))
                {
                    PlayAnim = false;
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(25);
                GUILayout.Label("Accelerate");
                SpeedAnim = EditorGUILayout.Slider(SpeedAnim, 0, 2f);
                GUILayout.Label("Time Anim : " + CurrentClip.length + " sec");
                time = EditorGUILayout.Slider(time, 0f, CurrentClip.length);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Loop ");
                Loop = EditorGUILayout.Toggle(Loop);
                GUILayout.EndHorizontal();
            }
            
        }
        
    }
}





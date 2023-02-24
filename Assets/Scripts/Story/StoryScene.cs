using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : ScriptableObject {
    public List<Sentence> sentences;
    public Sprite background;
    public StoryScene nextScene;
    public bool isLastScene;

    [System.Serializable]
    public struct Sentence {
        public string text;
    }
}

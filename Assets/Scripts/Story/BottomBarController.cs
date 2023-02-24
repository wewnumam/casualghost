using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BottomBarController : MonoBehaviour {
    public TextMeshProUGUI barText;
    private int sentenceIndex = -1;
    private StoryScene currentScene;
    private State state = State.COMPLETED;

    private enum State {
        PLAYING,
        COMPLETED
    }

    public void PlayScene(StoryScene storyScene) {
        currentScene = storyScene;
        sentenceIndex = -1;
        PlayNextSentence();
    }

    public void PlayNextSentence() {
        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
    }

    public bool IsCompleted() => state == State.COMPLETED;

    public bool IsLastSentence() => sentenceIndex + 1 == currentScene.sentences.Count;

    IEnumerator TypeText(string text) {
        barText.text = "";
        state = State.PLAYING;
        int wordIndex = 0;

        while (state != State.COMPLETED) {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(0.01f);

            if (++wordIndex == text.Length) {
                state = State.COMPLETED;
                break;   
            }
        }
    }
}

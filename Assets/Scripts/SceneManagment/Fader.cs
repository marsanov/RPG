using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            if(canvasGroup == null) return;
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1) //alpha is not 1
            {
                //moving alpha toward 1
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) //alpha is not 1
            {
                //moving alpha toward 1
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}

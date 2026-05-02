using UnityEngine;
using TMPro;
using System.Collections;

public class LevelTitleUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public float fadeDuration = 1f;
    public float showDuration = 2f;

    void Start()
    {
        titleText.gameObject.SetActive(true);
        StartCoroutine(ShowTitle());
    }

    IEnumerator ShowTitle()
    {
        // fade in
        yield return StartCoroutine(Fade(0f, 1f));

        // pidä näkyvissä
        yield return new WaitForSeconds(showDuration);

        // fade out
        yield return StartCoroutine(Fade(1f, 0f));
        titleText.gameObject.SetActive(false);
    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0f;

        Color color = titleText.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(start, end, time / fadeDuration);
            titleText.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        titleText.color = new Color(color.r, color.g, color.b, end);
    }
}
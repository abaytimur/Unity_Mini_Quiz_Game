using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private Text correctAnswersNumberText, wrongAnswersNumberText, resultPointsText;
    [SerializeField] private GameObject leftStar, middleStar, rightStar;
    [SerializeField] private GameObject resultPanel;
    private string url = "https://forms.gle/hMBkUYED6bmxkmPu6";

    public void ShowResults(int correctNumber, int wrongNumber, int points)
    {
        correctAnswersNumberText.text = correctNumber.ToString();
        wrongAnswersNumberText.text = wrongNumber.ToString();
        resultPointsText.text = points.ToString();

        leftStar.SetActive(false);
        middleStar.SetActive(false);
        rightStar.SetActive(false);

        if (correctNumber > 6 && correctNumber <= 13) 
        {
            middleStar.SetActive(true);
        }
        else if (correctNumber <= 19)
        {
            leftStar.SetActive(true);
            middleStar.SetActive(true);
        }
        else
        {
            leftStar.SetActive(true);
            middleStar.SetActive(true);
            rightStar.SetActive(true);
        }
    }

    public void RestartGame()
    {
        // SceneManager.LoadScene(sceneBuildIndex: 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        resultPanel.SetActive(false);
    }

    public void FeedbackButton()
    {
        Application.OpenURL(url);
    }
}
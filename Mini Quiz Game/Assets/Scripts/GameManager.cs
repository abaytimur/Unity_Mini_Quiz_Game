using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Question[] questions;

    private static List<Question> _unAnsweredQuestions;

    private Question activeQuestion;

    [SerializeField] private Text questionNumberText;
    [SerializeField] private GameObject buttonDisablePanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text questionText;
    [SerializeField] private Text correctAnswerText, wrongAnswerText;
    [SerializeField] private GameObject correctButton, wrongButton;
    private RectTransform rectTransform;
    private RectTransform rectTransform1;

    private int correctAnswersNumber, wrongAnswersNumber;
    private int totalPoints;
    private int totalQuestionNumber;
    
    private ResultManager resultManager;

    // Start is called before the first frame update
    private void Awake()
    {
        rectTransform1 = correctButton.GetComponent<RectTransform>();
        rectTransform = wrongButton.GetComponent<RectTransform>();
    }

    void Start()
    {
        if (_unAnsweredQuestions == null || _unAnsweredQuestions.Count == 0)
        {
            _unAnsweredQuestions = questions.ToList<Question>();
        }

        totalQuestionNumber = _unAnsweredQuestions.Count;
        questionNumberText.text = totalQuestionNumber - _unAnsweredQuestions.Count + " / " + totalQuestionNumber;
        correctAnswersNumber = 0;
        wrongAnswersNumber = 0;
        totalPoints = 0;

        PickRandomQuestion();
    }

    private void PickRandomQuestion()
    {
        rectTransform.DOLocalMoveX(320f, .2f);
        rectTransform1.DOLocalMoveX(-320f, .2f);

        int randomQuestionIndex = Random.Range(0, _unAnsweredQuestions.Count);
        activeQuestion = _unAnsweredQuestions[randomQuestionIndex];

        questionText.text = activeQuestion.question;

        if (activeQuestion.isItCorrect)
        {
            correctAnswerText.text = "Correct Answer!";
            wrongAnswerText.text = "Wrong Answer!";
        }
        else
        {
            correctAnswerText.text = "Wrong Answer!";
            wrongAnswerText.text = "Correct Answer!";
        }
    }

    IEnumerator WaitBetweenQuestions()
    {
        _unAnsweredQuestions.Remove(activeQuestion);
        yield return new WaitForSeconds(1f);

        if (_unAnsweredQuestions.Count <= 0)
        {
            resultPanel.SetActive(true);
            resultManager = Object.FindObjectOfType<ResultManager>();
            resultManager.ShowResults(correctAnswersNumber, wrongAnswersNumber, totalPoints);
        }
        else
        {
            PickRandomQuestion();
        }
    }

    public void CorrectButtonIsClicked()
    {
        if (activeQuestion.isItCorrect)
        {
            correctAnswersNumber++;
            totalPoints += 100;
        }
        else
        {
            wrongAnswersNumber++;
            totalPoints -= 25;
            if (totalPoints <= 0)
            {
                totalPoints = 0;
            }
        }
        StartCoroutine(ButtonDisablePanel());
        wrongButton.GetComponent<RectTransform>().DOLocalMoveX(1000f, .2f);
        StartCoroutine(WaitBetweenQuestions());
        questionNumberText.text = totalQuestionNumber - _unAnsweredQuestions.Count + " / " + totalQuestionNumber;
       
    }

    public void WrongButtonIsClicked()
    {
        if (!activeQuestion.isItCorrect)
        {
            correctAnswersNumber++;
            totalPoints += 100;
        }
        else
        {
            wrongAnswersNumber++;
            totalPoints -= 25;
        }
        StartCoroutine(ButtonDisablePanel());
        StartCoroutine(WaitBetweenQuestions());
        correctButton.GetComponent<RectTransform>().DOLocalMoveX(-1000f, .2f);
        questionNumberText.text = totalQuestionNumber - _unAnsweredQuestions.Count + " / " + totalQuestionNumber;
        
    }

    IEnumerator ButtonDisablePanel()
    {
        buttonDisablePanel.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        buttonDisablePanel.SetActive(false);
    }
}
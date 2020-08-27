using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class AnswerInput
{
    // Class constructors for all student answers

    public int qNumber;
    public string question;
    public GameObject inputObject;
    
    public AnswerInput(int qNumber, string question, GameObject inputObject)
    {
        this.qNumber = qNumber;
        this.question = question;
        this.inputObject = inputObject;
    }
}




public class QandAFunctionality : MonoBehaviour
{

    // Create set of input Q&A from inspector to add/change easily
    [SerializeField]
    private List<AnswerInput> _answers;

    // Dictionaries for Q&A to same number
    private Dictionary<int, string> _allQuestions = new Dictionary<int, string>();

    private Dictionary<int, List<string>> _allSavedAnswers = new Dictionary<int, List<string>>();

    private Dictionary<string, string> _lastQuestionsAndAnswers = new Dictionary<string, string>();

    public static string messageToSend = "";





    private void Start()
    {
        // Populate question and answer dictionaries
        QuestionsInDictionaryStart();
        AnswersInDictionaryStart();
    }

    private void OnDisable()
    {
        // For each, store new answer in new list element when lab script is closed
        SaveAnswers();

        // Set up the string to send as the email message with the questions and answers
        SetUpAnswersTextForEmail();
        Debug.Log(messageToSend);
    }




    // Get input text from the text object
    private string GetTextFromObject(GameObject _input)
    {
        TMP_InputField inputTMP = _input.GetComponent<TMP_InputField>();
        return inputTMP.text;
    }

    // To be called at the start to populate Q&A in dictionaries

    private void QuestionsInDictionaryStart()
    {
        foreach (AnswerInput a in _answers)
        {

            _allQuestions.Add(a.qNumber, a.question);
        }
    }

    private void AnswersInDictionaryStart()
    {
        foreach (AnswerInput a in _answers)
        {
            List<string> startingInput = new List<string>();

            string answerText = GetTextFromObject(a.inputObject);
            startingInput.Add(answerText);

            _allSavedAnswers.Add(a.qNumber, startingInput);
        }
    }




    // Save any new changes in a new element in the list so we can see
    // iterations of answers throughout ----> next step: implement code to work
    // out where in time line this is happening (with redo and undo!)

    private void SaveAnswers()
    {
        foreach (AnswerInput changedAnswer in _answers)
        {
            List<string> answerList = _allSavedAnswers[changedAnswer.qNumber];
            answerList.Add(GetTextFromObject(changedAnswer.inputObject));
        }
    }


    // Returns a dictionary of questions and their respective last answers

    private void GetLastAnswers()
    {
        foreach (AnswerInput answer in _answers)
        {
            string question = _allQuestions[answer.qNumber];
            string lastAnswer = _allSavedAnswers[answer.qNumber].Last();

            if (_lastQuestionsAndAnswers.ContainsKey(question) == false)
            {
                _lastQuestionsAndAnswers.Add(question, lastAnswer);
            } else
            {
                _lastQuestionsAndAnswers[question] = lastAnswer;
            }
        }
    }


    public void SetUpAnswersTextForEmail()
    {
        // Reset message to update with new last answers.
        messageToSend = "";
    
        GetLastAnswers();

        messageToSend += "Dear student," + "\n" + "Here are the answers you gave to the questions in the Virtual Diffraction Lab:" + "\n";

        foreach (string q in _lastQuestionsAndAnswers.Keys)
        {
            string a = _lastQuestionsAndAnswers[q];

            if (a != null && a != "")
            {
                messageToSend += "\n" + q + "\n" + a + "\n";
            }
            else
            {
                messageToSend += "\n" + q + "\n Unanswered.\n";
            }
            
        }
    }
}

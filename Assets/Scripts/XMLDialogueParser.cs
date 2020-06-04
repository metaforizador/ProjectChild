using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

public struct Question {
    [XmlElement("area")]
    public string area;

    [XmlElement("questionText")]
    public string questionText;

    [XmlArray("answers")]
    [XmlArrayItem("answer")]
    public Answer[] answers;
}

public struct Answer {
    [XmlElement("answerText")]
    public string answerText;

    [XmlElement("answerType")]
    public DialogueScript.AnswerType answerType;
}

[XmlRoot("root"), XmlType("questions")]
public class XMLDialogueParser {

    [XmlArray("questions")]
    [XmlArrayItem("question")]
    public List<Question> questions = new List<Question>();

    public static XMLDialogueParser Load(string path) {
        try {
            XmlSerializer serializer = new XmlSerializer(typeof(XMLDialogueParser));
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                return serializer.Deserialize(stream) as XMLDialogueParser;
            }
        } catch (Exception e) {
            UnityEngine.Debug.LogError("Exception loading config file: " + e);

            return null;
        }
    }

    /// <summary>
    /// Returns a random question which is filtered by the parameter value.
    /// </summary>
    /// <param name="area">area for the question</param>
    /// <returns>random question</returns>
    public static Question GetRandomQuestion(string area) {
        // Create an array for the questions
        List<Question> questions = new List<Question>();

        XMLDialogueParser data = Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/Dialogues.xml"));
        // Add all questions to list which belong to this area
        foreach (Question o in data.questions) {
            if (o.area.Equals(area)) {
                questions.Add(o);
            }
        }

        // Throw error if no questions are found with provided parameter
        if (questions.Count == 0) {
            throw new Exception($"There are no questions defined for {area}!");
        }

        int questionIndex = 0;
        // If there are more than 1 questions in this area, randomize question
        if (questions.Count > 1) {
            questionIndex = UnityEngine.Random.Range(0, questions.Count);   // If count is 5, random returns values between 0 and 4
        }

        return questions[questionIndex];
    }
}

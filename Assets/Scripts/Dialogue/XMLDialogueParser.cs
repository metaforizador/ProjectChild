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
    [XmlElement("answerType")]
    public WordsType answerType;

    [XmlElement("answerText")]
    public string answerText;
}

public struct Reply {
    [XmlElement("replyType")]
    public WordsType replyType;

    [XmlElement("replyText")]
    public string replyText;
}

[XmlRoot("root"), XmlType("questions")]
public class XMLDialogueParser {

    [XmlArray("questions")]
    [XmlArrayItem("question")]
    public List<Question> questions = new List<Question>();

    [XmlArray("replies")]
    [XmlArrayItem("reply")]
    public List<Reply> replies = new List<Reply>();

    private static string path = Path.Combine(Application.dataPath, "Resources/XMLFiles/Dialogues.xml");

    public static XMLDialogueParser Load() {
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
    /// <param name="area">Area for the question</param>
    /// <returns>Random question</returns>
    public static Question GetRandomQuestion(string area) {
        // Create an array for the questions
        List<Question> questions = new List<Question>();

        XMLDialogueParser data = Load();
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

        // If count is 5, random returns values between 0 and 4
        return questions[UnityEngine.Random.Range(0, questions.Count)];
    }

    /// <summary>
    /// Returns a random reply which is filtered by the parameter type.
    /// </summary>
    /// <param name="type">WordsType of the reply</param>
    /// <returns>Random reply</returns>
    public static string GetRandomReply(WordsType type) {
        // Create an array for the replies
        List<Reply> replies = new List<Reply>();

        XMLDialogueParser data = Load();
        // Add all replies to list which belong to provided type
        foreach (Reply r in data.replies) {
            if (r.replyType == type) {
                replies.Add(r);
            }
        }

        // If count is 5, random returns values between 0 and 4

        Reply randomReply = replies[UnityEngine.Random.Range(0, replies.Count)];

        return randomReply.replyText;
    }
}

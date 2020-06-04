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
}

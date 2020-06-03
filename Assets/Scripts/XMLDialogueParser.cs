using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

public struct DialogueFile {
    [XmlElement("questionText")]
    public string questionText;

    [XmlElement("answer")]
    public string answer;
}

[XmlRoot("root"), XmlType("questions")]
public class XMLDialogueParser {

    [XmlArray("questions")]
    [XmlArrayItem("question")]
    public List<DialogueFile> questions = new List<DialogueFile>();

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

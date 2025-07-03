using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XmlLoader : MonoBehaviour
{
    public float mainBGM;
    public float narSound;
    public float waterSound;
    public float explosionSound;
    public float windSound;
    public float systemSound;
    public float dotSound;
    public float drumSound;
    public float maxSpeed;
    public bool type;

    private void Start()
    {
        GenerateXml();
    }

    public void GenerateXml()
    {
        string path = Environment.CurrentDirectory + "\\Setting.xml";
        if (!System.IO.File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            // 루트 노드 생성
            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "Setting", string.Empty);
            xmlDoc.AppendChild(root);
            #region Sound
            // 자식 노드 생성
            XmlNode sound = xmlDoc.CreateNode(XmlNodeType.Element, "Sound", string.Empty);
            root.AppendChild(sound);
            XmlElement mainBgm = xmlDoc.CreateElement("MainBGM");
            mainBgm.InnerText = "1";
            sound.AppendChild(mainBgm);
            XmlElement narSound = xmlDoc.CreateElement("NarSound");
            narSound.InnerText = "1";
            sound.AppendChild(narSound);
            XmlElement waterSound = xmlDoc.CreateElement("WaterSound");
            waterSound.InnerText = "1";
            sound.AppendChild(waterSound);
            XmlElement explosionSound = xmlDoc.CreateElement("ExplosionSound");
            explosionSound.InnerText = "1";
            sound.AppendChild(explosionSound);
            XmlElement windSound = xmlDoc.CreateElement("WindSound");
            windSound.InnerText = "1";
            sound.AppendChild(windSound);
            XmlElement systemSound = xmlDoc.CreateElement("SystemSound");
            systemSound.InnerText = "1";
            sound.AppendChild(systemSound);
            XmlElement dotSound = xmlDoc.CreateElement("DotSound");
            dotSound.InnerText = "0.4";
            sound.AppendChild(dotSound);
            XmlElement drumSound = xmlDoc.CreateElement("DrumSound");
            drumSound.InnerText = "1";
            sound.AppendChild(drumSound);
            #endregion

            XmlNode type = xmlDoc.CreateNode(XmlNodeType.Element, "Type", string.Empty);
            root.AppendChild(type);

            xmlDoc.Save(path);
            GenerateXml();
        }
        else
        {
            StringReader stringReader = new StringReader(path);
            stringReader.Read();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Setting");
            foreach (XmlNode xml in xmlNodeList)
            {
                foreach (XmlNode subject in xml)
                {
                    if (subject.Name.Equals("Sound") && subject.HasChildNodes)
                    {
                        foreach (XmlNode type in subject)
                        {
                            if (type.Name.Equals("MainBGM"))
                            {
                                mainBGM = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("NarSound"))
                            {
                                narSound = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("WaterSound"))
                            {
                                waterSound = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("ExplosionSound"))
                            {
                                explosionSound = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("WindSound"))
                            {
                                windSound = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("SystemSound"))
                            {
                                systemSound = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("DotSound"))
                            {
                                dotSound = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("DrumSound"))
                            {
                                drumSound = float.Parse(type.InnerText);
                            }
                            if (type.Name.Equals("MaxSpeed"))
                            {
                                maxSpeed = float.Parse(type.InnerText);
                            }
                        }
                    }
                }
            }
        }
    }
}

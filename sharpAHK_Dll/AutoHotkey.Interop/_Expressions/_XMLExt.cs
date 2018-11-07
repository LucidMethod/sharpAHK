using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AHKExpressions
{
    public static partial class StringExtension
    {
        // === XML ===


        // Helper method for getting inner text of named XML element (From File or XML String)
        static private string XMLGetValue(string XML_StringOrPath, string name, string defaultValue = "")
        {
            if (!File.Exists(XML_StringOrPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XML_StringOrPath);

                XmlElement elValue = doc.DocumentElement.SelectSingleNode(name) as XmlElement;
                return (elValue == null) ? defaultValue : elValue.InnerText;
                //return defaultValue;
            }
            else  // xml file path passed in 
            {
                try
                {
                    XmlDocument docXML = new XmlDocument();
                    docXML.Load(XML_StringOrPath);
                    XmlElement elValue = docXML.DocumentElement.SelectSingleNode(name) as XmlElement;
                    return (elValue == null) ? defaultValue : elValue.InnerText;
                }
                catch
                {
                    return defaultValue;
                }
            }


        }

        // Helper method to set inner text of named element.  Creates document if it doesn't exist
        static public void XMLSetValue(string XML_FilePath, string name, string stringValue)
        {
            XmlDocument docXML = new XmlDocument();
            XmlElement elRoot = null;
            if (!File.Exists(XML_FilePath))
            {
                elRoot = docXML.CreateElement("root");
                docXML.AppendChild(elRoot);
            }
            else
            {
                docXML.Load(XML_FilePath);
                elRoot = docXML.DocumentElement;
            }
            XmlElement value = docXML.DocumentElement.SelectSingleNode(name) as XmlElement;
            if (value == null)
            {
                value = docXML.CreateElement(name);
                elRoot.AppendChild(value);
            }
            value.InnerText = stringValue;
            docXML.Save(XML_FilePath);
        }



    }
}

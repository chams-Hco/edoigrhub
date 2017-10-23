using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace ChamsICSLib.Utilities
{
    public static class XMLHelper
    {
        static public string serializeObjectToXMLString(Object obj)
        {
            string serializedString = String.Empty;
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, obj, namespaces);
                ms.Position = 0; //Position is currently at end, we need to set it to 0
                using (TextReader reader = new StreamReader(ms))
                {
                    serializedString = reader.ReadToEnd();
                }
            }
            return serializedString;
        }

        static public Object deserializeXMLStringToObject(String xmlString, Type objectType)
        {
            XmlSerializer deserializer = new XmlSerializer(objectType);         
            StringReader sr = new StringReader(xmlString);
            Object obj;
            obj = deserializer.Deserialize(sr);
            sr.Close();

            return obj;
        }

        public static string SerializeToString(object o)
        {
            string serialized = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //Serialize to memory stream
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(o.GetType());
            System.IO.TextWriter w = new System.IO.StringWriter(sb);
            ser.Serialize(w, o);
            w.Close();

            //Read to string
            serialized = sb.ToString();
            return serialized;
        }

        public static string XmlSerialize(object o)
        {
            using (var stringWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.GetEncoding(1252),
                    OmitXmlDeclaration = true
                };
                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    var xmlSerializer = new XmlSerializer(o.GetType());
                    xmlSerializer.Serialize(writer, o);
                }
                return stringWriter.ToString();
            }
        }
    }
}

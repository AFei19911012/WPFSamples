using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demos.Helper
{
    ////
    /// ----------------------------------------------------------------
    /// Copyright @BigWang 2023 All rights reserved
    /// Author      : BigWang
    /// Created Time: 2023/4/27 23:01:55
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By     Modified Content
    /// V1.0.0.0     2023/4/27 23:01:55                     BigWang         首次编写         
    ///
    public class SerializeHelper
    {
        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        public static void ObjectToBinary<T>(T obj, string filename) where T : class
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                BinaryFormatter format = new BinaryFormatter();
                format.Serialize(fs, obj);
                fs.Close();
            }
        }

        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T BinaryToObject<T>(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                try
                {
                    BinaryFormatter format = new BinaryFormatter();
                    T obj = (T)format.Deserialize(fs);
                    fs.Close();
                    return obj;
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }


        /// <summary>
        /// Json 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        public static void ObjectToJson<T>(T obj, string filename) where T : class
        {
            var json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filename, json);
        }

        /// <summary>
        /// Json 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string filename)
        {
            var json = File.ReadAllText(filename);
            var setting = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            };
            return JsonConvert.DeserializeObject<T>(json, setting);
        }


        /// <summary>
        /// xml 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        public static void ObjectToXml<T>(T obj, string filename)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(filename))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(xmlWriter, obj);
            }
        }

        /// <summary>
        /// xml 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T XmlToObject<T>(string filename) where T : class
        {
            using (XmlReader xmlReader = XmlReader.Create(filename))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}
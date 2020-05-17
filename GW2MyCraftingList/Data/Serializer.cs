using System;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Text;

namespace GW2ExplorerCraftTool.Data
{
    class Serializer<T>
    {
        public static T Deserialize(string json)
        {
            return new JavaScriptSerializer().Deserialize<T>(json);
        }
        public static string Serialize(T obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        public static T Deserialize(System.IO.Stream json)
        {
            return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(json);
        }
    }
}

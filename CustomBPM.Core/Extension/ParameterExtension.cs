using System;
using System.Collections.Generic;
using System.Linq;
using CustomBPM.Core.DataModel;

namespace CustomBPM.Core.Extension
{
    public static class ParameterExtension
    {
        /// <summary>
        /// Получение и из словаря и приведение  к заданному типу
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="variableName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetParameter<T>(this IDictionary<string, string> parameters, string variableName) 
        {
            string stringValue;
            if (parameters.TryGetValue(variableName, out stringValue))
            {
                return CastString<T>(stringValue);
            }
            return default(T);
            
        }

        /// <summary>
        /// Получение и из словаря и приведение  к заданному типу
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="variableName">Имя параметра</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetParameter<T>(this ProcessInstance processInstance, string variableName)
        {
            var field = processInstance.Fields.SingleOrDefault(x => x.Name == variableName);
            if (field != null && field.Value != null)
            {
                return CastString<T>(field.Value);
            }
            return default(T);

        }

        private static T CastString<T>(string value)
        {
            if (typeof(T).IsArray)
            {
                    Type elementType = typeof(T).GetElementType();
                    var arrayString = value.Split(';').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    Array array = Array.CreateInstance(elementType, arrayString.Length);
                    for (int i = 0; i < array.Length; i++)
                    {
                        var val = Convert.ChangeType(arrayString[i], elementType);
                        array.SetValue(val,i); 
                    }
                    return (T)(object)array;
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Кастуем в вид пригодный для хранения
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToParameterValue(this object obj)
        {
           
            if (obj.GetType().IsArray)
            {
                string value ="";
                Array a = (Array) obj;
                foreach (var item in a)
                {
                    value = string.Concat(value, item + ";");
                }
                return value;
            }
            return obj.ToString();
        }
    }
}

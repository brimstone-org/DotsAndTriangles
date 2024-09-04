using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotsTriangle.Utils
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "Dots Triangles/Save Data")]
    public class SaveData : ISaveData
    {
        public override void GetKey<T>(string key, ref T value, T defValue)
        {
            if (typeof(T) == typeof(int))
            {
                if (PlayerPrefs.HasKey(key))
                    value = ConvertValue<T>(PlayerPrefs.GetInt(key));
                else
                {
                    value = defValue;
                    SetKey<T>(key, value);
                }
                
            }

            if (typeof(T) == typeof(float))
            {
                if (PlayerPrefs.HasKey(key))
                    value = ConvertValue<T>(PlayerPrefs.GetFloat(key));
                else
                {
                    value = defValue;
                    SetKey<T>(key, value);
                }

            }

            /*
            if (typeof(T) == typeof(bool))
            {
                if (PlayerPrefs.HasKey(key))
                    value = ConvertValue<T>(value);
                else
                {
                    value = defValue;
                    SetKey<int>(key, ConvertValue<int>(value));
                }
            }
            */

            if (typeof(T) == typeof(string))
            {
                if (PlayerPrefs.HasKey(key))
                    value = ConvertValue<T>(PlayerPrefs.GetString(key));
                else
                {
                    value = defValue;
                    SetKey<T>(key, value);
                }
            }
        }

        public override void SetKey<T>(string key, T value)
        {
            if (typeof(T) == typeof(int))
            {
                PlayerPrefs.SetInt(key, ConvertValue<int>(value));
            }

            if (typeof(T) == typeof(float))
            {
                PlayerPrefs.SetFloat(key, ConvertValue<float>(value));
            }

            if (typeof(T) == typeof(string))
            {
                PlayerPrefs.SetString(key, ConvertValue<string>(value));
            }

            /*
            if (typeof(T) == typeof(bool))
            {
                PlayerPrefs.SetInt(key, ConvertValue<int>(value));
            }
            */
        }

        public static T ConvertValue<T>(object value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Global
{
    public static int GetCurTime()
    {
        return (int)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds;
    }

    public static string ParseFileToJson(string path)
    {
        FileStream fs = File.Open(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string json = sr.ReadToEnd();
        sr.Close();
        fs.Close();

        return json;
    }
}

public class Log
{
    /// <summary>
    /// 日志打印
    /// </summary>
    public static void Info(string file_name, string log)
    {
        Console.WriteLine("file_name:" + file_name + " log:" + log);
    }

    /// <summary>
    /// 警告打印
    /// </summary>
    public static void Warn(params object[] str_list)
    {
        string str = MergeStr(str_list);
        Console.WriteLine("Warn: " + str);
    }

    /// <summary>
    /// 错误打印
    /// </summary>
    public static void Error(params object[] str_list)
    {
        string str = MergeStr(str_list);
        Console.WriteLine("Error: " + str);
    }

    /// <summary>
    /// 调试打印
    /// </summary>
    public static void Debug(params object[] str_list)
    {
        string str = MergeStr(str_list);
        Console.WriteLine("Debug: " + str);
    }

    private static string MergeStr(params object[] str_list)
    {
        string str = "";
        for (int i = 0; i < str_list.Length; i++)
        {
            str = str + str_list[i].ToString();
            if (i < str_list.Length - 1)
            {
                str += "|";
            }
        }
        return str;
    }
}


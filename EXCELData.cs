using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class EXCELData : MonoBehaviour
{
    public struct RPGExcelData
    {
        public string RPGspeakerName;
        public string RPGspeakingContent;
        public string RPGsmallheadpicture;
        public string RPGBackground;

    }

    public static List<RPGExcelData> ReadRPGExcel(string filePath)//读入表格数据
    {
        List<RPGExcelData> excelData = new List<RPGExcelData>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        RPGExcelData data = new RPGExcelData();
                        data.RPGspeakerName = reader.IsDBNull(0) ? string.Empty : reader.GetValue(0)?.ToString();
                        data.RPGspeakingContent = reader.IsDBNull(1) ? string.Empty : reader.GetValue(1)?.ToString();
                        data.RPGsmallheadpicture = reader.IsDBNull(2) ? string.Empty : reader.GetValue(2)?.ToString();
                        data.RPGBackground = reader.IsDBNull(3) ? string.Empty : reader.GetValue(3)?.ToString();
                        excelData.Add(data);
                    }
                } while (reader.NextResult());
            }
        }
        return excelData;
    }
}

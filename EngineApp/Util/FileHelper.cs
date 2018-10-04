using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class FileHelper
    {
        public string ReadFile(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public void WriteFile(string content, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        public void CreateMICConf(MICSetting settings, string configOutputPath)
        {
            string templateFilePath = AppDomain.CurrentDomain.BaseDirectory + "config/DoubleMICConf.txt";

            string content =
                this.ReadFile(templateFilePath);

            content = string.Format(
                content
                , settings.AEC_Length
                , settings.MIC_Type == "2" ? string.Empty : "//"
                , settings.BF_Select_Angle
                , settings.DRC_Status == true ? string.Empty : "//"
                , settings.DRC_Gain
                , settings.AES_Status == true ? string.Empty : "//"
                , settings.AES_Level
                , settings.NR_Level
                , settings.MIC_Type == "2" ? string.Empty : "//"
                , settings.DOA_MIC_Interval
                , settings.AGC_Status == true ? string.Empty : "//"
                , settings.MIC_Type);
            this.WriteFile(content, configOutputPath);
        }

        public void UploadToServer(
            string localConfigPath,
            string serverConfigPath,
            string ip,
            string port,
            string user,
            string passWord)
        {
            string processPath = AppDomain.CurrentDomain.BaseDirectory + "Processer/PSCP.EXE";
            string argumentTemplate = "pscp {0} {1}@{2}:/{3} {4}";

            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo(processPath);
            processStartInfo.UseShellExecute = true;
            processStartInfo.RedirectStandardInput = false;
            processStartInfo.RedirectStandardOutput = false;
            processStartInfo.Arguments = string.Format(argumentTemplate
                , string.IsNullOrEmpty(port) ? string.Empty : "-P " + port
                , user
                , ip
                , localConfigPath
                , serverConfigPath);

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(processStartInfo);
        }
    }
}

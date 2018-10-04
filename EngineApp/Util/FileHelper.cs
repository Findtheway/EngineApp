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
                sw.Write(content);//
            }
        }

        public void CreateMICConf(MICSetting settings, string configOutputPath)
        {
            //配置模板路径
            string templateFilePath = AppDomain.CurrentDomain.BaseDirectory + "Config/DoubleMICConf.txt";

            //获取配置模板字符串
            string content =
                this.ReadFile(templateFilePath);

            //获取配置字符串
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

            //生成本地配置文件
            this.WriteFile(content, configOutputPath);
        }

        /// <summary>
        /// 获取调用命令字符串
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="localConfigPath"></param>
        /// <param name="serverConfigPath"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public string GetPSCPCommandArgument(
            CommondType commandType,
            string localConfigPath,
            string serverConfigPath,
            string ip,
            string port,
            string user,
            string passWord)
        {
            StringBuilder sb = new StringBuilder("pscp ");

            if (!string.IsNullOrEmpty(port))
                sb.AppendFormat("-P {0} ", port);

            if (commandType == CommondType.UpLoad)
            {
                sb.Append(localConfigPath);
                sb.AppendFormat(" {0}@{1}:{2}", user, ip, serverConfigPath);
            }
            else
            {
                sb.AppendFormat("{0}@{1}:{2} ", user, ip, serverConfigPath);
                sb.Append(localConfigPath);
            }

            return sb.ToString();
        }

        public void UploadMICConfgToServer(
            string localConfigPath,
            string serverConfigPath,
            string ip,
            string port,
            string user,
            string passWord)
        {
            this.ExcutePSCPCommand(this.GetPSCPCommandArgument(CommondType.UpLoad, localConfigPath, serverConfigPath, ip, port, user, passWord));
        }

        public void DownLoadMICConfg(
            string localConfigPath,
            string serverConfigPath,
            string ip,
            string port,
            string user,
            string passWord)
        {
            this.ExcutePSCPCommand(this.GetPSCPCommandArgument(CommondType.DownLoad, localConfigPath, serverConfigPath, ip, port, user, passWord));
        }

        public void ExcutePSCPCommand(string commandArguments)
        {
            string processPath = AppDomain.CurrentDomain.BaseDirectory + "Processer/PSCP.EXE";
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo(processPath);
            processStartInfo.UseShellExecute = true;
            processStartInfo.RedirectStandardInput = false;
            processStartInfo.RedirectStandardOutput = false;
            processStartInfo.Arguments = commandArguments;

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(processStartInfo);
        }
    }

    public enum CommondType
    {
        UpLoad = 1,
        DownLoad = 2
    }
}

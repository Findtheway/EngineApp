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

        public void CreateMICConf(MICSetting settings, string configOutputPath,string fileName)
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
                , settings.AES_Status == true ? "//" : "//"
                , settings.AES_Level
                , settings.NR_Level
                , settings.MIC_Type == "2" ? string.Empty : "//"
                , settings.DOA_MIC_Interval
                , settings.MIC_Type == "2" ? string.Empty : "//"
                , settings.MIC_Type);
            
            if (!Directory.Exists(configOutputPath)) {
                Directory.CreateDirectory(configOutputPath);
            }
            //生成本地配置文件
            this.WriteFile(content, System.IO.Path.Combine(configOutputPath,fileName));
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
            StringBuilder sb = new StringBuilder("");

            if (!string.IsNullOrEmpty(port))
                sb.AppendFormat("-P {0} -pw {1} ", port, passWord);

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

        public Task UploadMICConfgToServer(
            string localConfigPath,
            string serverConfigPath,
            string ip,
            string port,
            string user,
            string passWord)
        {
            return Task.Run(() =>
            {
                //this.ExcutePSCPCommand(this.GetPSCPCommandArgument(CommondType.UpLoad, localConfigPath, serverConfigPath, ip, port, user, passWord));
                SCPOperation scp = new SCPOperation(ip, port, user, passWord);
                scp.Put(localConfigPath, serverConfigPath);
            });
        }

        public Task DownLoadMICConfg(
            string localConfigPath,
            string serverConfigPath,
            string ip,
            string port,
            string user,
            string passWord)
        {
            return Task.Run(() =>
            {
                //this.ExcutePSCPCommand(this.GetPSCPCommandArgument(CommondType.DownLoad, localConfigPath, serverConfigPath, ip, port, user, passWord));
                SCPOperation scp = new SCPOperation(ip,port,user,passWord);
                scp.Get(serverConfigPath, localConfigPath);
            });
        }

        public void ExcutePSCPCommand(string commandArguments)
        {
            string processPath = AppDomain.CurrentDomain.BaseDirectory + "processer/PSCP.EXE";
            using (System.Diagnostics.Process exep = new System.Diagnostics.Process())
            {

                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.FileName = processPath;
                startInfo.Arguments = commandArguments;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                exep.StartInfo = startInfo;
                exep.Start();
                exep.WaitForExit();
            }
        }

        public MICSetting readMicConfFile(string filePath, string fileName)
        {
            MICSetting micSetting = null;
            if (File.Exists(filePath + fileName)) {
                micSetting = new MICSetting();
                FileStream fs = new FileStream(filePath + fileName, FileMode.Open);
                StreamReader streamReader = new StreamReader(fs);
                string line = "";
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Contains("AECParam"))
                    {
                        micSetting.AEC_Length = this.getConfValue(line, 0);
                    }
                    else if (line.Contains("BFParam"))
                    {
                        micSetting.BF_Select_Angle = this.getConfValue(line, 4);
                    }
                    else if (line.Contains("DRCParam"))
                    {
                        if (line.StartsWith("//"))
                        {
                            micSetting.DRC_Status = false;
                        }
                        else
                        {
                            micSetting.DRC_Status = true;
                        }
                        micSetting.DRC_Gain = this.getConfValue(line, 0);
                    }
                    else if (line.Contains("ESParam"))
                    {
                        if (line.StartsWith("//"))
                        {
                            micSetting.AES_Status = false;
                        }
                        else
                        {
                            micSetting.AES_Status = true;
                        }
                        micSetting.AES_Level = this.getConfValue(line, 0);
                    }
                    else if (line.Contains("NRParam"))
                    {
                        micSetting.NR_Level = this.getConfValue(line, 0);
                    }
                    else if (line.Contains("DOAParam"))
                    {
                        micSetting.DOA_MIC_Interval = this.getConfValue(line, 2);
                    }
                    //else if (line.Contains("AGCParam"))
                    //{
                    //    if (line.StartsWith("//"))
                    //    {
                    //        micSetting.AGC_Status = false;
                    //    }
                    //    else
                    //    {
                    //        micSetting.AGC_Status = true;
                    //    }
                    //}
                    else if (line.Contains("MICParam"))
                    {
                        micSetting.MIC_Type = this.getConfValue(line, 0);
                    }
                }
                streamReader.Close();
                fs.Close();
            }          
            return micSetting;
        }

        public string getConfValue(string line,int num) {
            string value = "";
            string[] strs1 = line.Split(':');
            string[] str2 = strs1[1].Split('=');
            value = str2[1].Trim().TrimStart('[').TrimEnd(']').Split(',')[num];
            return value;
        }
    }


    public enum CommondType
    {
        UpLoad = 1,
        DownLoad = 2
    }
}

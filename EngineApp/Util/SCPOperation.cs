using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Renci.SshNet;

namespace Util
{
    public class SCPOperation
    {
        private ScpClient scp;
        public bool Connected { get { return scp.IsConnected; } }
        public SCPOperation(string ip, string port, string user, string pwd) {
            scp = new ScpClient(ip, Int32.Parse(port), user, pwd);
        }
        public bool Connect() {
            try
            {
                if (!Connected) {
                    scp.Connect();
                }
                return true;
               
            }
            catch (Exception ex) {
                throw new Exception(string.Format("连接SCP失败，原因：{0}", ex.Message));
            }
        }

        public void Disconnect()
        {
            try {
                if (scp != null && Connected)
                {
                    scp.Disconnect();
                }
            }
            catch (Exception ex) {
                throw new Exception(string.Format("断开SCP失败，原因：{0}", ex.Message));
            }
            
        }
        #region SCP上传文件        
        /// <summary>        
        /// SFTP上传文件        
        /// </summary>        
        /// <param name="localPath">本地路径</param>        
        /// <param name="remotePath">远程路径</param>        
        public void Put(string localPath, string remotePath)
        {
            try
            {
                using (var file = File.OpenRead(localPath))
                {
                    Connect();
                    scp.Upload(file, remotePath);
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SCP文件上传失败，原因：{0}", ex.Message));
            }
        }
        #endregion
        #region SCP获取文件        
        /// <summary>        
        /// SFTP获取文件        
        /// </summary>        
        /// <param name="remotePath">远程路径</param>        
        /// <param name="localPath">本地路径</param>        
        public void Get(string remotePath, string localPath)
        {
            try
            {
                Connect();
                DirectoryInfo localdir = new DirectoryInfo(localPath);
                scp.Download(remotePath, localdir);
                Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SCP文件获取失败，原因：{0}", ex.Message));
            }
        }
        #endregion
        #region 连接ssh，然后执行命令
        public static void RestartDSP(string ip, string port, string user, string pwd,int type) {
            //创建SSH connection
            using (var ssh = new SshClient(ip, Convert.ToInt32(port), user, pwd))
            {

                //启动连接
                ssh.Connect();
                //执行重启脚本
                var output = ssh.RunCommand(string.Format("/usr/bin/RestartDSP.sh {0}", type));
                //释放连接
                ssh.Disconnect();
            }
        }
        #endregion

    }
}

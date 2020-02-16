using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Infrastructure.CMD
{
    public static class CmdManager
    {
        public static void RunCommand(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $@"/C {command}";
            process.StartInfo = startInfo;
            bool started = process.Start();
            if (!started) throw new Exception("erro");
            process.WaitForExit();
            process.Close();
        }
    }
}

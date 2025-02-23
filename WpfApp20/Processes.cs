using System;
using System.Diagnostics;

namespace WpfApp20
{
    public class Processes
    {
        public int ProcId { get; set; }
        public Process RefProc { get; set; }
        public string ProcName { get; set; }

        public Processes(int procId, string procName, Process refProc)
        {
            ProcId = procId;
            ProcName = procName;
            RefProc = refProc;
        }

        public override string ToString()
        {
            return $"{ProcName} (ID: {ProcId})";
        }
        public string GetDetails()
        {
            return $"Process Name: {ProcName}\nProcess ID: {ProcId}\nStart Time: {RefProc.StartTime}";
        }
    }
}

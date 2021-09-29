using System.Runtime.InteropServices;

namespace HalconWPF.Method
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W 2021 All rights reserved
    /// Author      : Taosy.W
    /// Created Time: 2021/9/29 15:55:15
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time         Modified By    Modified Content
    /// V1.0.0.0     2021/9/29 15:55:15    Taosy.W                 
    ///
    public class BeepMethod
    {
        public const uint BeepOk = 0x00000000;
        public const uint BeepError = 0x00000010;
        public const uint BeepQuestion = 0x00000020;
        public const uint BeepWarning = 0x00000030;
        public const uint BeepInformation = 0x00000040;

        [DllImport("user32.dll")]
        public static extern int MessageBeep(uint beepType);
        // MessageBeep(BeepOk); 

        [DllImport("kernel32.dll", EntryPoint = "Beep")]
        public static extern int Beep(int dwFreq, int dwDuration);
        // BeepMethod.Beep(800, 300);

        [DllImport("winmm.dll")]
        public static extern bool PlaySound(string filename, int Mod, int Flags);
        // PlaySound(@"C:\Windows\Media\Alarm01.wav", 0, 1);
        // 连续播放
        // PlaySound(@"C:\Windows\Media\Alarm01.wav", 0, 9);
    }
}

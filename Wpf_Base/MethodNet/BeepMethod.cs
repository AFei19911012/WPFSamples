using System.Runtime.InteropServices;
using System.Speech.Synthesis;

namespace Wpf_Base.MethodNet
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @CoderMan/CoderdMan1012 2022 All rights reserved
    /// Author      : CoderMan/CoderdMan1012
    /// Created Time: 22/09/01 17:13:23
    /// Description :
    /// ----------------------------------------------------------------
    /// Version      Modified Time              Modified By                               Modified Content
    /// V1.0.0.0     22/09/01 17:13:23    CoderMan/CoderdMan1012         首次编写         
    ///
    public static class BeepMethod
    {
        public const uint BeepOk = 0x00000000;
        public const uint BeepError = 0x00000010;
        public const uint BeepQuestion = 0x00000020;
        public const uint BeepWarning = 0x00000030;
        public const uint BeepInformation = 0x00000040;
        public static SpeechSynthesizer Synthesizer { get; set; } = new SpeechSynthesizer();

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


        /// <summary>
        /// 语音播报
        /// </summary>
        /// <param name="text"></param>
        public static void Speak(string text)
        {
            _ = Synthesizer.SpeakAsync(text);
        }
    }
}
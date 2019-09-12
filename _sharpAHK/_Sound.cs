using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === Sound Commands ===

        /// <summary>Emits a tone from the PC speaker.</summary>
        /// <param name="Frequency">The frequency of the sound, which can be an expression. It should be a number between 37 and 32767. If omitted, the frequency will be 523.</param>
        /// <param name="Duration">The duration of the sound, in milliseconds (can be an expression). If omitted, the duration will be 150.</param>
        public void SoundBeep(object Frequency = null, object Duration = null)
        {
            string frequencey = ""; string duration = "";
            if (Frequency != null) { frequencey = Frequency.ToString(); }
            if (Duration != null) { duration = Duration.ToString(); }

            ErrorLog_Setup(false);
            Execute("SoundBeep, " + frequencey + "," + duration);
        }

        /// <summary>Retrieves various settings from a sound device (master mute, master volume, etc.)</summary>
        /// <param name="ComponentType">If omitted or blank, it defaults to the word MASTER. Otherwise, it can be one of the following words: MASTER (synonymous with SPEAKERS), DIGITAL, LINE, MICROPHONE, SYNTH, CD, TELEPHONE, PCSPEAKER, WAVE, AUX, ANALOG, HEADPHONES, or N/A. If the sound device lacks the specified ComponentType, ErrorLevel will indicate the problem. The component labled Auxiliary in some mixers might be accessible as ANALOG rather than AUX. If a device has more than one instance of ComponentType (two of type LINE, for example), usually the first contains the playback settings and the second contains the recording settings. To access an instance other than the first, append a colon and a number to this parameter. For example: Analog:2 is the second instance of the analog component.</param>
        /// <param name="ControlType">If omitted or blank, it defaults to VOLUME. Otherwise, it can be one of the following words: VOLUME (or VOL), ONOFF, MUTE, MONO, LOUDNESS, STEREOENH, BASSBOOST, PAN, QSOUNDPAN, BASS, TREBLE, EQUALIZER, or the number of a valid control type (see soundcard analysis script). If the specified ComponentType lacks the specified ControlType, ErrorLevel will indicate the problem.</param>
        /// <param name="DeviceNumber">If this parameter is omitted, it defaults to 1 (the first sound device), which is usually the system's default device for recording and playback. Specify a number higher than 1 to operate upon a different sound device. This parameter can be an expression.</param>
        public string SoundGet(string ComponentType = "", string ControlType = "", string DeviceNumber = "")
        {
            string AHKLine = "SoundGet, OutputVar, " + ComponentType + "," + ControlType + "," + DeviceNumber;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Retrieves the wave output volume for a sound device.</summary>
        /// <param name="DeviceNumber">If this parameter is omitted, it defaults to 1 (the first sound device), which is usually the system's default device for recording and playback. Specify a number higher than 1 to operate upon a different sound device.</param>
        public string SoundGetWaveVolume(string DeviceNumber = "")
        {
            string AHKLine = "SoundGetWaveVolume, OutputVar, " + DeviceNumber;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            string OutVar = Execute(AHKLine, "OutputVar");   // execute AHK code and return variable value 
            return OutVar;
        }

        /// <summary>Plays a sound, video, or other supported file type.</summary>
        /// <param name="Filename">The name of the file to be played, which is assumed to be in %A_WorkingDir% if an absolute path isn't specified. To produce standard system sounds, specify an asterisk followed by a number as shown below. Note: the wait parameter has no effect in this mode. *-1: Simple beep. If the sound card is not available, the sound is generated using the speaker. *16: Hand (stop/error) *32: Question *48: Exclamation *64: Asterisk (info) </param>
        /// <param name="WaitUntilFinished">If False, the script's current thread will move on to the next commmand(s) while the file is playing. To avoid this, set Wait to TRUE, which causes the current thread to wait until the file is finished playing before continuing. Even while waiting, new threads can be launched via hotkey, custom menu item, or timer. Known limitation: If the WAIT parameter is omitted, the OS might consider the playing file to be "in use" until the script closes or until another file is played (even a nonexistent file).</param>
        public void SoundPlay(string Filename, bool WaitUntilFinished = false)
        {
            string wait = "";
            if (WaitUntilFinished) { wait = "Wait"; }

            ErrorLog_Setup(false);
            Execute("SoundPlay, " + Filename + "," + wait);
        }

        /// <summary>Changes various settings of a sound device (master mute, master volume, etc.)</summary>
        /// <param name="NewSetting">Percentage number between -100 and 100 inclusive (it can be a floating point number or expression). If the number begins with a plus or minus sign, the current setting will be adjusted up or down by the indicated amount. Otherwise, the setting will be set explicitly to the level indicated by NewSetting. For ControlTypes with only two possible settings -- namely ONOFF, MUTE, MONO, LOUDNESS, STEREOENH, and BASSBOOST -- any positive number will turn on the setting and a zero will turn it off. However, if the number begins with a plus or minus sign, the setting will be toggled (set to the opposite of its current state).</param>
        /// <param name="ComponentType">If omitted or blank, it defaults to the word MASTER. Otherwise, it can be one of the following words: MASTER (synonymous with SPEAKERS), DIGITAL, LINE, MICROPHONE, SYNTH, CD, TELEPHONE, PCSPEAKER, WAVE, AUX, ANALOG, HEADPHONES, or N/A. If the sound device lacks the specified ComponentType, ErrorLevel will indicate the problem. The component labeled Auxiliary in some mixers might be accessible as ANALOG rather than AUX. If a device has more than one instance of ComponentType (two of type LINE, for example), usually the first contains the playback settings and the second contains the recording settings. To access an instance other than the first, append a colon and a number to this parameter. For example: Analog:2 is the second instance of the analog component.</param>
        /// <param name="ControlType">If omitted or blank, it defaults to VOLUME. Otherwise, it can be one of the following words: VOLUME (or VOL), ONOFF, MUTE, MONO, LOUDNESS, STEREOENH, BASSBOOST, PAN, QSOUNDPAN, BASS, TREBLE, EQUALIZER, or the number of a valid control type (see soundcard analysis script). If the specified ComponentType lacks the specified ControlType, ErrorLevel will indicate the problem.</param>
        /// <param name="DeviceNumber">If this parameter is omitted, it defaults to 1 (the first sound device), which is usually the system's default device for recording and playback. Specify a number higher than 1 to operate upon a different sound device. This parameter can be an expression.</param>
        public bool SoundSet(string NewSetting, string ComponentType = "", string ControlType = "", string DeviceNumber = "")
        {
            string AHKLine = "SoundSet, " + NewSetting + "," + ComponentType + "," + ControlType + "," + DeviceNumber;  // ahk line to execute
            ErrorLog_Setup(true); // ErrorLevel Detection Enabled for this function in AHK 
            Execute(AHKLine);   // execute AHK code and return variable value

            if (!ahkGlobal.ErrorLevel) { return true; } // no error level - return true for success
            return false;  // error level detected - success = false
        }

        /// <summary>Changes the wave output volume for a sound device.</summary>
        /// <param name="Percent">Percentage number between -100 and 100 inclusive (it can be a floating point number or an expression). If the number begins with a plus or minus sign, the current volume level will be adjusted up or down by the indicated amount. Otherwise, the volume will be set explicitly to the level indicated by Percent.</param>
        /// <param name="DeviceNumber">If this parameter is omitted, it defaults to 1 (the first sound device), which is usually the system's default device for recording and playback. Specify a number higher than 1 to operate upon a different sound device.</param>
        public void SoundSetWaveVolume(string Percent, string DeviceNumber = "")
        {
            ErrorLog_Setup(false);
            Execute("SoundSetWaveVolume, " + Percent + "," + DeviceNumber);
        }


        #endregion
    }
}

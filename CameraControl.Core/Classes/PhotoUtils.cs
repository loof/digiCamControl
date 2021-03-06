#region Licence

// Distributed under MIT License
// ===========================================================
// 
// digiCamControl - DSLR camera remote control open source software
// Copyright (C) 2014 Duka Istvan
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY,FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region

using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading;
using CameraControl.Devices;

#endregion

namespace CameraControl.Core.Classes
{
    public class PhotoUtils
    {
        public static bool RunAndWait(string exe, string param)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(exe);
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
                startInfo.Arguments = param;
                Process process = Process.Start(startInfo);
                process.WaitForExit();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Run(string exe)
        {
            return Run(exe, "", ProcessWindowStyle.Minimized);
        }

        public static bool Run(string exe, string param)
        {
            return Run(exe, param, ProcessWindowStyle.Minimized);
        }

        public static bool Run(string exe, string param, ProcessWindowStyle processWindowStyle)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(exe);
                startInfo.WindowStyle = processWindowStyle;
                if (!string.IsNullOrEmpty(param))
                    startInfo.Arguments = param;
                Process process = Process.Start(startInfo);
                //process.WaitForExit();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return false;
            }
            return true;
        }

        [Obsolete("Not used anymore", false)]
        public static void CopyPhotoScale(string sourse, string dest, double scale)
        {
            File.Copy(sourse, dest, true);
        }


        public static void PlayCaptureSound()
        {
            try
            {
                string basedir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (basedir != null)
                {
                    var mplayer = new SoundPlayer(Path.Combine(basedir, "Data", "takephoto.wav"));
                    mplayer.Play();
                }
            }
            catch (Exception exception)
            {
                Log.Debug(exception);
            }
        }

        public static void Donate()
        {
            Run("http://www.digicamcontrol.com/donate/");
        }

        public static bool IsFileLocked(string file)
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(file,FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static void WaitForFile(string file)
        {
            int retry = 15;
            while (IsFileLocked(file) && retry>0 )
            {
                Thread.Sleep(100);
                retry--;
            }
        }

    }
}
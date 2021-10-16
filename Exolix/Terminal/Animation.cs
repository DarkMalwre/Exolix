using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Terminal
{
    public class AnimationSettings
    {
        public string? Prefix = "";

        public string[]? Frames = { "|", "/", "-", "\\" };

        public string? State = "processing";

        public int? Interval = 100;
    }

    public class Animation
    {
        private static bool Running = false;
        private static Thread? RenderingThreadInstance;
        private static int CurrentFrame = 0;
        private static AnimationSettings? Settings;

        public static void Start(string label, AnimationSettings? settings)
        {
            if (settings == null)
            {
                settings = new AnimationSettings();
            }

            Settings = settings;

            Running = true;
            CurrentFrame = 0;

            Thread renderingThread = new Thread(new ThreadStart(FrameRenderingThread));
            renderingThread.Start();

            RenderingThreadInstance = renderingThread;
        }

        public static void Stop(string? label)
        {
            if (Running)
            {
                Running = false;
                return;
            }

            new Exception("Animation is not running");
        }

        private static void FrameRenderingThread()
        {
            do
            {
                CurrentFrame++;
                if (CurrentFrame > Settings!.Frames!.Length - 1)
                {
                    CurrentFrame = 0;
                }

                Console.Write("\r" + Settings!.Frames[CurrentFrame] + " Label Is Here");

                Thread.Sleep((int)Settings!.Interval!);
            } while (Running);
        }
    }
}

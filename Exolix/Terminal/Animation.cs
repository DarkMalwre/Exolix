using Pastel;
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

        public string[]? Frames = { "   ",
            ".  ".Pastel("#60cdff"),
            ".. ".Pastel("#60cdff"),
            "...".Pastel("#60cdff"),
            " ..".Pastel("#60cdff"),
            "  .".Pastel("#60cdff")
        };

        public string? State = "processing";

        public int? Interval = 100;
    }

    public class Animation
    {
        private static bool Running = false;
        private static Thread? RenderingThreadInstance;
        private static int CurrentFrame = 0;
        private static AnimationSettings? Settings;
        private static string Label = "";

        public static void Start(string label, AnimationSettings? settings)
        {
            if (settings == null)
            {
                settings = new AnimationSettings();
            }

            Settings = settings;

            Running = true;
            CurrentFrame = 0;
            Label = label;

            Thread renderingThread = new Thread(new ThreadStart(FrameRenderingThread));
            renderingThread.Start();

            RenderingThreadInstance = renderingThread;
        }

        public static void Stop(string? label)
        {
            if (Running)
            {
                if (label != null)
                {
                    Label = label;
                }

                Running = false;
                RenderCurrentFrame();
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

                RenderCurrentFrame();
                Thread.Sleep((int)Settings!.Interval!);
            } while (Running);
        }

        public static void RenderCurrentFrame()
        {
            int consoleWith = Console.WindowWidth - Settings!.Frames[CurrentFrame].Length;
            if (consoleWith < 0)
            {
                consoleWith = 0;
            }

            string outputLabel = Label;
            string suffixSpacing = "";

            if (outputLabel.Length > consoleWith)
            {
                outputLabel = outputLabel.Substring(0, consoleWith);
            }
            else
            {
                int suffixLength = consoleWith - outputLabel.Length - Settings!.Frames[CurrentFrame].Length;
                if (suffixLength < 0)
                {
                    suffixLength = 0;
                }

                suffixSpacing = new string(' ', suffixLength);
            }

            Console.Write($"\r{Settings!.Frames[CurrentFrame]} {outputLabel}{suffixSpacing}");
        }
    }
}

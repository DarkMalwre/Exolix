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

        public int? Interval = 100;
    }

    public class Animation
    {
        private static Thread? RenderingThreadInstance;
        private static AnimationSettings? Settings;
        private static string State = "processing";
        private static int CurrentFrame = 0;
        private static bool Running = false;
        private static string Label = "";

        public static void Start(string label, AnimationSettings? settings = null)
        {
            if (settings == null)
            {
                settings = new AnimationSettings();
            }

            Settings = settings;
            State = "processing";
            Running = true;
            CurrentFrame = 0;
            Label = label;

            Thread renderingThread = new Thread(new ThreadStart(FrameRenderingThread));
            renderingThread.Start();

            RenderingThreadInstance = renderingThread;
        }

        public static void Stop(string? label = "", string newState = "success")
        {
            if (newState == "success" || newState == "processing" || newState == "error" || newState == "warning")
            {
                if (Running)
                {
                    if (label != null)
                    {
                        Label = label;
                    }

                    Running = false;
                    string prefixHex = "#60cdff";

                    if (newState == "success")
                    {
                        prefixHex = "#60cdff";
                    } else if (newState == "error")
                    {
                        prefixHex = "#ff0055";
                    } else if (prefixHex == "warning")
                    {
                        prefixHex = "#FFA500";
                    }

                    RenderCurrentFrame("·".Pastel(prefixHex));
                    return;
                }

                new Exception("Animation is not running");
                return;
            }

            new Exception("Invalid state type, the following states are supported [ \"processing\", \"success\", \"warning\", \"error\" ] ");
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

        public static void RenderCurrentFrame(string? prefixIcon = null)
        {
            int consoleWith = Console.WindowWidth - Settings!.Frames![CurrentFrame].Length;
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

            if (prefixIcon == null)
            {
                prefixIcon = Settings!.Frames[CurrentFrame];
            }

            Console.Write($"\r {prefixIcon} {outputLabel}{suffixSpacing}");
        }
    }
}

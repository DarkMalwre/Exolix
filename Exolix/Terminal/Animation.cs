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
        public string Prefix = "";

        public string[] Frames = { "   ",
            ".  ",
            ".. ",
            "...",
            " ..",
            "  ."
        };

        public int Interval = 100;

        public string FrameHexColor = "60cdff";
    }

    public class Animation
    {
        private static Thread? RenderingThreadInstance;
        private static AnimationSettings? Settings;
        private static int CurrentFrame = 0;
        private static bool Running = false;
        private static string Label = "";
        private static string LastOutput = "";

        public static void Start(string label, AnimationSettings? settings = null)
        {
            if (settings == null)
            {
                settings = new AnimationSettings();
            }

            Settings = settings;
            Running = true;
            CurrentFrame = 0;
            Label = label;

            if (RenderingThreadInstance == null)
            {
                Thread renderingThread = new Thread(new ThreadStart(FrameRenderingThread));
                RenderingThreadInstance = renderingThread;
                renderingThread!.Start();
            } else if (!RenderingThreadInstance.IsAlive)
            {
                Thread renderingThread = new Thread(new ThreadStart(FrameRenderingThread));
                RenderingThreadInstance = renderingThread;
                renderingThread!.Start();
            }
        }

        public static void Stop(string? label = null, string newState = "success")
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
                    string prefixHex = "60CDFF";

                    if (newState == "success")
                    {
                        prefixHex = "50FFAB";
                    } else if (newState == "error")
                    {
                        prefixHex = "FF0055";
                    } else if (newState == "warning")
                    {
                        prefixHex = "FFA500";
                    }

                    RenderCurrentFrame("·", prefixHex);
                    Logger.PrintDynamic("\n");
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
                if (CurrentFrame > Settings!.Frames.Length - 1)
                {
                    CurrentFrame = 0;
                }

                RenderCurrentFrame();
                Thread.Sleep((int)Settings!.Interval);
            } while (Running);
        }

        public static void RenderCurrentFrame(string? prefixIcon = null, string? prefixHex = null)
        {
            string suffixSpacing = "";
            string outputLabel = Label;

            int consoleWidth = Console.WindowWidth;

            if (prefixIcon == null)
            {
                prefixIcon = Settings!.Frames[CurrentFrame];
            }

            if (consoleWidth - LastOutput.Length >= 0)
            {
                // TODO: Cut off label
            }

            if ($"{prefixIcon} {outputLabel}".Length < LastOutput.Length) 
            {
                int suffixSize = LastOutput.Length - $"{prefixIcon} {outputLabel}".Length;
                suffixSpacing = new string(' ', suffixSize);
            }

            string renderPrefixIcon = "";
            if (prefixHex == null)
            {
                renderPrefixIcon = prefixIcon.Pastel("#" + Settings!.FrameHexColor);
            } else
            {
                renderPrefixIcon = prefixIcon.Pastel("#" + prefixHex);
            }
             
            Logger.PrintDynamic($"\r {renderPrefixIcon} {outputLabel}{suffixSpacing}");
            LastOutput = $"{prefixIcon} {outputLabel}";
        }
    }
}

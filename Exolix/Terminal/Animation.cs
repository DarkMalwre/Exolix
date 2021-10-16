using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Terminal
{
    public class AnimationSettings
    {
        string Pref
        {
            get;
            set;
        }
    }

    public class Animation
    {
        public static void Start(string label)
        {
            AnimationSettings settings = new AnimationSettings();
        }
    }
}

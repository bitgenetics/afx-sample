using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets.Scenes.Afx
{
    public class AfxEffectResponse
    {
        public string id {  get; set; }
        public bool success {  get; set; }
        public string? message { get; set; }
        public string type { get; set; }
 
    }

    public class AfxEffect
    {
        /// <summary>
        /// An identifier that is type unique. It goes not need to be globally unique.
        /// </summary>
        /// <remarks>Example: drop_box_random_loot</remarks>
        public string game_fx_id { get; set; }

        /// <summary>
        /// The Name of the effect.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Details what the effect does.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// optional url to a preview mp4 video of the in game effect.
        /// </summary>
        public string preview_video_url { get; set; }
    }
}

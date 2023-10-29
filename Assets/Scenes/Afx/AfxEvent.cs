using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes.Afx
{

    public class AfxEvent
    {
        /// <summary>
        /// An identifier that is type unique. It goes not need to be globally unique.
        /// </summary>
        /// <remarks>Example: floor_switch_activated</remarks>
        public string game_event_id { get; set; }
        /// <summary>
        /// The display name of the event.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Details on what the event means.
        /// </summary>
        /// <remarks>Example: An in game actor stepped on a floor switch in the current active scene.</remarks>
        public string description { get; set; }

    }
}

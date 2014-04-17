using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneGameService.Models
{
    public class GameAudio
    {
        public int id { get; set; }
        public int gameId { get; set; }
        public int sequenceNumber { get; set; }
        public Uri rawAudioUri { get; set; }
        public Uri garbledAudioUri { get; set; }
    }
}

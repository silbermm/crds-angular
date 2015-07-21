using System;
using System.Collections.Generic;
using Crossroads.Utilities.Extensions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripParticipantDto
    {
        public TripParticipantDto()
        {
            Trips = new List<TripDto>();
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonIgnore]
        public string Lastname { get; set; }

        [JsonIgnore]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "participantName")]
        public string ParticipantName
        {
            get { return string.Format("{0} {1}", Nickname, Lastname); }
        }

        [JsonProperty(PropertyName = "participantPhotoUrl")]
        public string PhotoUrl
        {
            get
            {
                var photoLib = new List<string>
                {
                    "http://media.licdn.com/media/AAEAAQAAAAAAAAI-AAAAJDEzNzMwNjlmLTcyZWItNDMxZC05MzQwLWI4Yzk5OGI5Njk1Mg.jpg",
                    "http://oceanaccelerator.com/wp-content/uploads/2015/02/DSC_2552-Edit-4.jpg",
                    "http://media.licdn.com/mpr/mpr/shrinknp_400_400/p/1/005/03d/38e/2579bdb.jpg",
                    "http://lh3.googleusercontent.com/-B5w9gl6mDxg/AAAAAAAAAAI/AAAAAAAAAEA/xA_B6LXyZns/s120-c/photo.jpg",
                    "http://oceanaccelerator.com/wp-content/uploads/2015/02/Kelly_Dolan-2-e1423084931360.jpg",
                    "http://media.licdn.com/mpr/mpr/shrink_200_200/p/2/000/18d/22b/27f7bfa.jpg",
                    "http://eastgermancinema.files.wordpress.com/2015/04/wozzeck7.jpg"
                };
                var rnd = new Random(Guid.NewGuid().GetHashCode());
                var index = rnd.Next(0, 7);
                return photoLib[index];
                //return "http://crossroads-media.s3.amazonaws.com/images/avatar.svg";
            }
        }

        [JsonProperty(PropertyName = "trips")]
        public List<TripDto> Trips { get; set; }
    }
}
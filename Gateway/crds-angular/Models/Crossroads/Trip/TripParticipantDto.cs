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

        [JsonProperty(PropertyName = "showGiveButton")]
        public bool ShowGiveButton { get; set; }

        [JsonProperty(PropertyName = "showShareButtons")]
        public bool ShowShareButtons { get; set; }

        [JsonProperty(PropertyName = "participantPhotoUrl")]
        public string PhotoUrl
        {
            get
            {
                IDictionary<int, string> dict = new Dictionary<int,string>();
                //kevin
                dict[2375571] = "https://lh3.googleusercontent.com/-ytlmW8lBruE/AAAAAAAAAAI/AAAAAAAAAB4/FmQcCesR9sU/photo.jpg";
                //rick
                dict[2375566] = "http://lh3.googleusercontent.com/-B5w9gl6mDxg/AAAAAAAAAAI/AAAAAAAAAEA/xA_B6LXyZns/s120-c/photo.jpg";
                //kelly
                dict[2375564] = "http://oceanaccelerator.com/wp-content/uploads/2015/02/Kelly_Dolan-2-e1423084931360.jpg";
                //mk
                dict[2375565] = "http://oceanaccelerator.com/wp-content/uploads/2015/02/DSC_2552-Edit-4.jpg";
                //shankx
                dict[695942] = "http://media.licdn.com/mpr/mpr/shrinknp_400_400/p/1/005/03d/38e/2579bdb.jpg";
                //rich
                dict[2217368] = "http://media.licdn.com/media/AAEAAQAAAAAAAAI-AAAAJDEzNzMwNjlmLTcyZWItNDMxZC05MzQwLWI4Yzk5OGI5Njk1Mg.jpg";
                //const string defaultImg = "http://eastgermancinema.files.wordpress.com/2015/04/wozzeck7.jpg";
                const string defaultImg = "http://crossroads-media.s3.amazonaws.com/images/avatar.svg";

                return dict.ContainsKey(this.ParticipantId) ? dict[ParticipantId] : defaultImg;

                //return "http://crossroads-media.s3.amazonaws.com/images/avatar.svg";
            }
        }

        [JsonProperty(PropertyName = "trips")]
        public List<TripDto> Trips { get; set; }
    }
}
(function(){
  module.exports = function ServeOpportunities(){
    return [{
          "day": "Saturday, November 16, 2014",
          "opportunities" : [
            {
              "time": "8:30am",
              "name" : "Kids Club Nusery",
              "members" : [
                { "name": "John",
                  "roles" : [
                    {"name": "NuseryA"},
                    {"name": "NuseryB"},
                    {"name": "NuseryC"},
                    {"name": "NuseryD"}
                  ]
                },
                { "name":  "Jane",
                  "roles" : [
                    {"name": "NuseryA"},
                    {"name": "NuseryB"},
                    {"name": "NuseryC"},
                    {"name": "NuseryD"}
                  ],
                  "signedup" : "yes"
                },
              ]
            },
            {
              "time": "8:30am",
              "name": "First Impressions",
               "members" : [
                  { "name": "John"},
                  {"name": "Jane" } 
               ]
            }
          ]
        },
        {
          "day" :"Sunday, November 17, 2014",
          "opportunities": [
            {
              "time": "8:30am",
              "name" : "Kids Club Nusery",
              "members" : [
                { "name": "John"},
                { "name": "Jane"}
              ],
            },
            {
              "time": "8:30am",
              'name': "Kids Club 1st Grade",
              'members' : [
                {"name": "John"},
                {"name": "Jane"},
                {"name": "Samantha"}
              ],
            },
            {
              "time": "8:30am",
              "name": "First Impressions",
              "members": [
              {"name": "John"},
              {"name":"Jane"}
              ]
            }
          ]
        }]
    
  }  
})();

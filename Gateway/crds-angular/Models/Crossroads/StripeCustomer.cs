using System.Collections.Generic;

namespace crds_angular.Models.Crossroads
{

    public class Metadata
    {
    }

    public class Subscriptions
    {
        public string @object { get; set; }
        public int total_count { get; set; }
        public bool has_more { get; set; }
        public string url { get; set; }
        public List<object> data { get; set; }
    }

    public class Metadata2
    {
    }

    public class SourceData
    {
        public string id { get; set; }
        public string @object { get; set; }
        public string last4 { get; set; }
        public string brand { get; set; }
        public string funding { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string fingerprint { get; set; }
        public string country { get; set; }
        public string name { get; set; }
        public object address_line1 { get; set; }
        public object address_line2 { get; set; }
        public object address_city { get; set; }
        public object address_state { get; set; }
        public string address_zip { get; set; }
        public object address_country { get; set; }
        public string cvc_check { get; set; }
        public object address_line1_check { get; set; }
        public object address_zip_check { get; set; }
        public object dynamic_last4 { get; set; }
        public Metadata2 metadata { get; set; }
        public string customer { get; set; }
    }

    public class Sources
    {
        public string @object { get; set; }
        public int total_count { get; set; }
        public bool has_more { get; set; }
        public string url { get; set; }
        public List<SourceData> data { get; set; }
    }

    public class StripeCustomer
    {
        public string @object { get; set; }
        public int created { get; set; }
        public string id { get; set; }
        public bool livemode { get; set; }
        public string description { get; set; }
        public object email { get; set; }
        public bool delinquent { get; set; }
        public Metadata metadata { get; set; }
        public Subscriptions subscriptions { get; set; }
        public object discount { get; set; }
        public int account_balance { get; set; }
        public object currency { get; set; }
        public Sources sources { get; set; }
        public string default_source { get; set; }
    }
}
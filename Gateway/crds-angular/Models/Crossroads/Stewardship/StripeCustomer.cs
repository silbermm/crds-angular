using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Stewardship
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
        public string bank_last4 { get; set; }
        public string brand { get; set; }
        public string funding { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string fingerprint { get; set; }
        public string country { get; set; }
        public string name { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string address_city { get; set; }
        public string address_state { get; set; }
        public string address_zip { get; set; }
        public string address_country { get; set; }
        public string cvc_check { get; set; }
        public string address_line1_check { get; set; }
        public string address_zip_check { get; set; }
        public string dynamic_last4 { get; set; }
        public Metadata2 metadata { get; set; }
        public string customer { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string routing_number { get; set; }
        public string bank_name { get; set; }
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
        public string email { get; set; }
        public bool delinquent { get; set; }
        public Metadata metadata { get; set; }
        public Subscriptions subscriptions { get; set; }
        public string discount { get; set; }
        public int account_balance { get; set; }
        public string currency { get; set; }
        public Sources sources { get; set; }
        public string default_source { get; set; }
        public string last4 { get; set; }
        public string brand { get; set; }
    }
}
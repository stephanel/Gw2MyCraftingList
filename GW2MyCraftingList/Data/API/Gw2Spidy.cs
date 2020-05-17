using System;
using System.Runtime.Serialization;

namespace GW2ExplorerCraftTool.Data.API
{
    public class Gw2Spidy
    {
        [DataContract]
        public class ItemResult
        {
            [DataMember]
            public int data_id;
            [DataMember]
            public string img;
            [DataMember]
            public int gw2db_external_id;
            [DataMember]
            public string name;
            [DataMember]
            public int rarity;
            [DataMember]
            public int restriction_level;
            [DataMember]
            public int type_id;
            [DataMember]
            public int sub_type_id;
            [DataMember]
            public string price_last_changed;
            [DataMember]
            public string max_offer_unit_price;
            [DataMember]
            public string min_sale_unit_price;
            [DataMember]
            public string offer_availability;
            [DataMember]
            public string sale_availability;
            [DataMember]
            public string sale_price_change_last_hour;
            [DataMember]
            public string offer_price_change_last_hour;
        }

        [DataContract]
        public class Item
        {
            //{"result":{"data_id":36906,"name":"Apothecary's Destroyer Longbow","rarity":5,"restriction_level":80,
            //  "img":"https:\/\/dfach8bufmqqv.cloudfront.net\/gw2\/img\/content\/a9415f9.png","type_id":18,"sub_type_id":2,"price_last_changed":"2013-08-28 06:09:20 UTC",
            //  "max_offer_unit_price":161598,"min_sale_unit_price":349997,"offer_availability":8,"sale_availability":21,"gw2db_external_id":71284,"sale_price_change_last_hour":0,"offer_price_change_last_hour":-18}}
            //{"result":{"data_id":42403,"name":"Infinite Molten Berserker Tonic","rarity":5,"restriction_level":0,"img":"https:\/\/dfach8bufmqqv.cloudfront.net\/gw2\/img\/content\/40c6cbd4.png","type_id":7,"sub_type_id":0,"price_last_changed":"2013-05-30 06:47:20 UTC","max_offer_unit_price":407802,"min_sale_unit_price":595499,"offer_availability":39,"sale_availability":29,"gw2db_external_id":0,"sale_price_change_last_hour":0,"offer_price_change_last_hour":0}}
            [DataMember]
            public ItemResult result;
            public Exception exception;

        }
    }
}

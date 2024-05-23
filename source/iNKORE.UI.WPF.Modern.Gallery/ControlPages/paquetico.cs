using iNKORE.UI.WPF.Modern.Controls;
using System;

namespace iNKORE.UI.WPF.Modern.Gallery.Common
{

    public class paquetico
    {
        public paquetico (  
                            int _ID, 
                            string _amount, 
                            string _cellphone, 
                            string _provider, 
                            int _point_sale, 
                            int _sales_person
                        ){
            ID = _ID;
            amount = _amount;
            cellphone = _cellphone;
            provider = _provider;
            point_sale = _point_sale;
            sales_person = _sales_person;
        }

        public int ID { get; set; }
        public string amount { get; set; }
        public string cellphone {get; set;}
        public string provider {get; set;}
        public int point_sale {get;set;}
        public int sales_person{get;set;}
    }

}
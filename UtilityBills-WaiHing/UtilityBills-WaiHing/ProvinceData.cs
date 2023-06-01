using System;

namespace UtilityBills_WaiHing
{
    public class ProvinceData
    {
        public string Province { get; set; }
        public double SalesTax { get; set; }

        public ProvinceData(string province, double salesTax)
        {
            this.Province = province;
            this.SalesTax = salesTax;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UtilityBills_WaiHing
{
    public partial class MainPage : ContentPage
    {
        // Declare const for charges
        public const double dayChargePerUnit = 0.314, nightChargePerUnit = 0.186, rebateRate = 0.095;
        
        // Declare a List of provines for selection and data members for calculation
        private List<ProvinceData> provinceList = new List<ProvinceData>();        
        private string provinceValue;
        private double dayUsage, eveningUsage;
        private bool toggleValue;
        

        public MainPage()
        {
            InitializeComponent();

            // Populate the province list to the picker in main
            provinceList.Add(new ProvinceData("AB", 0.0));
            provinceList.Add(new ProvinceData("BC", 0.12));
            provinceList.Add(new ProvinceData("ON", 0.13));
            provinceList.Add(new ProvinceData("NL", 0.15));

            picker.ItemsSource = provinceList.Select((p) => p.Province).ToList(); // Provide only the Province data to the picker on UI
        }

        /* The Picker method triggered by selelcting province from main */
        private void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the province text from the picker
            provinceValue = picker.SelectedItem as string;

            // Default settings for toggle
            toggle.IsToggled = false;
            toggle.IsEnabled = true;

            if (provinceValue == "BC")
            {
                toggle.IsToggled = true;
                toggle.IsEnabled = false;
            }
            toggleValue = toggle.IsToggled;
        }

        /* The Calculation button that validates inputs and returns final amount for the receipt */
        private void Button_Clicked(object sender, EventArgs e)
        {
            // Variables for calculation
            double dayTimeCharge = 0;
            double eveningCharge = 0;
            double totalUsageCharge = 0;
            double provinceSalesTax = 0;
            double rebate = 0;
            double tax = 0;
            double totalAmt = 0;
            string dayTimeUsageFromUI = "";
            string eveningUsageFromUI = "";

            // Extract usage input values from UI
            dayTimeUsageFromUI = dayTime.Text;
            eveningUsageFromUI = nightTime.Text;
            toggleValue = toggle.IsToggled;

            // Reset error text
            error.Text = string.Empty;

            // Input validations
            if (string.IsNullOrEmpty(provinceValue))
            {
                error.Text += "Please choose a province.\n";
            }
            if (string.IsNullOrEmpty(dayTimeUsageFromUI))
            {
                error.Text += "Please enter the daytime usage.\n";                
            }
            else if (!double.TryParse(dayTimeUsageFromUI, out dayUsage))
            {
                error.Text += "Please enter a numeric daytime usage value.\n";
            }
            if (string.IsNullOrEmpty(eveningUsageFromUI))
            {
                error.Text += "Please enter the evening usage.\n";
            }
            else if (!double.TryParse(eveningUsageFromUI, out eveningUsage))
            {
                error.Text += "Please enter a numeric evening usage value.\n";
            }

            // If validation fails, error text will display; Otherwise, calculation will proceed
            if (!string.IsNullOrEmpty(error.Text))
            {
                error.IsVisible = true;
            }
            else 
            {
                // enable the breakdown section
                breakdownSection.IsVisible = true;
                rebateLabel.IsVisible = false;

                // Charges calculation
                dayTimeCharge = dayUsage * dayChargePerUnit;
                dayTimeCharge = Math.Round(dayTimeCharge, 2);
                dayChargeLabel.Text = $"Daytime usage charge: ${dayTimeCharge}";

                eveningCharge = eveningUsage * nightChargePerUnit;
                eveningCharge = Math.Round(eveningCharge, 2);
                eveningChargeLabel.Text = $"Evening usage charge: ${eveningCharge}";

                totalUsageCharge = dayTimeCharge + eveningCharge;
                totalUsageChargeLabel.Text = $"Total Charges: ${totalUsageCharge}";

                // Retrieve the sales tax based on the selected province
                provinceSalesTax = provinceList.FirstOrDefault(p => p.Province == provinceValue)?.SalesTax ?? 0.0;

                tax = provinceSalesTax * totalUsageCharge;
                tax = Math.Round(tax, 2);
                salesTaxLabel.Text = $"Sales Tax {provinceSalesTax * 100}%: ${tax}";

                if (toggleValue == true)
                {
                    rebateLabel.IsVisible = true;
                    rebate = totalUsageCharge * rebateRate;
                    rebate = Math.Round(rebate, 2);
                    rebateLabel.Text = $"Environmental rebate: -${rebate}";
                }

                totalAmt = totalUsageCharge + tax - rebate;
                totalLabel.Text = $"You Must Pay: ${totalAmt}";
            }
        }

        // Clear button that resets all values and UI components back to default
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            // Values reset
            provinceValue = string.Empty;
            dayUsage = 0;
            eveningUsage = 0;
            toggleValue = false;

            // UI components reset
            picker.SelectedItem = string.Empty;
            dayTime.Text = string.Empty;
            nightTime.Text = string.Empty;
            toggle.IsToggled = false;
            error.IsVisible = false;
            breakdownSection.IsVisible = false;
        }
    }
}

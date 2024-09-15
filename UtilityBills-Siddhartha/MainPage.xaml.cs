using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UtilityBills_Siddhartha
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void OnCalculateClicked(object sender, EventArgs e)
        {
            string province = provincePicker.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(province))
            {
                DisplayAlert("Error", "Please select a province.", "Ok");
                return;
            }

            double.TryParse(daytimeUsage.Text, out double DaytimeUsage);
            double.TryParse(eveningUsage.Text, out double EveningUsage);
            bool isRenewable = renewableSwitch.IsToggled;
            double totalCharge = CalculateCharges(province, DaytimeUsage, EveningUsage, isRenewable);
            resultLabel.Text = $"You must pay: ${totalCharge:F2}";

        }
        private void OnResetClicked(object sender, EventArgs e)
        {
            provincePicker.SelectedIndex = -1;
            daytimeUsage.Text = string.Empty;
            eveningUsage.Text = string.Empty;
            renewableSwitch.IsToggled = false;
            resultLabel.Text = "You must pay:";
        }
        private double CalculateCharges(string province, double daytimeUsage, double eveningUsage, bool isRenewable)
        {
            double tax = getTax(province);
            double Daytimecharges = daytimeUsage * 0.314;
            dayTimeCharges.Text = $"Daytime Usage Charge: ${ Daytimecharges:F2}";
            double Eveningcharges = eveningUsage * 0.186;
            eveningCharges.Text = $"Evening Usage Charge: ${Eveningcharges:F2}";

            double TotalCharges =  Daytimecharges + Eveningcharges;
            total.Text = $"Total Charges: ${TotalCharges:F2}";

            double SalesTax = TotalCharges * tax;
            salesTax.Text = $"Sales Tax: ${SalesTax:F2}";

            double totalWithTax = TotalCharges + SalesTax;
            if (isRenewable|| province == "BC")
            {
                double renewDeduct = TotalCharges * 0.095;
                rebate.Text = $"Environmental Rebate: -${renewDeduct:F2}";
                return (TotalCharges - renewDeduct) + SalesTax;
            }
            else
            {
                return totalWithTax;
            }

        }
        private double getTax(string province)
        {
            double rate=0;
            if(province == "ON")
            {
                rate = 0.13;
            }
            if(province == "AB")
            {
                rate = 1;
            }
            if (province == "NL")
            {
                rate = 0.15;
            }
            if(province == "BC")
            {
                rate = 0.12;
            }
            return rate;
        }
    }
}

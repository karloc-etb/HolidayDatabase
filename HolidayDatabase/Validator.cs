using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HolidayDatabase
{
    class Validator
    {
        // Validate Holiday Number textbox
        public bool ValidateHolidayNumber(TextBox holidayNumber)
        {
            bool output = true;
            int holidayNumberInt;

            Int32.TryParse(holidayNumber.Text, out holidayNumberInt);

            // If textbox is empty then display error message and return false
            if (holidayNumber.Text == "")
            {
                MessageBox.Show("Holiday number textbox cannot be empty!", "Empty textbox", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
                holidayNumber.Focus();
            }

            // If textbox input is not in range then display an error message and return false
            else if (holidayNumberInt < 200 || holidayNumberInt > 1000)
            {
                MessageBox.Show("Holiday number input is not in range (200-1000) or is not a number!", "Input not in range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
                holidayNumber.Focus();
            }
            return output;
        }

        // Validate Holiday Destination textbox
        public bool ValidateHolidayDestination(TextBox holidayDestination)
        {
            bool output = true;

            // If textbox is empty then display an error message, return false and focus on the texbox
            if (holidayDestination.Text == "")
            {
                MessageBox.Show("Holiday destination cannot be empty!", "Empty textbox", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
                holidayDestination.Focus();
            }

            // If input does not match regex then display error message, return false and focus on the texbox
            else if (!Regex.IsMatch(holidayDestination.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Holiday destination input must consist of only letters!", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
                holidayDestination.Focus();
            }
            return output;
        }

        // Validate holiday cost textbox
        public bool ValidateHolidayCost(TextBox holidayCost)
        {
            bool output = true;
            string holidayCostNoSymbol = holidayCost.Text.TrimStart('€');
            decimal holidayCostDecimal;
            
            Decimal.TryParse(holidayCostNoSymbol, out holidayCostDecimal);
            double holidayCostDouble = Convert.ToDouble(holidayCostDecimal);

            // If textbox is empty then display error message and return false
            if (holidayCost.Text == "")
            {
                MessageBox.Show("Holiday cost cannot be empty!", "Empty textbox!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }

            // If it not emtpy the check two wrong possible outcomes
            else if (holidayCost.Text != "")
            {
                // If input is not in range then display error message, clear the textbox and return false
                if (holidayCostDecimal < 0m || holidayCostDecimal > 5000m)
                {
                    MessageBox.Show("Holiday cost input cannot be less than 0 or greater than 5000!", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    holidayCost.Clear();
                    output = false;
                }

                // If input is not a number then display error message, clear the textbox and return false
                else if (Double.IsNaN(holidayCostDouble))
                {
                    MessageBox.Show("Holiday cost input must be decimal number (xx.xxx...)!", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                    holidayCost.Clear();
                }
            }
            return output;
        }

        // Check if holiday departure date input is in correct format
        public bool ValidateHolidayDepartureDate(TextBox holidayDepartureDate)
        {
            bool output = true;
            int year = DateTime.Now.Year;

            string holidayDepartureDateString;
            string holidayYearString, holidayMonthString, holidayDayString;
            int holidayYearInt, holidayMonthInt, holidayDayInt;

            if (!Regex.IsMatch(holidayDepartureDate.Text, @"^(\d{2})[\-\/](\d{2})[\-\/]\d+$"))
            {

                MessageBox.Show("Holiday departure date is not in correct format (dd-MM-yyyy) or date is invalid!", "Empty textbox!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
            }
            else if (holidayDepartureDate.Text != "")
            {
                holidayDepartureDateString = holidayDepartureDate.Text;

                holidayYearString = holidayDepartureDateString.Substring(6, 4);
                holidayYearInt = Convert.ToInt32(holidayYearString);

                holidayMonthString = holidayDepartureDateString.Substring(3, 2);
                holidayMonthInt = Convert.ToInt32(holidayMonthString);

                holidayDayString = holidayDepartureDateString.Substring(0, 2);
                holidayDayInt = Convert.ToInt32(holidayDayString);

                Int32.TryParse(holidayYearString, out holidayYearInt);
                Int32.TryParse(holidayMonthString, out holidayMonthInt);
                Int32.TryParse(holidayDayString, out holidayDayInt);

                if (holidayYearInt > year + 4)
                {
                    MessageBox.Show("Holiday departure date cannot be booked in advance of more than 4 years!", "Booking error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }
                else if (holidayYearInt < year)
                {
                    MessageBox.Show("Holiday departure date cannot be booked previous to today's date year!", "Booking error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }

                if (holidayMonthInt > 12)
                {
                    MessageBox.Show("Holiday departure date's month cannot be greater than 12!", "Booking error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }
                else if (holidayMonthInt < 1)
                {
                    MessageBox.Show("Holiday departure date day cannot be smaller than 1!", "Booking error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }

                if (holidayDayInt > 31)
                {
                    MessageBox.Show("Holiday departure date's day cannot be greater than 31!", "Booking error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }
                else if (holidayDayInt < 1)
                {
                    MessageBox.Show("Holiday departure date's day cannot be smaller than 1!", "Booking error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    output = false;
                }
            }
            return output;
        }

        // Check if number of days input is in correct format
        public bool ValidateHolidayNoOfDays(TextBox holidayNoOfDays)
        {
            bool output = true;
            string holidayNoOfDaysString = holidayNoOfDays.Text;
            int holidayNoOfDaysInt;

            if (holidayNoOfDays.Text == "")
            {
                MessageBox.Show("Holiday number of days cannot be empty!", "Empty textbox!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                output = false;
                holidayNoOfDays.Focus();
            }

            else if (holidayNoOfDays.Text != "")
            {
                Int32.TryParse(holidayNoOfDaysString, out holidayNoOfDaysInt);

                if (!Regex.IsMatch(holidayNoOfDaysString, @"^[0-9]+$"))
                {
                    MessageBox.Show("Holiday number of days input must be a number!", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    holidayNoOfDays.Clear();
                    holidayNoOfDays.Focus();
                    output = false;
                }
                else if (holidayNoOfDaysInt <= 0 || holidayNoOfDaysInt >= 31)
                {
                    MessageBox.Show("Holiday number of days cannot be less than 1 or greater than 30!", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    holidayNoOfDays.Clear();
                    holidayNoOfDays.Focus();
                    output = false;
                }
            }
            return output;
        }
    }
}

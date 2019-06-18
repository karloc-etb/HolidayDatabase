using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HolidayDatabase
{
    public partial class frmHoliday : Form
    {
        Holiday holiday = new Holiday();
        Holiday cancelHoliday = new Holiday();
        BindingSource bindingSource = new BindingSource();
        SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM tblHoliday", HolidayDbConnection.GetConnection());
        DataSet dataSet = new DataSet("tblHoliday");
        Validator validator = new Validator();

        public frmHoliday()
        {
            InitializeComponent();
        }

        // When form first starts Add button will be set to visible and Save button to invisible
        private void frmHoliday_Load(object sender, EventArgs e)
        {
            btnAdd.Visible = true;
            btnSave.Visible = false;

            ConnectTable();
            AddBindings();
            RecordCounter();
        }


        // Create a Holiday object based on the inputs
        private Holiday HolidayData(Holiday holiday)
        {
            holiday.HolidayNo = Convert.ToInt32(txtHolidayNumber.Text);
            holiday.Destination = Convert.ToString(txtDestination.Text);
            holiday.Cost = Convert.ToDecimal(txtCost.Text.TrimStart('€'));
            holiday.DepartureDate = Convert.ToDateTime(txtDepartureDate.Text);
            holiday.NoOfDays = Convert.ToInt32(txtNoOfDays.Text);
            holiday.Available = chkbAvailable.Checked;

            return holiday;
        }

        // Check if all the textboxes inputs are in correct format
        private bool CheckHolidayInput()
        {
            bool output = false;

            if (validator.ValidateHolidayNumber(txtHolidayNumber)
                && validator.ValidateHolidayDestination(txtDestination)
                && validator.ValidateHolidayCost(txtCost)
                && validator.ValidateHolidayDepartureDate(txtDepartureDate)
                && validator.ValidateHolidayNoOfDays(txtNoOfDays))
            {
                output = true;
            }

            return output;
        }

        // Update the current record displaying
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool checkHolidayInput = CheckHolidayInput();

            DisconnectTable();

            if (checkHolidayInput)
            {
                Holiday holidayData = this.HolidayData(holiday);
                HolidayDbCommand.UpdateHoliday(HolidayData(holiday));
            }

            ConnectTable();
            ClearBindings();
            AddBindings();
        }

        // When the user clicks Add, Add button will be set to invisible and Save to visible
        // Clear the textboxes and uncheck Available
        // And remove bindings from textboxes
        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Visible = false;
            btnSave.Visible = true;

            txtHolidayNumber.Clear();
            txtDestination.Clear();
            txtCost.Clear();
            txtDepartureDate.Clear();
            txtNoOfDays.Clear();
            chkbAvailable.Checked = false;

            ClearBindings();
        }


        
        // Check if inputs are in correct format
        // If yes the the record will be added to the table
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool checkHolidayInput = CheckHolidayInput();

            if (checkHolidayInput)
            {
                btnAdd.Visible = true;
                btnSave.Visible = false;

                Holiday holidayData = this.HolidayData(holiday);
                HolidayDbCommand.AddHoliday(holidayData);
                ReconnectTable();
                AddBindings();
                RecordCounter();
            }
            else
                ReconnectTable();
        }

        // Delete the record from the table
        private void btnDelete_Click(object sender, EventArgs e)
        {
            HolidayDbCommand.DeleteHoliday(holiday);

            ReconnectTable();
            ClearBindings();
            AddBindings();
            RecordCounter();
        }

        // Cancel the current action (adding, editing)
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSource.CancelEdit();
            if (btnSave.Visible)
            {
                DisconnectTable();
                this.frmHoliday_Load(sender, e);
            }
            ClearBindings();
            AddBindings();
        }

        // Print all the records to a textfile
        private void btnPrint_Click(object sender, EventArgs e)
        {
            HolidayDbCommand.PrintHoliday();
        }

        // Exit the program
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // When clicked it goes to to the first record in the table
        private void btnFirst_Click(object sender, EventArgs e)
        {
            // Cancel edit
            bindingSource.CancelEdit();

            // Goes to first record
            bindingSource.MoveFirst();

            ReconnectTable();
            ClearBindings();
            AddBindings();
            RecordCounter();
        }

        // Go to to the previous record in the table
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            bindingSource.CancelEdit();
            bindingSource.MovePrevious();

            ReconnectTable();
            ClearBindings();
            AddBindings();
            RecordCounter();
        }

        // Go to the next record in the table
        private void btnNext_Click(object sender, EventArgs e)
        {
            bindingSource.CancelEdit();
            bindingSource.MoveNext();

            ReconnectTable();
            ClearBindings();
            AddBindings();
            RecordCounter();
        }

        // Go to the last record in the table
        private void btnLast_Click(object sender, EventArgs e)
        {
            bindingSource.CancelEdit();
            bindingSource.MoveLast();

            ReconnectTable();
            ClearBindings();
            AddBindings();
            RecordCounter();
        }

        // Displays the position of currently displayed record of the table
        private void RecordCounter()
        {
            txtRecordCount.Text = Convert.ToString(bindingSource.Position + 1) + " of " + Convert.ToString(bindingSource.Count);
        }

        // Remove the table from dataset
        private void DisconnectTable()
        {
            dataSet.Tables.RemoveAt(0);
        }

        // Add the table to dataset, add dataset to data adapter, add dataset to bindingsource
        private void ConnectTable()
        {
            dataSet.Tables.Add("tblHoliday");
            dataAdapter.Fill(dataSet.Tables["tblHoliday"]);
            bindingSource.DataSource = dataSet.Tables[0];
        }

        // Combines the code from DisconnectTable() and ConnectTable() methods
        private void ReconnectTable()
        {
            dataSet.Tables.RemoveAt(0);
            dataSet.Tables.Add("tblHoliday");
            dataAdapter.Fill(dataSet.Tables["tblHoliday"]);
            bindingSource.DataSource = dataSet.Tables[0];
        }

        // Clear bindings from textboxes
        private void ClearBindings()
        {
            txtHolidayNumber.DataBindings.Clear();
            txtDestination.DataBindings.Clear();
            txtCost.DataBindings.Clear();
            txtDepartureDate.DataBindings.Clear();
            txtNoOfDays.DataBindings.Clear();
            chkbAvailable.DataBindings.Clear();
        }

        // Add bindings to textboxes
        private void AddBindings()
        {
            txtHolidayNumber.DataBindings.Add(new Binding("Text", bindingSource, "HolidayNo"));
            txtDestination.DataBindings.Add(new Binding("Text", bindingSource, "Destination"));
            
            //txtCost.DataBindings.Add(new Binding("Text", bindingSource, "Cost"));
            Binding cost = new Binding ("Text", bindingSource, "Cost");
            // Add the delegates to the event.
            cost.Format += new ConvertEventHandler(DecimalToCurrencyString);
            cost.Parse += new ConvertEventHandler(CurrencyStringToDecimal);
            txtCost.DataBindings.Add(cost);

            txtDepartureDate.DataBindings.Add(new Binding("Text", bindingSource, "DepartureDate", true, DataSourceUpdateMode.OnValidation, "", "dd-MM-yyyy"));
            txtNoOfDays.DataBindings.Add(new Binding("Text", bindingSource, "NoOfDays"));
            chkbAvailable.DataBindings.Add(new Binding("Checked", bindingSource, "Available"));
        }

        // Converts decimal to currency string with curency symbol
        private void DecimalToCurrencyString(object sender, ConvertEventArgs cevent)
        {
            if (cevent.DesiredType != typeof(string))
                return;

            cevent.Value = ((decimal)cevent.Value).ToString("c");
        }

        // Converts currency to decimal
        private void CurrencyStringToDecimal(object sender, ConvertEventArgs cevent)
        {
            
            if (cevent.DesiredType != typeof(decimal))
                return;

            
            cevent.Value = Decimal.Parse(cevent.Value.ToString(),
            NumberStyles.Currency, null);
        }

        // If user clicks away from Cost textbox then clear the bindings
        private void txtCost_Leave(object sender, EventArgs e)
        {
            ClearBindings();
            
        }

        private void txtDepartureDate_Leave(object sender, EventArgs e)
        {
            ClearBindings();
        }

        
    }
}

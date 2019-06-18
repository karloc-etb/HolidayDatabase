using Microsoft.VisualBasic;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace HolidayDatabase
{
    class HolidayDbCommand
    {
        public HolidayDbCommand()
        {
            
        }
        
        // This method is called when user presses Update button
        public static void UpdateHoliday(Holiday holiday)
        {

            // Get connection
            SqlConnection connection = HolidayDbConnection.GetConnection();

            // Query string
            string updateStatement = "UPDATE tblHoliday " +
                                     "SET Destination = @Destination, Cost = @Cost, DepartureDate = @DepartureDate, " +
                                     "NoOfDays = @NoOfDays, Available = @Available " +
                                     "WHERE HolidayNo = @HolidayNo";

            // Update command with query string and connection
            SqlCommand updateCommand = new SqlCommand(updateStatement, connection);

            // Add parameters
            updateCommand.Parameters.AddWithValue("@HolidayNo", holiday.HolidayNo);
            updateCommand.Parameters.AddWithValue("@Destination", holiday.Destination);
            updateCommand.Parameters.AddWithValue("@Cost", holiday.Cost);
            updateCommand.Parameters.AddWithValue("@DepartureDate", holiday.DepartureDate);
            updateCommand.Parameters.AddWithValue("@NoOfDays", holiday.NoOfDays);
            updateCommand.Parameters.AddWithValue("@Available", holiday.Available);

            try
            {
                connection.Open();

                int count = updateCommand.ExecuteNonQuery();

                if (count >= 1)
                    MessageBox.Show("Holiday number " + holiday.HolidayNo + " updated", "Holiday update");
                else
                    MessageBox.Show("Holiday update failed", "Holiday update");
            }
            catch (SqlException se)
            {
                MessageBox.Show(se.Message + "\n" + se.GetType().ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        // This method is called when user presses Print button
        public static void PrintHoliday()
        {
            SqlConnection connection = HolidayDbConnection.GetConnection();

            string path = @"PrintFile.txt";

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            else
            {
                StreamWriter sw = new StreamWriter(path);
                string query = "SELECT * FROM tblHoliday";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                string date = DateAndTime.Now.ToString("dd/MM/yyyy");
                int count = 0, pageCount = 1;


                sw.WriteLine("{0, 40}{1, 30}\n{2, 37}\n\n", "Downton Travel", "Page " + pageCount, date);
                sw.WriteLine(String.Format("{0, -15}{1, -15}{2, -15}{3, 10}{4, 15}\n",
                                           "HolidayNumber", "Destination", "DepartureDate", "Cost", "Available"));
                try
                {
                    while (reader.Read())
                    {
                        string status = reader["Available"].ToString();
                        ++count;

                        DateTime departureDate = (DateTime) reader["DepartureDate"];
                        string shortDepartureDate = departureDate.ToString("dd/MM/yyyy");

                        if (status == "True")
                            status = "Yes";
                        else if (status == "False")
                            status = "No";

                        

                        sw.WriteLine(String.Format("{0, -15}{1, -15}{2, -15}{3, 10:c}{4, 15}",
                        reader["HolidayNo"], reader["Destination"], shortDepartureDate, reader["Cost"], status));

                       
                    }
                }
                catch (SqlException se)
                {
                    MessageBox.Show(se.Message + "\n" + se.GetType().ToString());
                }
                finally
                {
                    Process.Start("notepad.exe", path);
                    connection.Close();
                    sw.Close();
                }
            }
        }

        // This method is called when user presses Add button
        public static void AddHoliday(Holiday holiday)
        {
            SqlConnection connection = HolidayDbConnection.GetConnection();

            string insertStatement =
                "INSERT tblHoliday (HolidayNo, Destination, Cost, DepartureDate, NoOfDays, Available) " +
                "VALUES (@HolidayNo, @Destination, @Cost, @DepartureDate, @NoOfDays, @Available)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);

            insertCommand.Parameters.AddWithValue("@HolidayNo", holiday.HolidayNo);
            insertCommand.Parameters.AddWithValue("@Destination", holiday.Destination);
            insertCommand.Parameters.AddWithValue("@Cost", holiday.Cost);
            insertCommand.Parameters.AddWithValue("@DepartureDate", holiday.DepartureDate);
            insertCommand.Parameters.AddWithValue("@NoOfDays", holiday.NoOfDays);
            insertCommand.Parameters.AddWithValue("@Available", holiday.Available);

            //open close
            try
            {
                connection.Open();

                int count = insertCommand.ExecuteNonQuery();

                if (count == 1)
                    MessageBox.Show("Holiday added!", "Add Holiday", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Holiday add failed!", "Add Holiday", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException se)
            {
                MessageBox.Show(se.Message + "\n" + se.GetType().ToString(), "Add exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        // This method is called when user presses Delete button
        public static void DeleteHoliday(Holiday holiday)
        {
            string holidayNo = "";
            DialogResult result = new DialogResult();

            holidayNo = Interaction.InputBox("Enter the number of the holiday that you want to delete:", "Holiday deletion", "1", 830, 460);

            if(holidayNo != "")
                result = MessageBox.Show("Are you sure to delete HolidayNo: " + holidayNo + " ?","Holiday deletetion", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                SqlConnection connection = HolidayDbConnection.GetConnection();

                string deleteStatement = "DELETE FROM tblHoliday WHERE HolidayNo = @HolidayNo";

                SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);

                deleteCommand.Parameters.AddWithValue("@HolidayNo", Convert.ToInt32(holidayNo));


                try
                {
                    connection.Open();

                    int count = deleteCommand.ExecuteNonQuery();

                    if (count >= 1)
                        MessageBox.Show("Holiday has been deleted", "Holiday deletion");
                    else
                        MessageBox.Show("Holiday deletion failed", "Holiday deletion");
                }
                catch (SqlException se)
                {
                    MessageBox.Show(se.Message + "\n" + se.GetType().ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}

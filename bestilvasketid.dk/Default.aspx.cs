using bestilvasketid.dk.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bestilvasketid.dk
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //InfoBox.Text = "Velkommen " + HttpContext.Current.User.Identity.Name;
            DateTime date = DateTime.Now;

            if (HttpContext.Current.User.Identity.IsAuthenticated) AddPanels();
            else Response.Redirect("/Account/Login.aspx");

            // ScriptManager.RegisterStartupScript(this, typeof(Page), "GetResokution","$(document).ready(function(){EnableControls();", true);
        }

        public void AddPanels()
        {
            DateTime date = DateTime.Now;
            Panel centerdiv = new Panel();
            centerdiv.ControlStyle.CssClass = "centerdiv";
            ContentPlaceHolder cph = (ContentPlaceHolder)Master.FindControl("MainContent");
            cph.Controls.Add(centerdiv);

            for (int i = 0; i < 6; i++)
            {
                Panel DailyPanel = new Panel();
                DailyPanel.ID = "DailyPanel" + i.ToString();
                DailyPanel.ControlStyle.CssClass = "daily-view";

                centerdiv.Controls.Add(DailyPanel);

                DailyPanel.Controls.Add(AddDateButton(date.AddDays(i)));

                Panel MachinesPanel = new Panel();
                MachinesPanel.ControlStyle.CssClass = "machines-view";
                DailyPanel.Controls.Add(MachinesPanel);

                for (int m = 0; m < AdminSettings.amountmachines; m++)
                {
                    Panel MachinePanel = new Panel();
                    MachinePanel.ID = "MachinePanel" + m.ToString() + "/" + i;
                    MachinePanel.ControlStyle.CssClass = "machine";

                    MachinePanel.Controls.Add(AddMachineButton(m, i));

                    Panel TimetablePanel = new Panel();
                    TimetablePanel.ControlStyle.CssClass = "machine-timetable";
                    MachinePanel.Controls.Add(TimetablePanel);

                    List<Button> ButtonList = AddButtons(date.AddDays(i), m);

                    foreach (var button in ButtonList) TimetablePanel.Controls.Add(button);
                    MachinesPanel.Controls.Add(MachinePanel);
                }
            }
        }

        public Button AddDateButton(DateTime date)
        {
            //CREATES THE DATE BUTTON ON TOP
            Button DateButton = new Button();
            DateButton.ID = "DayButton " + date.ToString("dd / MM - yyyy");
            DateButton.Text = date.DayOfWeek + " " + date.ToString("dd/MM/yyyy");
            DateButton.ControlStyle.CssClass = "booking-button button-date weekday";

            return DateButton;
        }
        public Button AddMachineButton(int machine, int schedule)
        {
            //CREATES THE DATE BUTTON ON TOP
            Button MachineButton = new Button();
            MachineButton.ID = "machine " + machine + "/" + schedule;
            MachineButton.Text = "Maskine #" + (machine + 1);
            MachineButton.ControlStyle.CssClass = "booking-button button-date machine-title";
            //MachineButton.ControlStyle.Width = 150 * AdminSettings.amountmachines;

            return MachineButton;
        }
        public List<Button> AddButtons(DateTime date, int m)
        {
            List<Button> Buttonlist = new List<Button>();

            //FILLS DATATABLE WITH SCHEDULES FROM SQL
            string fromDate = date.ToString("yyyy-MM-dd");
            DataTable DT = new SQLClass().ReadSQLDatatable($"SELECT * from [dbo].[SCHEDULE] where CONVERT(date, [datetime]) = '{fromDate}' and [machine_FK] = {m}");

            //CREATES THE SCHEDULE BUTTONS
            float amountOfSchedulesPerDay = (AdminSettings.closingTime - AdminSettings.openingTime) * 60 / AdminSettings.scheduleTimeInMinutes;
            for (int i = 0; i < amountOfSchedulesPerDay; i++)
            {
                //ADDS HOURS AND MINUTES TO THE TIME THE BOOKING BUTTON SHOWS
                DateTime showtime = date.Date.AddHours(AdminSettings.openingTime).AddMinutes(i * AdminSettings.scheduleTimeInMinutes);
                Button BookingButton = new Button();
                BookingButton.ID = m + "#" + date.ToString("dd/MM/yyyy ") + showtime.ToShortTimeString();
                BookingButton.Text = showtime.ToShortTimeString() + " - " + showtime.AddMinutes(AdminSettings.scheduleTimeInMinutes).ToShortTimeString();
                BookingButton.ControlStyle.CssClass = "booking-button button-free time";
                //BookingButton.ControlStyle.CssClass = "button large regular green";
                //BookingButton.OnClientClick += new EventHandler(ButtonClicked); 
                BookingButton.Click += new EventHandler(ButtonClicked);
                foreach (DataRow row in DT.Rows)
                {
                    if (row["dateTime"].ToString().Contains(showtime.ToString()))
                    {
                        if (row["status"].ToString() == "1")
                        {
                            //IF SCHEDULE IS FOUND IN SQL SET BUTTON AS BOOKED DEPENDING ON USERINFOSETTING A NAME OR ADDRESS MIGHT BE ADDED
                            BookingButton.ControlStyle.CssClass = "booking-button button-booked time";
                            // BookingButton.ControlStyle.CssClass = "button large regular red";
                            switch (AdminSettings.showUserInfo)
                            {
                                case false:
                                    BookingButton.Text += " Optaget";
                                    break;
                                case true:
                                    string SQL = $"select [address] from dbo.[USER] where [email] = '{row["username_FK"]}'";
                                    BookingButton.Text += " " + new SQLClass().ReadSQL(SQL)[0];
                                    break;
                                //case 2:
                                //    BookingButton.Text += row[];
                                //    break;
                                default:
                                    BookingButton.Text += " Optaget";
                                    break;
                            }
                        }
                        else if (row["status"].ToString() == "2")
                        {
                            BookingButton.Text = " Maskinen er i stykker";
                        }
                        else if (row["status"].ToString() == "3")
                        {
                            BookingButton.Text = " Tekniker er tilkaldt";
                        }
                    }
                }

                if (DateTime.Now > showtime.AddMinutes(AdminSettings.scheduleTimeInMinutes))
                {
                    BookingButton.ControlStyle.CssClass = "booking-button button-past time";
                    //BookingButton.Click += new EventHandler(ButtonClicked);
                    //BookingButton.OnClientClick += new EventHandler(ButtonClicked);
                }

                //ADDS THE CREATED BUTTON TO THE LIST
                Buttonlist.Add(BookingButton);

            }
            return Buttonlist;
        }

        public void ButtonClicked(object sender, EventArgs e)
        {
            string UserId = System.Web.HttpContext.Current.User.Identity.Name;
            string[] split = (sender as Button).ID.Split('#');


            DateTime datetime = DateTime.Parse(split[1]);
            string SQLDateTime = datetime.ToString("yyyy-MM-dd HH:mm:00").Replace(".", ":");

            //CHECKS TO SEE IF SCHEDULE HAVE BEEN BOOKED ALREADY
            SQLClass SQL = new SQLClass();
            string Query = $"select [datetime] from dbo.SCHEDULE where [datetime] = '{SQLDateTime}' and [machine_FK] = '{split[0]}'"; //SQLDateTime
            List<string> ListSQL = SQL.ReadSQL(Query);
            if (ListSQL.Count > 0) { return; }//Already have a schedule  

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("datetime", typeof(DateTime));
            dataTable.Columns.Add("timeadded", typeof(DateTime));
            dataTable.Columns.Add("status", typeof(int));
            dataTable.Columns.Add("showID", typeof(int));
            dataTable.Columns.Add("username_fk", typeof(string));
            dataTable.Columns.Add("machine_fk", typeof(int));

            //                 DateTimeSQL / Status / showUser / User (email) / Machine
            dataTable.Rows.Add(datetime, DateTime.Now, 1, AdminSettings.showUserInfo, UserId, split[0]);//datetime SQL

            //INSERTS THE SCHEDULE INTO SQL
            SQL.WriteSQLDatatable(@"insert into [dbo].[SCHEDULE] ([datetime], [timeadded], [status], [showID], [username_FK], [machine_FK]) 
SELECT dataTable.datetime, dataTable.timeadded, dataTable.status, dataTable.showID, dataTable.username_FK, dataTable.machine_FK 
FROM @DataTable dataTable", dataTable, "[dbo].[SCHEDULEImport]");

            //SENDS EMAIL
            new MailClass().SendEmail(UserId, datetime);
            Response.Redirect("~/");
            //Response.Redirect(Request.RawUrl);
        }


    }
}
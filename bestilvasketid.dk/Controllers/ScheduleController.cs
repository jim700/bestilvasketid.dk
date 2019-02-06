using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using bestilvasketid.dk.Controllers;

namespace bestilvasketid.dk.Controllers
{
    public class ScheduleController : Controller
    {
/*
        public List<Button> AddButtons(DateTime date)
        {
            List<Button> Buttonlist = new List<Button>();

            //CREATES THE DATE BUTTON ON TOP
            Button DateButton = new Button();
            DateButton.ID = "DayButton " + date.ToString("dd / MM - yyyy");
            DateButton.Text = date.DayOfWeek + date.ToString("dd / MM - yyyy");
            DateButton.ControlStyle.CssClass = "booking-button button-date";
            Buttonlist.Add(DateButton);

            //FILLS DATATABLE WITH SCHEDULES FROM SQL
            string fromDate = date.ToString("yyyy-MM-dd");
            DataTable DT = new SQLClass().ReadSQLDatatable($"SELECT * from [dbo].[SCHEDULE] where CONVERT(date, [datetime]) = '{fromDate}'");

            //CREATES THE SCHEDULE BUTTONS
            float amountOfSchedulesPerDay = (AdminSettings.closingTime - AdminSettings.openingTime) * 60 / AdminSettings.scheduleTimeInMinutes;
            for (int i = 0; i < amountOfSchedulesPerDay; i++)
            {
                //ADDS HOURS AND MINUTES TO THE TIME THE BOOKING BUTTON SHOWS
                DateTime showtime = date.Date.AddHours(AdminSettings.openingTime).AddMinutes(i * AdminSettings.scheduleTimeInMinutes);
                Button BookingButton = new Button();
                BookingButton.ID = date.ToString("dd/MM/yyyy ") + showtime.ToShortTimeString();
                BookingButton.Text = showtime.ToShortTimeString() + " - " + showtime.AddMinutes(AdminSettings.scheduleTimeInMinutes).ToShortTimeString();
                BookingButton.ControlStyle.CssClass = "booking-button button-free";
                //BookingButton.OnClientClick += new EventHandler(ButtonClicked); 
                BookingButton.Click += new EventHandler(ButtonClicked);

                foreach (DataRow row in DT.Rows)
                {
                    if (row["dateTime"].ToString().Contains(showtime.ToString()))
                    {

                        if (row["status"].ToString() == "1")
                        {
                            //IF SCHEDULE IS FOUND IN SQL SET BUTTON AS BOOKED DEPENDING ON USERINFOSETTING A NAME OR ADDRESS MIGHT BE ADDED
                            BookingButton.ControlStyle.CssClass = "booking-button button-booked";
                            switch (AdminSettings.showUserInfo)
                            {
                                case 0:
                                    BookingButton.Text += " Optaget";
                                    break;
                                case 1:
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
                    BookingButton.ControlStyle.CssClass = "booking-button button-past";
                    //BookingButton.Click += new EventHandler(ButtonClicked);
                    //BookingButton.OnClientClick += new EventHandler(ButtonClicked);
                }

                //ADDS THE CREATED BUTTON TO THE LIST
                Buttonlist.Add(BookingButton);
            }
            return Buttonlist;
        }

    */
        public void ButtonClicked(object sender, EventArgs e)
        {
            string UserId = System.Web.HttpContext.Current.User.Identity.Name;
            DateTime datetime = DateTime.Parse((sender as Button).ID);
            string SQLDateTime = datetime.ToString("yyyy-MM-dd HH:mm:00").Replace(".",":");

            //CHECKS TO SEE IF SCHEDULE HAVE BEEN BOOKED ALREADY
            SQLClass SQL = new SQLClass();
            string Query = $"select [datetime] from dbo.SCHEDULE where [datetime] = '{SQLDateTime}'"; //SQLDateTime
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
            dataTable.Rows.Add(datetime, DateTime.Now, 1, AdminSettings.showUserInfo, UserId, 1);//datetime SQL

            //INSERTS THE SCHEDULE INTO SQL
            SQL.WriteSQLDatatable(@"insert into [dbo].[SCHEDULE] ([datetime], [timeadded], [status], [showID], [username_FK], [machine_FK]) 
SELECT dataTable.datetime, dataTable.timeadded, dataTable.status, dataTable.showID, dataTable.username_FK, dataTable.machine_FK 
FROM @DataTable dataTable", dataTable, "[dbo].[SCHEDULEImport]");

            //SENDS EMAIL
            new MailClass().SendEmail(UserId, datetime);
            Response.Redirect("~/");
            //Response.Redirect("Default.aspx");

        }

        // GET: Schedule
        public ActionResult Index()
        {
            return View();
        }
    }
}

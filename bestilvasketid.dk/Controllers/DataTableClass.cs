using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace bestilvasketid.dk.Controllers
{
    public class DataTableClass
    {

        public DataTable objToDataTable(Models.UserModel obj)
        {
            DataTable dt = new DataTable();
            Models.UserModel objmkt = new Models.UserModel();
            //dt.Columns.Add("Column_Name");
            foreach (PropertyInfo info in typeof(Models.UserModel).GetProperties())
            {
                dt.Columns.Add(info.Name);
            }


            dt.AcceptChanges();
            return dt;
        }


        public static DataTable ObjectToData(object o)
        {
            DataTable dt = new DataTable("OutputData");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            o.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(o, null);
                    dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(o, null);
                }
                catch { }
            });
            return dt;
        }

        //        create
        //        DataTable dataTable = new DataTable();
        //        dataTable.Columns.Add("datetime", typeof(DateTime));
        //            dataTable.Columns.Add("timeadded", typeof(DateTime));
        //            dataTable.Columns.Add("status", typeof(int));
        //            dataTable.Columns.Add("showID", typeof(int));
        //            dataTable.Columns.Add("username_fk", typeof(string));
        //            dataTable.Columns.Add("machine_fk", typeof(int));

        //                             DateTimeSQL / Status / showUser / User(email) / Machine
        //           dataTable.Rows.Add(datetime, DateTime.Now, 1, AdminSettings.showUserInfo, UserId, 1);//datetime SQL

        //        INSERTS THE SCHEDULE INTO SQL
        //        SQL.WriteSQLDatatable(@"insert into [dbo].[SCHEDULE] ([datetime], [timeadded], [status], [showID], [username_FK], [machine_FK]) 
        //SELECT dataTable.datetime, dataTable.timeadded, dataTable.status, dataTable.showID, dataTable.username_FK, dataTable.machine_FK 
        //FROM @DataTable dataTable", dataTable, "[dbo].[SCHEDULEImport]");

    }
}
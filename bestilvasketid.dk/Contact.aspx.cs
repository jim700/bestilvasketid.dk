using bestilvasketid.dk.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bestilvasketid.dk
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OpenTimeLabel.Text += AdminSettings.openingTime.ToString();

        }

        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (OpenTime.Text !="") AdminSettings.openingTime = int.Parse(OpenTime.Text);
            if (CloseTime.Text != "") AdminSettings.closingTime = int.Parse(CloseTime.Text);
            if (MaxScheduleTime.Text != "") AdminSettings.scheduleTimeInMinutes = int.Parse(MaxScheduleTime.Text);
            if (Machines.Text != "") AdminSettings.amountmachines = int.Parse(Machines.Text);
            AdminSettings.showUserInfo = IdentityCheckBox.Checked;
        }


    }
}

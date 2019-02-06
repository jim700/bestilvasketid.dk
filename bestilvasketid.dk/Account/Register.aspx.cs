using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using bestilvasketid.dk.Models;
using System.Data;

namespace bestilvasketid.dk.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                UserModel userModel = new UserModel() { Email = Email.Text, Address = Address.Text, Created = DateTime.Now };

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("email", typeof(string));
                dataTable.Columns.Add("address", typeof(string));
                dataTable.Columns.Add("created", typeof(DateTime));
                dataTable.Columns.Add("lastlogin", typeof(DateTime));
                dataTable.Columns.Add("deleted", typeof(DateTime));


                //                 DateTimeSQL / Status / showUser / User (email) / Machine
                dataTable.Rows.Add(Email.Text, Address.Text, DateTime.Now, null, null);//datetime SQL



                // System.Data.DataTable UserDT = Controllers.DataTableClass.ObjectToData(userModel);
                new Controllers.SQLClass().WriteSQLDatatable(@"INSERT INTO [dbo].[USER] ([email], [address], [created]) 
                        SELECT dataTable.email, dataTable.address, dataTable.created 
                        FROM @DataTable dataTable", dataTable, "[dbo].[USERImport]");

                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}
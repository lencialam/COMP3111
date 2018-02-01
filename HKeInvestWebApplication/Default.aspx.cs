using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientUI.Visible = false;
            EmployeeUI.Visible = false;
            AnyoneSearchDiv.Visible = true;
            ClientSearchDiv.Visible = false;
            if (Context.User.IsInRole("Client"))
            {
                ClientUI.Visible = true;
                AnyoneSearchDiv.Visible = false;
                ClientSearchDiv.Visible = true;
            }
            else if (Context.User.IsInRole("Employee"))
            {
                EmployeeUI.Visible = true;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HKeInvestCode myCode = new HKeInvestCode();
            myCode.sendEmail("lencialam@gmail.com", "hello", "body");
        }
    }
}
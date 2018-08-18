using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Devart.Data.PostgreSql;
using ShareTrading.Common.Src;

namespace ShareTrading
{
  public partial class FrmStatus : Form
  {
    public FrmStatus()
    {
      InitializeComponent();
      getStatus();
    }
    public void getStatus()
    {
      List<statusLine> displayLines = new List<statusLine>();
      // get system var status
      List<DBAccess.SystemVars> varList = new List<DBAccess.SystemVars>();
      string extraWhere = string.Empty;
      string orderBy = string.Empty;
      List<PgSqlParameter> paramList = new List<PgSqlParameter>();
      paramList = new List<PgSqlParameter>();
      paramList.Add(new PgSqlParameter("@P1", "NEXT_RUN"));
      extraWhere += " AND sv_desc = @P1 ";
      if (DBAccess.GetSystemVarRecords(paramList, out varList, DBAccess.SystemVarsList, extraWhere, orderBy))
      {
        statusLine line = new statusLine();
        line.text = varList[0].Status;
        displayLines.Add(line);
      }

      // get last update date for dividends, price history, brokers recommendations, company details
      // display
      dgvStatus.DataSource = displayLines;
    }
    public class statusGrid
    {
      public List<statusLine> lines { get; set; }
    }
    public class statusLine
    {
      public string text { get; set; }
      public string status { get; set; }
      public DateTime lastUpdate { get; set; }
    }
  }
}

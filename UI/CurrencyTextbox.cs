using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareTrading.UI
{
  public partial class CurrencyTextbox : System.Windows.Forms.TextBox
  {
    private string _workingText;
    public string WorkingText
    {
      get { return _workingText; }
      set { _workingText = value; }
    }

    private char _thousandsSeparator = ',';
    private string _preFix = "$";
    private char _decimalsSeparator = '.';
    private int _decimalPlaces = 2;


    protected override void OnLostFocus(EventArgs e)
    {
      this.Text = formatText();
      base.OnLostFocus(e);
    }

    protected override void OnGotFocus(EventArgs e)
    {
      this.Text = this.WorkingText;
      base.OnGotFocus(e);
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
      if (!Char.IsDigit(e.KeyChar))
      {
        if (!(e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == _decimalsSeparator))
          e.Handled = true;
      }
      base.OnKeyPress(e);
    }

    private string formatText()
    {

      this.WorkingText = this.Text.Replace(_preFix, "").Replace(_thousandsSeparator.ToString(), "");
      decimal val = 0M;
      if (Decimal.TryParse(this.WorkingText, out val))
        return string.Format("{0}", val.ToString("C2"));

      // couldn't parse to a decimal so leave as is
      return this.WorkingText;
      
    }
  }
}

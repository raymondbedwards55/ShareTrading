using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareTrading
{
  public class ImportPriceHistory
  {
    public static void ImportPriceFile(string _filename)
    {
      using (System.IO.StreamReader sr = new System.IO.StreamReader(System.IO.File.Open(_filename, System.IO.FileMode.Open)))
      {
        // No header line in the file
        string inputLine = string.Empty;  // sr.ReadLine();
        //if (string.IsNullOrEmpty(inputLine))
        //  return;
        //// header line
        //string[] hdr = inputLine.Split(',');
        while ((inputLine = sr.ReadLine()) != null)
        {
          if (String.IsNullOrEmpty(inputLine) || inputLine.Length < 2)
            continue;
          string[] flds = inputLine.Split(',');
          if (flds.Length > 7)     // in case there are commas within any of the field values
            Console.WriteLine();

          DBAccess.ASXPriceDate rec = DBAccess.GetSpecificASXPriceRecord(flds[0], DateTime.ParseExact(flds[1], "yyyyMMdd", null));
          if (rec == null)
          {
            rec = new DBAccess.ASXPriceDate();
            rec.ASXCode = flds[0];
            rec.PrcClose = decimal.Parse(flds[5]);
            rec.PrcOpen = decimal.Parse(flds[2]);
            rec.PrcHigh = decimal.Parse(flds[3]);
            rec.PrcLow = decimal.Parse(flds[4]);
            rec.PriceDate = DateTime.ParseExact(flds[1], "yyyyMMdd", null);
            rec.Volume = int.Parse(flds[6]);
            DBAccess.ASXprcInsert(rec);
          }
          else
          {
            rec.PrcClose = decimal.Parse(flds[5]);
            rec.PrcOpen = decimal.Parse(flds[2]);
            rec.PrcHigh = decimal.Parse(flds[3]);
            rec.PrcLow = decimal.Parse(flds[4]);
            rec.Volume = int.Parse(flds[6]);
            DBAccess.ASXprcUpdate(rec);
          }

        }
      }
    } 

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S_61.MyControl;
using System.Xml.Linq;
using System.Windows.Forms;

namespace S_61.Model
{
    class SetLanguage
    {
        static string Path;
        public static List<Control> list = new List<Control>();

        public static void CastControl(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is lblT) list.Add(c as lblT);
                if (c is btnBrowT) list.Add(c as btnBrowT);
                if (c.HasChildren) CastControl(c);
            }
        }

        public static void CreateXml(Form Frm, List<Control> CC)
        {
            XElement element = new XElement("zh-CN");
            CC.ForEach(c => element.Add(new XElement(c.Name, c.Text)));
            string s = Frm.Name;
            s = s.Substring(3);
            element.Save(@"c:\\" + s + ".xml");
        }

        public static void Change(List<Control> CL, object Country,string Frm)
        {
            Path = Application.StartupPath;
            Path = Path.Remove(Path.LastIndexOf(@"\"));
            Path = Path.Remove(Path.LastIndexOf(@"\"));
            switch (Country.ToString())
            {
                case "1":
                    Path += @"\Language\CN\" + Frm + ".xml";
                    break;
            }
            try
            {
                XElement element = XElement.Load(Path);
                CL.ForEach(cl => cl.Text = element.Element(cl.Name).Value.ToString());
            }
            catch { }
        }

    }
}

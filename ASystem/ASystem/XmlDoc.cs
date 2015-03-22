using System;
using System.Text;
using System.IO;
using System.Xml;

namespace ASystem
{
    class XmlDoc 
    {
        static XmlDocument _document;

        public XmlDoc()
        {
            _document = new XmlDocument();
        }

        public void CreateXML(string pathToXml)
        {
            try
            {
                if (!File.Exists(pathToXml))
                {
                    XmlTextWriter textWritter = new XmlTextWriter(pathToXml, Encoding.UTF8);

                    textWritter.WriteStartDocument();

                    textWritter.WriteStartElement("head");

                    textWritter.WriteEndElement();

                    textWritter.Close();
                }

                XmlDocument document = new XmlDocument();

                document.Load(pathToXml);
                XmlNode element = document.CreateElement("element");
                document.DocumentElement.AppendChild(element);
                XmlAttribute attribute = document.CreateAttribute("number");
                attribute.Value = "1";
                element.Attributes.Append(attribute);

                XmlNode subElement1 = document.CreateElement("subElement1");
                subElement1.InnerText = "Hello";
                element.AppendChild(subElement1);

                XmlNode subElement2 = document.CreateElement("subElement2");
                subElement2.InnerText = "Dear";
                element.AppendChild(subElement2);

                XmlNode subElement3 = document.CreateElement("subElement3");
                subElement3.InnerText = "Habr";
                element.AppendChild(subElement3);

                document.Save(pathToXml);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public string ReadXmlFile(DateTime date, int numberEvent)
        {
            string line = "";

            _document = new XmlDocument();
           
            string pathToXml = @"Data/" + date.Year + "/" + date.Month + "/" + date.Day + ".xml";

            _document.Load(pathToXml);
 
            foreach (XmlElement element in _document.GetElementsByTagName("event"))
            {
                if (Convert.ToInt32(element.Attributes["number"].Value) == numberEvent)
                {
                    foreach (XmlElement el in element)
                    {
                        if (el.Name == "timeStart") { line += el.InnerText + " - "; }
                        if (el.Name == "eventText") { line += el.InnerText + ", "; }
                        if (el.Name == "eventPlace") { line += el.InnerText + "\n"; }
                        if (el.Name == "priority") { line += "Приоритет: " + el.InnerText + "/3"; }
                    }
                }
            }

            return line;
        }

        public static int GetAmountEvents()
        {
            int amountEvents = 0;
            foreach (XmlElement element in _document.GetElementsByTagName("event"))
            {
                amountEvents++;
            }
            return amountEvents;
        }



        public static void MarksActiveDays(DateTime date)
        {
            _document = new XmlDocument();

            if (date.Month >= DateTime.Now.Month && date.Year >= DateTime.Now.Year)
            {
                int maxPriority = 0;
                for (int day = 1; day <= DateTime.DaysInMonth(date.Year, date.Month); day++)
                {
                    string pathToXml = @"Data/" + date.Year + "/" + date.Month + "/" + day + ".xml";

                    _document.Load(pathToXml);

                    foreach (XmlElement element in _document.GetElementsByTagName("event"))
                    {
                        for (int numberEvent = 1; numberEvent <= GetAmountEvents(); numberEvent++)
                        {
                            if(date.Day > day)
                            {
                                MainWindow.GetButtonDayActive(MainWindow.SearchButton(Convert.ToString(day)), 0);
                            }
                            else if (Convert.ToInt32(element.Attributes["number"].Value) == numberEvent)
                            {
                                foreach (XmlElement el in element)
                                {
                                    int priority = 0;
                                    if (el.Name == "priority")
                                    {
                                        priority = Convert.ToInt32(el.InnerText);
                                        if (priority > maxPriority) { maxPriority = priority; }
                                    }

                                    MainWindow.GetButtonDayActive(MainWindow.SearchButton(Convert.ToString(day)), priority);

                                }
                            }
                        }
                    }
                }
            }

            else
            {
                for (int day = 1; day <= DateTime.DaysInMonth(date.Year, date.Month); day++)
                {
                    string pathToXml = @"Data/" + date.Year + "/" + date.Month + "/" + day + ".xml";

                    _document.Load(pathToXml);

                    foreach (XmlElement element in _document.GetElementsByTagName("event"))
                    {
                        MainWindow.GetButtonDayActive(MainWindow.SearchButton(Convert.ToString(day)), 0);
                    }
                }
            }
        }
    }
}

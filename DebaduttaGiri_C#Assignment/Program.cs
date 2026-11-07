using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace DebaduttaGiri_C_Assignment
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                DataAccess access = new DataAccess();
                Response emps = await access.GetEmployees();



                List<Employees> employeeList = new List<Employees>();
                foreach (var emp in emps.Employees)
                {

                    var start = DateTime.Parse(emp.StarTimeUtc);
                    var end = DateTime.Parse(emp.EndTimeUtc);

                    var TDuration = end - start;

                    string EmpName = string.IsNullOrWhiteSpace(emp.EmployeeName) ? "Unknown" : emp.EmployeeName;


                    var empExist = employeeList.FirstOrDefault(x => x.EmployeeName == EmpName);
                    //Console.WriteLine(TDuration);
                    if (empExist != null) { empExist.TotalTime += TDuration; }
                    else
                    {
                        employeeList.Add(new Employees
                        {
                            EmployeeName = EmpName,
                            TotalTime = TDuration
                        });
                    }


                }




                var sortedEmployees = employeeList.OrderByDescending(e => e.TotalTime).ToList();
                //bool hrs = sortedEmployees.Any(emp => emp.TotalTime.TotalHours > 100);

                StringBuilder html = new StringBuilder();

                html.Append("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n   \r\n    <title>Employee Print</title>\r\n</head>\r\n<style>\r\n    body{\r\n      background-color: #fdfdfd;;\r\n    }\r\n    table{\r\n        \r\n        width: 100%;\r\n        border: 1px solid black;\r\n         border-collapse: collapse;\r\n    }\r\n    td,th{\r\n        border: 1px solid black;\r\n       text-align: center;\r\n        \r\n    }\r\n    th{\r\n        background-color: rgb(164, 76, 12);\r\n        color: #fdfdfd;\r\n    }\r\n    \r\n    \r\n</style>\r\n<body>\r\n    <table>\r\n        <tr>\r\n            <th>Employee Name</td>\r\n            <th>Total Time Worked</td>\r\n        </tr>");
                foreach (var emp in sortedEmployees)
                {
                    if (emp.TotalTime.TotalHours < 100)
                    {
                        html.Append($"<tr style='background-color:lightblue;'><td>{emp.EmployeeName} </td><td>{emp.TotalTime}</td></tr>");
                    }
                    else
                    {
                        html.Append($"<tr><td>{emp.EmployeeName} </td><td>{emp.TotalTime}</td></tr>");
                    }
                }
                html.Append("</table>\r\n</body>\r\n</html>");



                string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                File.WriteAllText(Path.Combine(folder, "employeeReport.html"), html.ToString());

                double totalHours = sortedEmployees.Sum(e => e.TotalTime.TotalHours);



                float startAngle = 0;

                Color[] colors = {
                Color.Red, Color.Green, Color.Blue, Color.Orange,
                Color.Purple, Color.Yellow, Color.Pink, Color.Cyan,
                Color.Brown, Color.Gray, Color.LightGreen
            };

                Bitmap bitmap = new Bitmap(700, 850);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);

                    for (int i = 0; i < sortedEmployees.Count; i++)
                    {
                        double hours = sortedEmployees[i].TotalTime.TotalHours;
                        double percentage = hours / totalHours * 100;
                        float angle = (float)(percentage * 360 / 100);

                        Brush brush = new SolidBrush(colors[i % colors.Length]);

                        graphics.FillPie(brush, 50, 50, 450, 450, startAngle, angle);

                        startAngle += angle;
                        brush.Dispose();
                    }

                    for (int i = 0; i < sortedEmployees.Count; i++)
                    {
                        double percentage = sortedEmployees[i].TotalTime.TotalHours / totalHours * 100;

                        graphics.FillRectangle(new SolidBrush(colors[i % colors.Length]), 50, 510 + i * 25, 15, 15);

                        graphics.DrawString(sortedEmployees[i].EmployeeName + " - " + percentage.ToString("0.0") + "%", new Font("Arial", 10), Brushes.Black, 75, 510 + i * 25);
                    }

                    bitmap.Save(Path.Combine(folder, "EmployeePieChart.png"), ImageFormat.Png);
                }

                bitmap.Dispose();


            }
            catch
            {
                throw;
            }
        }
    }
}

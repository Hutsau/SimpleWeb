using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для LogsPage.xaml
    /// </summary>
    public partial class LogsPage : Page
    {
        static readonly string LogFile = $@"Logs\{DateTime.Now.ToString("dd_MM_yyyy")}.log";

        static readonly string FormatLogTime = @"^\[\d\d:\d\d:\d\d\]";
        static readonly string FormatLog = @"^\[\d\d:\d\d:\d\d\] .+";

        static readonly string StartLog = "~~~~~~~~~~ LogListener started ~~~~~~~~~~~\r\r";
        static readonly string StopLog = "\r~~~~~~~~~ LogListener is stopped  ~~~~~~~~~\r";

        static TimeSpan LastLogTime;
        static DateTime LastWriteTime;


        public LogsPage()
        {
            InitializeComponent();

            

            this.Loaded += (sender, e) => {
                Content.Focus();

                Content.AppendText(StartLog);
                LogListener();     
            };
        }

        private async void LogListener() {
            var FirstFlag = true;
            LastWriteTime = new DateTime();
            while (true)
            {
                await Task.Delay(1);

                try
                {
                    
                    if (LastWriteTime == new FileInfo(LogFile).LastWriteTime) continue;

                    LastWriteTime = new FileInfo(LogFile).LastWriteTime;

                    var newLines = File.ReadAllLines(LogFile, Encoding.UTF8)
                        .Where(x => Regex.IsMatch(x, FormatLog) &&
                                    TimeSpan.Parse(Regex.Replace(Regex.Match(x, FormatLogTime).Value, @"[\[\]]", "")) > LastLogTime)
                        .ToList();

                    if (newLines.LastOrDefault() != null)
                        LastLogTime = TimeSpan.Parse(Regex.Replace(Regex.Match(newLines.Last(), FormatLogTime).Value, @"[\[\]]", ""));

                    foreach (var line in newLines) {
                        if (!FirstFlag)
                        {
                            GlobalVars.growlNotifications.AddNotification(new Notification
                            {
                                Title = Regex.Replace(Regex.Match(line, FormatLogTime).Value, @"[\[\]]", ""),
                                ImageUrl = "pack://application:,,,/Resources/microsoft-windows-8-logo.png",
                                Message = Regex.Replace(line, $"{FormatLogTime} ", "")
                            });
                            await Task.Delay(1000);
                        }

                        Content.AppendText($"{line}\r");
                    } Content.CaretPosition = Content.Document.ContentEnd;

                    FirstFlag = false;
                }
                catch (FileNotFoundException) {
                    Content.AppendText($"{StopLog}Файл {LogFile} не найден.\r");
                    Content.CaretPosition = Content.Document.ContentEnd;
                    break;
                }
                catch (DirectoryNotFoundException) {
                    Content.AppendText($"{StopLog}Файл {LogFile} не найден.\r");
                    Content.CaretPosition = Content.Document.ContentEnd;
                    break;
                }
                catch (Exception ex) {
                    Content.AppendText($"{StopLog}{ex.Message}\rПопробуйте перезагрузить страницу. Если не помогает, свяжитесь с разработчиком.\r");
                    Content.CaretPosition = Content.Document.ContentEnd;
                    break;
                }
            }
        }
    }
}

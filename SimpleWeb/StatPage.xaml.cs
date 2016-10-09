using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using SimpleWeb.Models;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using System.Data;
using System.Threading;

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для StatPage.xaml
    /// </summary>
    public partial class StatPage : Page
    {
        Repository repository = new Repository();
        static readonly string MY_TAG = "AIzaSyCy26ZyY-VB9beUVbElXLJ4lwlChE-fSOU";
        static readonly byte maxRank = 9; 
        IEnumerable<int> studentsID;

        Student currStudent;
        Group currGroup;
        //IEnumerable<Test> tests;

        List<StatView> preDG_Html;
        List<StatView> preDG_Css;
        List<StatView> preDG_Js;
        List<StatView> preDG_Xml;

        public StatPage()
        {
            InitializeComponent();

            // DEBUG
            //
            if (GlobalVars.flag)
            {
                repository.SaveGroup(new Group { GroupID = 0, GroupNumber = "32411" });
                repository.SaveGroup(new Group { GroupID = 0, GroupNumber = "22411" });
                repository.SaveStudent(new Student
                {
                    StudentID = 0,
                    GroupID = 1,
                    Name = "Alex",
                    Surname = "1Hutsau",
                });
                repository.SaveStudent(new Student
                {
                    StudentID = 0,
                    GroupID = 1,
                    Name = "Alex",
                    Surname = "Hadkevich",
                });
                repository.SaveStudent(new Student
                {
                    StudentID = 0,
                    GroupID = 2,
                    Name = "Alex",
                    Surname = "Hutsau",
                });
                repository.SaveStudent(new Student
                {
                    StudentID = 0,
                    GroupID = 1,
                    Name = "Alexey",
                    Surname = "Hutsau",
                });

                setData();
            }
            GlobalVars.flag = false;

            SearchGroup.GotFocus += mFocus;
            SearchStudent.GotFocus += mFocus;
            SearchGroup.TextChanged += mTextChanged;
            SearchStudent.TextChanged += mTextChanged;

            AddGroupBt.MouseUp += GlobalVars.mMouseUp;
            EditGroupBt.MouseUp += GlobalVars.mMouseUp;
            DeleteGroupBt.MouseUp += GlobalVars.mMouseUp;
            AddStudentBt.MouseUp += GlobalVars.mMouseUp;
            EditStudentBt.MouseUp += GlobalVars.mMouseUp;
            DeleteStudentBt.MouseUp += GlobalVars.mMouseUp;

            AddGroupBt.MouseDown += GlobalVars.mMouseDown;
            EditGroupBt.MouseDown += GlobalVars.mMouseDown;
            DeleteGroupBt.MouseDown += GlobalVars.mMouseDown;
            AddStudentBt.MouseDown += GlobalVars.mMouseDown;
            EditStudentBt.MouseDown += GlobalVars.mMouseDown;
            DeleteStudentBt.MouseDown += GlobalVars.mMouseDown;

            AddGroupBt.MouseLeave += GlobalVars.mMouseLeave;
            EditGroupBt.MouseLeave += GlobalVars.mMouseLeave;
            DeleteGroupBt.MouseLeave += GlobalVars.mMouseLeave;
            AddStudentBt.MouseLeave += GlobalVars.mMouseLeave;
            EditStudentBt.MouseLeave += GlobalVars.mMouseLeave;
            DeleteStudentBt.MouseLeave += GlobalVars.mMouseLeave;

            AddGroupBt.MouseUp += mMouseUp;
            EditGroupBt.MouseUp += mMouseUp;
            DeleteGroupBt.MouseUp += mMouseUp;
            AddStudentBt.MouseUp += mMouseUp;
            EditStudentBt.MouseUp += mMouseUp;
            DeleteStudentBt.MouseUp += mMouseUp;

            AddGroupBt.IsEnabled = true;
            EditGroupBt.IsEnabled = false;
            DeleteGroupBt.IsEnabled = false;
            AddStudentBt.IsEnabled = false;
            EditStudentBt.IsEnabled = false;
            DeleteStudentBt.IsEnabled = false;

            AddGroupBt.ToolTip = "Добавить группу";
            EditGroupBt.ToolTip = "Изменить группу";
            DeleteGroupBt.ToolTip = "Удалить группу";
            AddStudentBt.ToolTip = "Добавить студента";
            EditStudentBt.ToolTip = "Изменить студента";
            DeleteStudentBt.ToolTip = "Удалить студента";

            RankDown.ToolTip = "Rank down";
            RankUp.ToolTip = "Rank up";

            SearchGroupClearBt.MouseUp += mImageMouseUp;
            SearchStudentClearBt.MouseUp += mImageMouseUp;
            RankUp.MouseUp += mImageMouseUp;
            RankDown.MouseUp += mImageMouseUp;

            DG_Header.Sorting += SortHandler;
            DG_Html.SelectionChanged += mSelectionChanged;
            DG_Css.SelectionChanged += mSelectionChanged;
            DG_Js.SelectionChanged += mSelectionChanged;
            DG_Xml.SelectionChanged += mSelectionChanged;
            Separator_html.Content = "< HTML />";
            Separator_css.Content = "{ CSS }";
            Separator_js.Content = "JavaScript ( )";
            Separator_xml.Content = "< XML />";

            BadStat.Visibility = Visibility.Visible;
        }

        // DEBUG
        private void setData()
        {
            repository.SaveTest(new Test
            {
                TestID = 0,
                Name = $"{GlobalVars.HTML_TAG}Test 1",
                Priority = 0,
                Time = 45
            });
            repository.SaveTest(new Test
            {
                TestID = 0,
                Name = $"{GlobalVars.HTML_TAG}Test 5",
                Priority = 0,
                Time = 45
            });
            repository.SaveTest(new Test
            {
                TestID = 0,
                Name = $"{GlobalVars.HTML_TAG}Test 2",
                Priority = 0,
                Time = 45
            });
            repository.SaveTest(new Test
            {
                TestID = 0,
                Name = $"{GlobalVars.JS_TAG}Test 3",
                Priority = 0,
                Time = 45
            });
            repository.SaveTest(new Test
            {
                TestID = 0,
                Name = $"{GlobalVars.HTML_TAG}Test 4",
                Priority = 0,
                Time = 45
            });

            repository.SaveTest(new Test
            {
                TestID = 0,
                Name = $"{GlobalVars.XML_TAG}Test 6",
                Priority = 0,
                Time = 45
            });

            repository.SaveStat(new Stat
            {
                StudentID = 1,
                TestID = 1,
                Result = 5,
                Date = DateTime.Now
            });
            repository.SaveStat(new Stat
            {
                StudentID = 1,
                TestID = 3,
                Result = 3,
                Date = DateTime.Now
            });
            repository.SaveStat(new Stat
            {
                StudentID = 1,
                TestID = 3,
                Result = 6,
                Date = DateTime.Now
            });
            repository.SaveStat(new Stat
            {
                StudentID = 1,
                TestID = 4,
                Result = 7,
                Date = DateTime.Now
            });
        }

        private void SortHandler(object sender, DataGridSortingEventArgs e) {
            if (!preDG_Html.Any() && !preDG_Css.Any() && !preDG_Js.Any() && !preDG_Xml.Any()) return;

            e.Column.SortDirection = e.Column.SortDirection == ListSortDirection.Ascending ? 
                ListSortDirection.Descending : ListSortDirection.Ascending;

            DG_Html.Items.Clear();
            DG_Css.Items.Clear();
            DG_Js.Items.Clear();
            DG_Xml.Items.Clear();

            mSort(e.Column);      
        }

        private void mSort(DataGridColumn column) {
            if (column != null)
                    switch (column.DisplayIndex) {
                        case 0:
                            switch (column.SortDirection) {
                                case ListSortDirection.Ascending:
                                    preDG_Html.OrderBy(x => x.Rank)
                                              .ToList()
                                              .ForEach(x => DG_Html.Items.Add(x));

                                    preDG_Css.OrderBy(x => x.Rank)
                                             .ToList()
                                             .ForEach(x => DG_Css.Items.Add(x));

                                    preDG_Js.OrderBy(x => x.Rank)
                                             .ToList()
                                             .ForEach(x => DG_Js.Items.Add(x));

                                    preDG_Xml.OrderBy(x => x.Rank)
                                             .ToList()
                                             .ForEach(x => DG_Xml.Items.Add(x));

                                    break;
                                case ListSortDirection.Descending:
                                    preDG_Html.OrderByDescending(x => x.Rank)
                                              .ToList()
                                              .ForEach(x => DG_Html.Items.Add(x));

                                    preDG_Css.OrderByDescending(x => x.Rank)
                                             .ToList()
                                             .ForEach(x => DG_Css.Items.Add(x));

                                    preDG_Js.OrderByDescending(x => x.Rank)
                                             .ToList()
                                             .ForEach(x => DG_Js.Items.Add(x));

                                    preDG_Xml.OrderByDescending(x => x.Rank)
                                             .ToList()
                                             .ForEach(x => DG_Xml.Items.Add(x));

                                    break;
                            } break;
                        case 1:
                            switch (column.SortDirection) {
                                case ListSortDirection.Ascending:
                                    preDG_Html.Where(x => x.BestResult != "-")
                                              .OrderBy(x => x.BestResult)
                                              .ToList()
                                              .ForEach(x => DG_Html.Items.Add(x));
                                    preDG_Html.Where(x => x.BestResult == "-")
                                              .ToList()
                                              .ForEach(x => DG_Html.Items.Add(x));

                                    preDG_Css.Where(x => x.BestResult != "-")
                                              .OrderBy(x => x.BestResult)
                                              .ToList()
                                              .ForEach(x => DG_Css.Items.Add(x));
                                    preDG_Css.Where(x => x.BestResult == "-")
                                              .ToList()
                                              .ForEach(x => DG_Css.Items.Add(x));

                                    preDG_Js.Where(x => x.BestResult != "-")
                                              .OrderBy(x => x.BestResult)
                                              .ToList()
                                              .ForEach(x => DG_Js.Items.Add(x));
                                    preDG_Js.Where(x => x.BestResult == "-")
                                              .ToList()
                                              .ForEach(x => DG_Js.Items.Add(x));

                                    preDG_Xml.Where(x => x.BestResult != "-")
                                              .OrderBy(x => x.BestResult)
                                              .ToList()
                                              .ForEach(x => DG_Xml.Items.Add(x));
                                    preDG_Xml.Where(x => x.BestResult == "-")
                                              .ToList()
                                              .ForEach(x => DG_Xml.Items.Add(x));

                                    break;
                                case ListSortDirection.Descending:
                                    preDG_Html.OrderByDescending(x => x.BestResult)
                                              .ToList()
                                              .ForEach(x => DG_Html.Items.Add(x));

                                    preDG_Css.OrderByDescending(x => x.BestResult)
                                             .ToList()
                                             .ForEach(x => DG_Css.Items.Add(x));

                                    preDG_Js.OrderByDescending(x => x.BestResult)
                                             .ToList()
                                             .ForEach(x => DG_Js.Items.Add(x));

                                    preDG_Xml.OrderByDescending(x => x.BestResult)
                                             .ToList()
                                             .ForEach(x => DG_Xml.Items.Add(x));

                                    break;
                            } break;
                        case 2:
                            switch (column.SortDirection) {
                                case ListSortDirection.Ascending:
                                    preDG_Html.OrderBy(x => x.TestName)
                                              .ToList()
                                              .ForEach(x => DG_Html.Items.Add(x));

                                    preDG_Css.OrderBy(x => x.TestName)
                                             .ToList()
                                             .ForEach(x => DG_Css.Items.Add(x));

                                    preDG_Js.OrderBy(x => x.TestName)
                                             .ToList()
                                             .ForEach(x => DG_Js.Items.Add(x));

                                    preDG_Xml.OrderBy(x => x.TestName)
                                             .ToList()
                                             .ForEach(x => DG_Xml.Items.Add(x));

                                    break;
                                case ListSortDirection.Descending:
                                    preDG_Html.OrderByDescending(x => x.TestName)
                                              .ToList()
                                              .ForEach(x => DG_Html.Items.Add(x));

                                    preDG_Css.OrderByDescending(x => x.TestName)
                                             .ToList()
                                             .ForEach(x => DG_Css.Items.Add(x));

                                    preDG_Js.OrderByDescending(x => x.TestName)
                                             .ToList()
                                             .ForEach(x => DG_Js.Items.Add(x));

                                    preDG_Xml.OrderByDescending(x => x.TestName)
                                             .ToList()
                                             .ForEach(x => DG_Xml.Items.Add(x));

                                    break;
                            }
                            break;
                    }
            else {
                preDG_Html.ForEach(x => DG_Html.Items.Add(x));

                preDG_Css.ForEach(x => DG_Css.Items.Add(x));

                preDG_Js.ForEach(x => DG_Js.Items.Add(x));

                preDG_Xml.ForEach(x => DG_Xml.Items.Add(x));
            }
        }

        private void groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddStudentBt.IsEnabled = false;
            EditStudentBt.IsEnabled = false;
            DeleteStudentBt.IsEnabled = false;
            EditGroupBt.IsEnabled = false;
            DeleteGroupBt.IsEnabled = false;
            Students.SelectedItem = null;

            SearchStudent.Text = MY_TAG;
            SearchStudent.Text = "";
            if (Groups.SelectedItem != null) {
                currGroup = repository.GetGroup((string)Groups.SelectedItem);

                AddStudentBt.IsEnabled = true;
                EditGroupBt.IsEnabled = true;
                DeleteGroupBt.IsEnabled = true;
            }
        }

        private void students_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditStudentBt.IsEnabled = false;
            DeleteStudentBt.IsEnabled = false;
            BadStat.Visibility = Visibility.Visible;

            if (Students.SelectedItem != null) {
                currStudent = repository.GetStudent(studentsID.ElementAt(Students.SelectedIndex));

                EditStudentBt.IsEnabled = true;
                DeleteStudentBt.IsEnabled = true;

                BadStat.Visibility = Visibility.Collapsed;

                StudentFIO.Content = $"{currStudent.Surname} {currStudent.Name} {currStudent.SecondName}".Trim();
                StudentRank.Content = $"{currStudent.Progress}";

                DG_Html.Items.Clear();
                DG_Css.Items.Clear();
                DG_Js.Items.Clear();
                DG_Xml.Items.Clear();

                preDG_Html = new List<StatView>();
                preDG_Css = new List<StatView>();
                preDG_Js = new List<StatView>();
                preDG_Xml = new List<StatView>();

                repository.GetTests.ToList().ForEach(x => {
                    var statView = new StatView {
                        TestID = x.TestID,
                        Rank = x.Priority,
                        BestResult = x.Stats
                            .Where(xl => xl.StudentID == currStudent.StudentID)
                            .GroupBy(xl => xl.TestID)
                            .Select(xl => xl.Max(xxl => xxl.Result))
                            .FirstOrDefault()
                            .ToString(),
                        TestName = x.Name.Substring(1)
                    };

                    if (x.Name.ElementAt(0) == GlobalVars.HTML_TAG) preDG_Html.Add(statView);
                    if (x.Name.ElementAt(0) == GlobalVars.CSS_TAG) preDG_Css.Add(statView);
                    if (x.Name.ElementAt(0) == GlobalVars.JS_TAG) preDG_Js.Add(statView);
                    if (x.Name.ElementAt(0) == GlobalVars.XML_TAG) preDG_Xml.Add(statView);
                });

                mSort(DG_Header.Columns.SingleOrDefault(x => x.SortDirection != null));

                BadHtml.Visibility = preDG_Html.Any() ? Visibility.Collapsed : Visibility.Visible;
                BadCss.Visibility = preDG_Css.Any() ? Visibility.Collapsed : Visibility.Visible;
                BadJs.Visibility = preDG_Js.Any() ? Visibility.Collapsed : Visibility.Visible;
                BadXml.Visibility = preDG_Js.Any() ? Visibility.Collapsed : Visibility.Visible;

            } else foreach (var x in DG_Header.Columns) x.SortDirection = null;
        }

        private void mSelectionChanged(object sender, SelectionChangedEventArgs e) {

            if (e.AddedItems.Count == 0) {
                DG_HtmlDop.Items.Clear();
                DG_CssDop.Items.Clear();
                DG_JsDop.Items.Clear();
                DG_XmlDop.Items.Clear();
                return;
            }

            var TestID = ((StatView)e.AddedItems[0]).TestID;
            switch (((DataGrid)sender).Name) {
                case "DG_Html":
                    DG_HtmlDop.Items.Clear();
                    foreach (var x in currStudent.Stats.Where(x => x.TestID == TestID))
                        DG_HtmlDop.Items.Add(new StatViewDop {
                            Date = x.Date.ToString("g"),
                            Result = x.Result
                        });
                    break;
                case "DG_Css":
                    DG_CssDop.Items.Clear();
                    foreach (var x in currStudent.Stats.Where(x => x.TestID == TestID))
                        DG_CssDop.Items.Add(new StatViewDop {
                            Date = x.Date.ToString("g"),
                            Result = x.Result
                        });
                    break;
                case "DG_Js":
                    DG_JsDop.Items.Clear();
                    foreach (var x in currStudent.Stats.Where(x => x.TestID == TestID))
                        DG_JsDop.Items.Add(new StatViewDop {
                            Date = x.Date.ToString("g"),
                            Result = x.Result
                        });
                    break;
                case "DG_Xml":
                    DG_XmlDop.Items.Clear();
                    foreach (var x in currStudent.Stats.Where(x => x.TestID == TestID))
                        DG_XmlDop.Items.Add(new StatViewDop {
                            Date = x.Date.ToString("g"),
                            Result = x.Result
                        });
                    break;
            }
        }

        private void mFocus(object sender, RoutedEventArgs e) {
            switch (((TextBox)sender).Name) {
                case "SearchGroup":
                    Groups.SelectedItem = null;
                    break;
                case "SearchStudent":
                    Students.SelectedItem = null;
                    break;
            }
        }

        private void mTextChanged(object sender, TextChangedEventArgs e) {
            switch (((TextBox)sender).Name) {
                case "SearchGroup":
                    if (SearchGroup.Text.Equals(MY_TAG)) break;

                    SearchGroupClearBt.Visibility = SearchGroup.Text.Equals("") ? 
                        Visibility.Hidden : Visibility.Visible;

                    Groups.ItemsSource = repository.GetGroupsStrings
                        .Where(x => x.Contains(SearchGroup.Text));

                    SearchGroup.Visibility = Groups.Items.Count == 0 && SearchGroup.Text.Equals("") ? 
                        Visibility.Hidden : Visibility.Visible;

                    break;
                case "SearchStudent":
                    if (SearchStudent.Text.Equals(MY_TAG)) break;

                    SearchStudentClearBt.Visibility = SearchStudent.Text.Equals("") ?
                        Visibility.Hidden : Visibility.Visible;

                    try {
                        var students = repository.GetStudents((string)Groups.SelectedItem)
                            .Where(x => x.Surname.ToLower().Contains(SearchStudent.Text));

                        Students.ItemsSource = students.Select(x => x.Surname);
                        studentsID = students.Select(x => x.StudentID);
                    } catch (Exception) {
                        Students.ItemsSource = null;
                    }

                    SearchStudent.Visibility = Students.Items.Count == 0 && SearchStudent.Text.Equals("") ?
                        Visibility.Hidden : Visibility.Visible;

                    break;
            }
        }

        private void mMouseUp(object sender, MouseButtonEventArgs e) {
            if (!GlobalVars.IsPressedFlag) return;

            GlobalVars.IsPressedFlag = false;

            switch (((Grid)sender).Name) {
                case "AddGroupBt":
                    GlobalVars.groupID = 0;

                    new EditGroup { Owner = ((MainWindow)Application.Current.MainWindow) }.ShowDialog();
                    
                    if (GlobalVars.saveFlag) {
                        repository = new Repository();

                        SearchGroup.Text = MY_TAG;
                        SearchGroup.Text = "";
                    }

                    break;
                case "EditGroupBt":
                    GlobalVars.groupID = currGroup.GroupID;

                    new EditGroup { Owner = ((MainWindow)Application.Current.MainWindow) }.ShowDialog();

                    if (GlobalVars.saveFlag) {
                        repository = new Repository();

                        SearchGroup.Text = MY_TAG;
                        SearchGroup.Text = "";
                    }

                    break;
                case "DeleteGroupBt":
                    GlobalVars.groupID = currGroup.GroupID;
                    GlobalVars.studentID = 0;
                    GlobalVars.testID = 0;

                    new DeleteWindow { Owner = ((MainWindow)Application.Current.MainWindow) }.ShowDialog();

                    if (GlobalVars.deleteFlag) {
                        repository = new Repository();

                        SearchGroup.Text = MY_TAG;
                        SearchGroup.Text = "";
                    }

                    break;
                case "AddStudentBt":
                    GlobalVars.studentID = 0;
                    GlobalVars.groupID = currGroup.GroupID;

                    new EditStudent { Owner = ((MainWindow)Application.Current.MainWindow) }.ShowDialog();

                    if (GlobalVars.saveFlag) {
                        repository = new Repository();

                        SearchStudent.Text = MY_TAG;
                        SearchStudent.Text = "";
                    }

                    break;
                case "EditStudentBt":
                    GlobalVars.studentID = currStudent.StudentID;
                    GlobalVars.groupID = currGroup.GroupID;

                    new EditStudent { Owner = ((MainWindow)Application.Current.MainWindow) }.ShowDialog();

                    if (GlobalVars.saveFlag) {
                        repository = new Repository();

                        SearchStudent.Text = MY_TAG;
                        SearchStudent.Text = "";
                    }

                    break;
                case "DeleteStudentBt":
                    GlobalVars.groupID = 0;
                    GlobalVars.studentID = currStudent.StudentID;
                    GlobalVars.testID = 0;

                    new DeleteWindow { Owner = ((MainWindow)Application.Current.MainWindow) }.ShowDialog();

                    if (GlobalVars.deleteFlag) {
                        repository = new Repository();

                        SearchStudent.Text = MY_TAG;
                        SearchStudent.Text = "";
                    }

                    break;
            }
        }

        private void mGridMouseDown(object sender, MouseButtonEventArgs e) {
            GlobalVars.IsPressedFlag = true;
        }

        private static void mGridMouseLeave(object sender, MouseEventArgs e) {
            GlobalVars.IsPressedFlag = false;
        }

        private void mImageMouseUp(object sender, MouseButtonEventArgs e) {
            switch (((Image)sender).Name) {
                case "SearchGroupClearBt":
                    SearchGroup.Text = "";
                    SearchGroupClearBt.Visibility = Visibility.Hidden;
                    break;

                case "SearchStudentClearBt":
                    SearchStudent.Text = "";
                    SearchStudentClearBt.Visibility = Visibility.Hidden;
                    break;

                case "RankUp":
                    if (currStudent.Progress != maxRank) {
                        StudentRank.Content = currStudent.Progress += 1;
                        repository.SaveStudent(currStudent);
                    } break;

                case "RankDown":
                    if (currStudent.Progress != 0) {
                        StudentRank.Content = currStudent.Progress -= 1;
                        repository.SaveStudent(currStudent);
                    } break; 
            }
        }
    }
}

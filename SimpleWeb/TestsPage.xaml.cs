using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MahApps.Metro.Controls.Dialogs;
using SimpleWeb.Models;
using System.ComponentModel;

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для TestsPage.xaml
    /// </summary>
    public partial class TestsPage : Page
    {
        Repository repository = new Repository();
        List<QTestView> _QTests = new List<QTestView>();
        Test currTest;

        List<TestView> preDG_Html;
        List<TestView> preDG_Css;
        List<TestView> preDG_Js;
        List<TestView> preDG_Xml;

        public TestsPage()
        {
            InitializeComponent();

            Separator_html.Content = "< HTML />";
            Separator_css.Content = "{ CSS }";
            Separator_js.Content = "JavaScript ( )";
            Separator_xml.Content = "< XML />";

            CreateTestBt.MouseDown += GlobalVars.mMouseDown;
            SaveBt.MouseDown += GlobalVars.mMouseDown;
            DeleteTestBt.MouseDown += GlobalVars.mMouseDown;
            ClearFormBt.MouseDown += GlobalVars.mMouseDown;
            CloseFormBt.MouseDown += GlobalVars.mMouseDown;

            CreateTestBt.MouseUp += GlobalVars.mMouseUp;
            SaveBt.MouseUp += GlobalVars.mMouseUp;
            DeleteTestBt.MouseUp += GlobalVars.mMouseUp;
            ClearFormBt.MouseUp += GlobalVars.mMouseUp;
            CloseFormBt.MouseUp += GlobalVars.mMouseUp;

            CreateTestBt.MouseLeave += GlobalVars.mMouseLeave;
            SaveBt.MouseLeave += GlobalVars.mMouseLeave;
            DeleteTestBt.MouseLeave += GlobalVars.mMouseLeave;
            ClearFormBt.MouseLeave += GlobalVars.mMouseLeave;
            CloseFormBt.MouseLeave += GlobalVars.mMouseLeave;

            CreateTestBt.MouseUp += mMouseUp;
            SaveBt.MouseUp += mMouseUp;
            DeleteTestBt.MouseUp += mMouseUp;
            ClearFormBt.MouseUp += mMouseUp;
            CloseFormBt.MouseUp += mMouseUp;

            BadTest.Visibility = Visibility.Visible;

            DG_Header.Sorting += SortHandler;
            DG_Html.SelectionChanged += mSelectionChanged;
            DG_Css.SelectionChanged += mSelectionChanged;
            DG_Js.SelectionChanged += mSelectionChanged;
            DG_Xml.SelectionChanged += mSelectionChanged;

            TestName.TextChanged += mTextChanged;

            GetTests();

            _QTests.Add(new QTestView {
                Question = "Question 1",
                RightAnswer = "Right Answer",
                WrongAnswers = "Wrong Anwer 1 | Wrong Answer 2" });

            _QTests.Add(new QTestView {
                Question = "Question 2",
                RightAnswer = "Right Answer",
                WrongAnswers = "Wrong Anwer 1 | Wrong Answer 2" });

            mToolTip.Content = "Для разделения неверных\nответов используйте #\nВ сочетании с пробелами.";

            QTests.ItemsSource = new ListCollectionView(_QTests);
        }

        private void GetTests() {
            CELabel.Content = string.Empty;
            BadTest.Visibility = Visibility.Visible;

            DG_Html.Items.Clear();
            DG_Css.Items.Clear();
            DG_Js.Items.Clear();
            DG_Xml.Items.Clear();

            preDG_Html = new List<TestView>();
            preDG_Css = new List<TestView>();
            preDG_Js = new List<TestView>();
            preDG_Xml = new List<TestView>();

            repository.GetTests.ToList().ForEach(x => {
                var testView = new TestView {
                    TestID = x.TestID,
                    Rank = x.Priority,
                    TestName = x.Name.Substring(1)
                };

                if (x.Name.ElementAt(0) == GlobalVars.HTML_TAG) preDG_Html.Add(testView);
                if (x.Name.ElementAt(0) == GlobalVars.CSS_TAG) preDG_Css.Add(testView);
                if (x.Name.ElementAt(0) == GlobalVars.JS_TAG) preDG_Js.Add(testView);
                if (x.Name.ElementAt(0) == GlobalVars.XML_TAG) preDG_Xml.Add(testView);
            });

            mSort(DG_Header.Columns.SingleOrDefault(x => x.SortDirection != null));

            BadHtml.Visibility = preDG_Html.Any() ? Visibility.Collapsed : Visibility.Visible;
            BadCss.Visibility = preDG_Css.Any() ? Visibility.Collapsed : Visibility.Visible;
            BadJs.Visibility = preDG_Js.Any() ? Visibility.Collapsed : Visibility.Visible;
            BadXml.Visibility = preDG_Xml.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private bool IsTestChanged
        {
            get
            {
                if (BadTest.Visibility == Visibility.Visible) return true;

                if (CELabel.Content.ToString() == "Create test".ToUpper())
                {
                    if (TestName.Text == string.Empty &&
                        TestType.SelectedIndex == 0 &&
                        TestRank.SelectedIndex == 0 &&
                        TestTime.SelectedIndex == 0 &&
                        !_QTests.Any()) return false;
                    else return true;
                }
                else
                {
                    var qtests = repository.GetQTests(currTest.TestID);

                    var flag = true;
                    foreach (var x in _QTests) {
                        var index = _QTests.IndexOf(x);
                        try
                        {
                            if (x.QTestID != qtests.ElementAt(index).QTestID ||
                                x.Question != qtests.ElementAt(index).Question ||
                                x.RightAnswer != qtests.ElementAt(index).RightAnswer ||
                                x.WrongAnswers != qtests.ElementAt(index).WrongAnswers)
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception) { flag = false; break; }
                    }

                    if (TestName.Text == currTest.Name.Substring(1) &&
                        TestType.SelectedIndex == int.Parse(currTest.Name[0].ToString()) &&
                        TestRank.SelectedIndex == currTest.Priority &&
                        TestTime.SelectedIndex == currTest.Time / 15 - 1 &&
                        flag) return false;
                    else return true;

                    
                }
            }
        }

        private void mTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length == 1)
                ((TextBox)sender).Text = ((TextBox)sender).Text.ToUpper();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
        }

        private async void SortHandler(object sender, DataGridSortingEventArgs e) {
            if (!preDG_Html.Any() && !preDG_Css.Any() && !preDG_Js.Any() && !preDG_Xml.Any()) return;

            if (IsTestChanged &&
                BadTest.Visibility == Visibility.Collapsed &&
                await ((MainWindow)Application.Current.MainWindow)
                .ShowMessageAsync("Discard changes", "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative)
                == MessageDialogResult.Negative) return;

            BadTest.Visibility = Visibility.Visible;

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
                            } break;
                    }
            else {
                preDG_Html.ForEach(x => DG_Html.Items.Add(x));

                preDG_Css.ForEach(x => DG_Css.Items.Add(x));

                preDG_Js.ForEach(x => DG_Js.Items.Add(x));

                preDG_Xml.ForEach(x => DG_Xml.Items.Add(x));
            }
        }

        bool SelectionFlag = false;
        bool ConfirmFlag = false;
        int currIndex;
        private async void mSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count == 0) return;

            ErrorLabel.Visibility = Visibility.Collapsed;

            if (!SelectionFlag &&
                BadTest.Visibility == Visibility.Collapsed && 
                IsTestChanged && 
                await ((MainWindow)Application.Current.MainWindow)
                .ShowMessageAsync("Discard changes", "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative)
                == MessageDialogResult.Negative) {

                SelectionFlag = true;
                ConfirmFlag = true;

                DG_Html.SelectedItem = null;
                DG_Css.SelectedItem = null;
                DG_Js.SelectedItem = null;
                DG_Xml.SelectedItem = null;

                if (currTest != null)
                {
                    if (preDG_Html.SingleOrDefault(x => x.TestID == currTest.TestID) != null)
                        DG_Html.SelectedIndex = currIndex;

                    if (preDG_Css.SingleOrDefault(x => x.TestID == currTest.TestID) != null)
                        DG_Css.SelectedIndex = currIndex;

                    if (preDG_Js.SingleOrDefault(x => x.TestID == currTest.TestID) != null)
                        DG_Js.SelectedIndex = currIndex;

                    if (preDG_Xml.SingleOrDefault(x => x.TestID == currTest.TestID) != null)
                        DG_Xml.SelectedIndex = currIndex;
                }

                SelectionFlag = false;
                ConfirmFlag = false;
                return;
            }

            if (!ConfirmFlag) {
                switch (((DataGrid)sender).Name) {
                    case "DG_Html":
                        DG_Css.SelectedItem = null;
                        DG_Js.SelectedItem = null;
                        DG_Xml.SelectedItem = null;
                        break;
                    case "DG_Css":
                        DG_Html.SelectedItem = null;
                        DG_Js.SelectedItem = null;
                        DG_Xml.SelectedItem = null;
                        break;
                    case "DG_Js":
                        DG_Css.SelectedItem = null;
                        DG_Html.SelectedItem = null;
                        DG_Xml.SelectedItem = null;
                        break;
                    case "DG_Xml":
                        DG_Css.SelectedItem = null;
                        DG_Html.SelectedItem = null;
                        DG_Js.SelectedItem = null;
                        break;
                }

                currTest = repository.GetTest(((TestView)e.AddedItems[0]).TestID);
                currIndex = ((DataGrid)sender).SelectedIndex;

                CELabel.Content = "Edit test".ToUpper();
                TestName.Text = currTest.Name.Substring(1);

                TestType.SelectedIndex = int.Parse(currTest.Name[0].ToString());
                TestRank.SelectedIndex = currTest.Priority;
                TestTime.SelectedIndex = currTest.Time / 15 - 1;

                _QTests = repository.GetQTests(currTest.TestID);
                QTests.ItemsSource = new ListCollectionView(_QTests);

                BadTest.Visibility = Visibility.Collapsed;
            }

            DeleteTestBt.IsEnabled = true;
            ClearFormBt.IsEnabled = false;
        }

        private void mMouseUp(object sender, MouseButtonEventArgs e) {
            if (!GlobalVars.IsPressedFlag) return;

            GlobalVars.IsPressedFlag = false;

            FocusLabel.Focus();
            ErrorLabel.Visibility = Visibility.Collapsed;

            switch (((Grid)sender).Name) {
                case "CreateTestBt": CreateTest(); break;
                case "ClearFormBt": ClearForm(); break;
                case "CloseFormBt": CloseForm(); break;
                case "DeleteTestBt": DeleteTest(); break;
                case "SaveBt": Save(); break;
            }
        }

        private void Save() {
            if (currTest != null && !IsTestChanged) CloseForm();

            QTests.CommitEdit();

            if (TestName.Text == string.Empty) {
                ErrorLabel.Content = "[ Введите название теста ]";
                ErrorLabel.Visibility = Visibility.Visible;
                return;
            }

            if (currTest == null && !repository.CheckTestName(TestName.Text)) {
                ErrorLabel.Content = "[ Тест с таким названием уже существует ]";
                ErrorLabel.Visibility = Visibility.Visible;
                return;
            }

            if (_QTests.Count < 5) {
                ErrorLabel.Content = "[ Минимальное число вопросов - 5 ]";
                ErrorLabel.Visibility = Visibility.Visible;
                return;
            }

            if (_QTests.Where(x => x.Question == string.Empty ||
                    x.RightAnswer == string.Empty ||
                    x.WrongAnswers == string.Empty).Any()) {
                ErrorLabel.Content = "[ Все ячейки должны быть заполнены ]";
                ErrorLabel.Visibility = Visibility.Visible;
                return;
            }

            var test_id = repository.SaveTest(new Test {
                TestID = currTest == null ? 0 : currTest.TestID,
                Name = $"{TestType.SelectedIndex}{TestName.Text}",
                Priority = (byte)TestRank.SelectedIndex,
                Time = (byte)((TestTime.SelectedIndex + 1) * 15)
            }).TestID;

            List<QTest> qtests = new List<QTest>();
            foreach (var x in _QTests) {
                qtests.Add(new QTest {
                    QTestID = x.QTestID,
                    TestID = test_id,
                    Question = x.Question,
                    RightAnswer = x.RightAnswer,
                    WrongAnswers = x.WrongAnswers
                });
            } repository.SaveQTests(qtests);

            GetTests();
        }

        private void DeleteTest() {
            GlobalVars.groupID = 0;
            GlobalVars.studentID = 0;
            GlobalVars.testID = currTest.TestID;

            new DeleteWindow { Owner = ((MainWindow)Application.Current.MainWindow) }.ShowDialog();

            if (GlobalVars.deleteFlag) {
                repository = new Repository();

                GetTests();
            }
        }

        private async void CreateTest() {
            if (currTest == null && BadTest.Visibility == Visibility.Collapsed) return;

            if (IsTestChanged &&
                BadTest.Visibility == Visibility.Collapsed &&
                await ((MainWindow)Application.Current.MainWindow)
                .ShowMessageAsync("Discard changes", "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative)
                == MessageDialogResult.Negative) return;

            DG_Html.SelectedItem = null;
            DG_Css.SelectedItem = null;
            DG_Js.SelectedItem = null;
            DG_Xml.SelectedItem = null;

            currTest = null;

            CELabel.Content = "Create test".ToUpper();

            TestName.Text = string.Empty;
            TestType.SelectedIndex = 0;
            TestRank.SelectedIndex = 0;
            TestTime.SelectedIndex = 0;

            _QTests.Clear();
            QTests.ItemsSource = new ListCollectionView(_QTests);

            ClearFormBt.IsEnabled = true;
            DeleteTestBt.IsEnabled = false;
            BadTest.Visibility = Visibility.Collapsed;
        }

        private void ClearForm() {
            if (!IsTestChanged) return;

            TestName.Text = string.Empty;
            TestType.SelectedIndex = 0;
            TestRank.SelectedIndex = 0;
            TestTime.SelectedIndex = 0;

            _QTests.Clear();
            QTests.ItemsSource = new ListCollectionView(_QTests);
        }

        private async void CloseForm() {
            if (IsTestChanged && await ((MainWindow)Application.Current.MainWindow)
                .ShowMessageAsync("Discard changes", "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative)
                == MessageDialogResult.Negative) return;

            DG_Html.SelectedItem = null;
            DG_Css.SelectedItem = null;
            DG_Js.SelectedItem = null;
            DG_Xml.SelectedItem = null;
            CELabel.Content = string.Empty;
            BadTest.Visibility = Visibility.Visible;
        }

        private bool IsPressedFlag { get; set; } = false;
        private void mHyperlinkMouseDown(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).FontSize -= 1;
            IsPressedFlag = true;
        }
        private void mHyperlinkMouseLeave(object sender, MouseEventArgs e)
        {
            if (IsPressedFlag) {
                ((TextBlock)sender).FontSize += 1;
                IsPressedFlag = false;
            }
        }

        private void QTestUpBtMouseUp(object sender, MouseEventArgs e) {
            if (!IsPressedFlag) return;

            ((TextBlock)sender).FontSize += 1;

            IsPressedFlag = false;

            QTestUp(GetRowIndex(e.OriginalSource));
        }

        private void QTestUp(int index) {
            QTests.CommitEdit();

            if (index == -1 || index == 0) return;

            var currItem = _QTests.ElementAt(index);
            _QTests.RemoveAt(index);
            _QTests.Insert(index - 1, currItem);

            QTests.ItemsSource = new ListCollectionView(_QTests);
        }

        private void DeleteQTestBtMouseUp(object sender, MouseButtonEventArgs e) {
            if (!IsPressedFlag) return;

            ((TextBlock)sender).FontSize += 1;

            IsPressedFlag = false;

            DeleteQTest(GetRowIndex(e.OriginalSource));
        }

        private void DeleteQTest(int index) {
            QTests.CommitEdit();

            _QTests.RemoveAt(index);

            QTests.ItemsSource = new ListCollectionView(_QTests);
        }

        private int GetRowIndex(object OriginalSource) {
            var dep = (DependencyObject)OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
                dep = VisualTreeHelper.GetParent(dep);

            if (dep == null) return -1;

            var cell = (DataGridCell)dep;

            while ((dep != null) && !(dep is DataGridRow))
                dep = VisualTreeHelper.GetParent(dep);

            var row = (DataGridRow)dep;

            var datagrid = (DataGrid)ItemsControl.ItemsControlFromItemContainer(row);

            var index = datagrid.ItemContainerGenerator.IndexFromContainer(row);

            return index;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWeb.Models
{
    public class Repository
    {
        private mContext context = new mContext();

        public void Save() { context.SaveChanges(); }

        public async Task SaveAsync() {
            await Task.Run(() => {
                context.SaveChangesAsync();
            });
        }
        public string GetPass
        {
            get { return context.Admin.First().Password; }
            set { context.Admin.First().Password = value; context.SaveChanges(); }
        }

        public void SaveStudent(Student student) {
            if (student.StudentID == 0) context.Students.Add(student);
            else {
                var _student = context.Students.Find(student.StudentID);
                if (_student != null) {
                    _student.Name = student.Name;
                    _student.Surname = student.Surname;
                    _student.SecondName = student.SecondName;
                }
            } context.SaveChanges();
        }

        public void SaveGroup(Group group) {
            if (group.GroupID == 0) context.Groups.Add(group);
            else {
                var _group = context.Groups.Find(group.GroupID);
                if (_group != null) _group.GroupNumber = group.GroupNumber;
            } context.SaveChanges();
        }

        public Test SaveTest(Test test) {
            Test _test;
            if (test.TestID == 0) _test = context.Tests.Add(test);
            else {
                _test = context.Tests.Find(test.TestID);
                if (_test != null) {
                    _test.Name = test.Name;
                    _test.Priority = test.Priority;
                    _test.Time = test.Time;
                }
            } context.SaveChanges();
            return _test;
        }

        public void SaveQTest(QTest qtest) {
            if (qtest.QTestID == 0) context.QTests.Add(qtest);
            else {
                var _qtest = context.QTests.Find(qtest.QTestID);
                if (_qtest != null) {
                    _qtest.Question = qtest.Question;
                    _qtest.RightAnswer = qtest.RightAnswer;
                    _qtest.WrongAnswers = qtest.WrongAnswers;
                }
            } context.SaveChanges();
        }

        public void SaveQTests(List<QTest> qtests) {
            var _new = qtests.Where(x => x.QTestID == 0).ToArray();
            var _old = qtests.Where(x => x.QTestID != 0).ToArray();

            if (_new.Count() != 0) context.QTests.AddRange(_new);

            if (_old != null)
                foreach (var x in _old) {
                    var _qtest = context.QTests.Find(x.QTestID);
                    if (_qtest != null) {
                        _qtest.Question = x.Question;
                        _qtest.RightAnswer = x.RightAnswer;
                        _qtest.WrongAnswers = x.WrongAnswers;
                    }
                }

            context.SaveChanges();
        }

        public void SaveStat(Stat stat) {
            context.Stats.Add(stat);
            context.SaveChanges();
        }

        public IEnumerable<Student> GetAllStudents {
            get { return context.Students.OrderBy(x => x.Surname); }
        }

        public IEnumerable<Student> GetStudents(string groupNumber) {
            try {
                return context.Groups
                    .Single(x => x.GroupNumber == groupNumber)
                    .Students
                    .OrderBy(x => x.Surname);
            } catch (Exception) {
                return null;
            }
        }

        public Student GetStudent(int StudentID) {
            return context.Students.Find(StudentID);
        }

        public IEnumerable<string> GetGroupsStrings {
            get {
                return context.Groups
                  .Select(x => x.GroupNumber)
                  .OrderBy(x => x);
            }
        }

        public IEnumerable<Group> GetGroups {
            get { return context.Groups; }
        }

        public string GetGroupNumber(int GroupID) {
            return context.Groups.Find(GroupID).GroupNumber;
        }

        public Group GetGroup(string GroupNumber) {
            return context.Groups.SingleOrDefault(x => x.GroupNumber == GroupNumber);
        }

        public Group GetGroup(int GroupID) {
            return context.Groups.SingleOrDefault(x => x.GroupID == GroupID);
        }

        public IEnumerable<Test> GetTests {
            get { return context.Tests; }
        }

        public Test GetTest(int TestID) {
            return context.Tests.SingleOrDefault(x => x.TestID == TestID);
        }

        public bool CheckTestName(string TestName) {
            return context.Tests.Where(x => x.Name.Substring(1) == TestName).FirstOrDefault() == null ? true : false;
        }

        public List<QTestView> GetQTests(int TestID) {
            var mList = new List<QTestView>();

            foreach (var x in context.Tests.SingleOrDefault(x => x.TestID == TestID).QTests) {
                mList.Add(new QTestView {
                    QTestID = x.QTestID,
                    Question = x.Question,
                    RightAnswer = x.RightAnswer,
                    WrongAnswers = x.WrongAnswers
                });
            }

            return mList;
        }

        //public async Task DeleteGroupAsync(Group group) {
        //    foreach (var x in group.Students.ToArray())
        //        await DeleteStudentAsync(x);

        //    context.Groups.Remove(group);
        //    await context.SaveChangesAsync();
        //}

        public void DeleteGroup(Group group) {
            foreach (var x in group.Students.ToArray())
                DeleteStudent(x);

            context.Groups.Remove(group);
            //context.SaveChanges();
        }

        //public async Task DeleteStudentAsync(Student student) {
        //    context.Stats.RemoveRange(student.Stats);
        //    await context.SaveChangesAsync();

        //    context.Students.Remove(student);
        //    await context.SaveChangesAsync();
        //}

        public void DeleteStudent(Student student) {
            context.Stats.RemoveRange(student.Stats);
            //context.SaveChanges();

            context.Students.Remove(student);
            //context.SaveChanges();
        }

        public void DeleteStat(Stat stat) {
            context.Stats.Remove(stat);
            context.SaveChanges();
        }

        public void DeleteTest(Test test) {
            //foreach (var x in test.QTests.ToArray())
            //    DeleteQTests(x);

            context.QTests.RemoveRange(test.QTests);
            //context.SaveChanges();

            context.Stats.RemoveRange(test.Stats);
            //context.SaveChanges();

            context.Tests.Remove(test);
           // context.SaveChanges();
        }

        public void DeleteQTests(QTest qtest) {
            context.QTests.Remove(qtest);
            //context.SaveChanges();
        }
    }
}

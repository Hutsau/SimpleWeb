//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QTest
    {
        public int QTestID { get; set; }
        public int TestID { get; set; }
        public string Question { get; set; }
        public string RightAnswer { get; set; }
        public string WrongAnswers { get; set; }
    
        public virtual Test Test { get; set; }
    }
}
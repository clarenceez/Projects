//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace QIC.Farm.Service.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class FamilyCard
    {
        public int ID { get; set; }
        public int FamilyID { get; set; }
        public int CardKindID { get; set; }
        public int TotalCount { get; set; }
        public int DistributedCount { get; set; }
        public bool IsValid { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}

using System;
using System.Runtime.Serialization;
namespace BusinessLogic.BindingModel
{
    [DataContract]
    public class DogovorBM
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public decimal Cena { get; set; }
        [DataMember]
        public DateTime Data { get; set; }
        [DataMember]
        public string FIO { get; set; }
    }
}

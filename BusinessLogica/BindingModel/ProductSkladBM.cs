using BusinessLogica.Enum;
using System;
using System.Runtime.Serialization;
namespace BusinessLogic.BindingModel
{
    [DataContract]
    public class ProductSkladBM
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int SkladId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public DateTime Data { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public Status Status { get; set; }
    }
}

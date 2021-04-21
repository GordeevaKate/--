using System.Runtime.Serialization;
namespace BusinessLogic.BindingModel
{

        [DataContract]
    public class DogovorProductBM
        {

        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int DogovorId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}

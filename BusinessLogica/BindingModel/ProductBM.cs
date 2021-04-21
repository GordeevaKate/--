using System.Runtime.Serialization;
namespace BusinessLogic.BindingModel
{
    [DataContract]
    public  class ProductBM
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal Cena { get; set; }

    }
}

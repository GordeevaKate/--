using System.Runtime.Serialization;
namespace BusinessLogic.BindingModel
{
    [DataContract]
    public  class SkladBM
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}

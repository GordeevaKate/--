using System.Runtime.Serialization;

namespace BusinessLogic.BindingModel
{
    [DataContract]
    public class UserBM
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
}

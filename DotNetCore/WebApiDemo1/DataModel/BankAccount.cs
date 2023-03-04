using WebApiDemo1.Enums;

namespace WebApiDemo1.DataModel
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IfscCode { get; set; }
        public int AccountNumber { get; set; }
        public AccountTypes AccountType { get; set; }
        public string AccountHolder1Name { get; set; }
        public string Holder1Email { get; set; }
        public string Holder1Address { get; set; }


        //Join Account Fields
        public string AccountHolder2Name { get; set; }
        public string Holder2Email { get; set; }
        public string Holder2Address { get; set; }

        //Current Account Fields
        public string CompanyName { get; set; }
        public string GSTNo { get; set; }

    }
}
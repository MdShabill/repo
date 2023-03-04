using System.Collections;
using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO.BankAccountServicesDTO;

namespace WebApiDemo1.Repositories
{
    public interface IBankAccountServiceRepository
    {
        public int Add(BankAccount bankAccount);

        //public int CreateSavingsAccount(SavingsAccountDto savingsAccount);
        //public int CreateCurrentAccount(CurrentAccountDto currentAccount);
        //public int CreateJointAccount(JointAccountDto jointAccount);
    }
}

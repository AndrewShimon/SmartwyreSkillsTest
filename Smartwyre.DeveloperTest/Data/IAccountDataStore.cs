using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Data
{
    public interface IAccountDataStore
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}

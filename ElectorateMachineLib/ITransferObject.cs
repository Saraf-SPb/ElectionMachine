using ElectionMachine.Core.DB;
using ElectionMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ElectionMachine
{
    [ServiceContract]
    public interface ITransferObject
    {
        [OperationContract]
        int GetSum(int a, int b);
        [OperationContract]
        int GetMultiPly(int a, int b);
        [OperationContract]
        User Auth(string login, string password);
        [OperationContract]
        string AddElectorat(string fio, string phone, int userId, string baseName);
        [OperationContract]
        List<ElectorateWithUserName> GetAllElectorateList();
        [OperationContract]
        int Amount();
        [OperationContract]
        Electorate GetElectorate(int id);
    }
}

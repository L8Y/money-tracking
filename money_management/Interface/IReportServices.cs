﻿namespace money_management.Interface
{
    public interface IReportServices
    {
        public int AddTransaction(string Reason, int Amount, int balance, byte Transaction_type, DateTime currentDate, int userId, int User_BankId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdministration.Application.Common
{
    public class Enum
    {
        public struct ValidateMessages
        {
            public const string UserAlreadyExistsId = "User already exists with the same ID";
            public const string UserAlreadyExistsUsername = "User already exists with the same username";
            public const string UserAlreadyExistsEmail = "User already exists with the same email";

            public const string UsernameMandatory = "Username is a mandatory field";
            public const string UserNoExists = "User no exists";
            public const string UserDeleted = "User has been deleted";
        }

        public struct RowStates
        {
            public const string AvailableState = "1";
            public const string NotAvailableState = "0";
        }
    }
}

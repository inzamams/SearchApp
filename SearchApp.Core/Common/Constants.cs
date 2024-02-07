namespace SearchApp.Core.Common
{
    public static class Constants
    {
        public static class ProgramConstants
        {
            public const string LogFileName = "Logs/Log.log";
            public const string LogDateTimeFormat = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
        }

        public static class DapperConstants
        {
            public const string Connectionstring = "DefaultConnection";
        }

        public static class DateConstants
        {
            public static string AcceptedFormats = "MM/dd/yyyy hh:mm";
            public static string[] DateFormats = {"MM/dd/yyyy","M/dd/yyyy", "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                   "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                   "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                   "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                   "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
        }

        public static class SqlConstants
        {
            public static string Sp_GetFlightDetails = "sp_GetFlightDetails";
            public static string Sp_GetUserSearchHistory = "sp_GetUserSearchHistory";
            public static string Sp_InsertUser = "sp_InsertUser";
            public static string Sp_ValidateUser = "sp_ValidateUser";
        }

        public static class UserConstants
        {
            public static string UserId = "UserId";
        }

        public static class Messages
        {
            public static string UserRegistered = "User registered successfully. Please login to continue.";
            public static string UserNotRegistered = "User not registered. Please connect to system Administrator";
            public static string LoginSuccess = "Login successful. Please use this access token for calling other API's";
            public static string ReLogin = "Please login to continue.";
            public static string HistoryNotAvailable = "User search history not available.";
            public static string FlightNotAvailable = "Flight details not available.";
            public static string MendatoryFields = "Email and Password is mendatory";
        }
    }
}

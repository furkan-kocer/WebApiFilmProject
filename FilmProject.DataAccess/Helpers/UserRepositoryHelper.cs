using FilmProject.DataAccess.Entities;

namespace FilmProject.DataAccess.Helpers
{
    internal static class UserRepositoryHelper
    {
        internal static List<string> AnalyzeConflicts(List<User> conflictingUsers,User user)
        {
            var conflicts = new List<string>();
            if(conflictingUsers.Any())
            {
                const string emailExist = "Email already exist.";
                const string phoneExist = "PhoneNumber already exist.";
                foreach (var users in conflictingUsers)
                {
                    if (users.Email == user.Email && !(conflicts.Contains(emailExist)))
                        conflicts.Add(emailExist);
                    if (users.PhoneNumber == user.PhoneNumber && !(string.IsNullOrEmpty(user.PhoneNumber)) && !(conflicts.Contains(phoneExist)))
                        conflicts.Add(phoneExist);
                    if (conflicts.Contains(emailExist) && conflicts.Contains(phoneExist))
                        break;
                }
            }
            return conflicts;
        } 
    }
}

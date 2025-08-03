namespace ProjectManagementAPI.Models.Enum
{
    public class Enums
    {
        public enum UserRole
        {
            Admin=0,
            ProjectManager=1,
            Developer=2,
            Viewer=3
        }

        public enum ProjectStatus
        {
            NotStarted,
            InProgress,
            Completed,
            OnHold
        }

        public enum TaskStatus
        {
            NotStarted=0,
            InProgress=1,
            Completed=2
        }
    }

}


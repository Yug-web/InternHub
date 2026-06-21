namespace InternHub.Models
{
    /// <summary>
    /// Tracks the current status of an internship application.
    /// </summary>
    public enum ApplicationStatus
    {
        Applied,
        OAScheduled,
        OACompleted,
        InterviewScheduled,
        Rejected,
        Selected
    }

    /// <summary>
    /// Defines the type of an interview round.
    /// </summary>
    public enum InterviewType
    {
        Technical,
        HR,
        Coding
    }

    /// <summary>
    /// Represents the priority level of a deadline task.
    /// </summary>
    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
